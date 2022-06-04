using System;
using System.Collections.Generic;
using NCMB;
using TMPro;
using UnityEngine;

namespace UnityCustomRankingTemplate.Scripts
{
    using static RankingUtils;

    public class RankingManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private RankingRecord _rankingRecord;
        [SerializeField] private Transform _rankingContentsPanel;
        [SerializeField, Range(0, 500)] private float _scrollPadding = 100.0f;
        private readonly List<RankingRecord> _records = new List<RankingRecord>();

        private string _uniqueUserId = "";
        private bool _sendBusy = false;
        private bool _fetchBusy = false;

        private void Start()
        {
            // 初回起動時にPlayerPrefsを初期化
            InitPlayerPrefs();
        }

        /// <summary>
        /// 初回起動時にユーザデータを初期化する
        /// </summary>
        private void InitPlayerPrefs()
        {
            var uniqueId = "invalid id";
            // 初回起動時のみ初期化
            if (!PlayerPrefs.HasKey(UniqueUserIdKey))
            {
                // ユーザデータと紐づけるための一意ID
                Guid guid = Guid.NewGuid();
                uniqueId = guid.ToString();
                PlayerPrefs.SetString(UniqueUserIdKey, uniqueId);
                // デフォルトのユーザ名をセット
                PlayerPrefs.SetString(ClientUserNameKey, DefaultUserName);
                PlayerPrefs.Save();
            }
            else
            {
                uniqueId = PlayerPrefs.GetString(UniqueUserIdKey);
            }
            // 一意IDのキャッシュ
            _uniqueUserId = uniqueId;
        }

        /// <summary>
        /// ローカルデータ、データベースの名前を更新する
        /// </summary>
        public void ChangeName(string newName)
        {
            // ローカルデータ上のユーザ名を更新
            PlayerPrefs.SetString(ClientUserNameKey, newName);
            PlayerPrefs.Save();
            
            // データベース上のユーザ名を更新
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(NCMBStorageKey);
            // 一意IDに紐づいたデータを検索
            query.WhereEqualTo(UniqueUserIdKey, _uniqueUserId);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 取得に成功
                if (e == null)
                {
                    // 既にスコアデータが存在している場合のみ変更
                    if (objList.Count != 0)
                    {
                        objList[0][UserNameKey] = newName;
                        objList[0].SaveAsync();
                    }
                }
                else
                {
                    Debug.LogWarning($"ユーザ名の変更に失敗しました: {e}");
                }
            });
        }

        /// <summary>
        /// ランキングの送信
        /// </summary>
        public void SendRanking(int score)
        {
            // 重複送信防止
            if (_sendBusy)
            {
                Debug.Log("ランキングデータ送信中のため処理されません");
                return;
            }
            _sendBusy = true;
            
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(NCMBStorageKey);
            var userName = PlayerPrefs.GetString(ClientUserNameKey);

            // 一意IDに紐づいたデータを検索
            query.WhereEqualTo(UniqueUserIdKey, _uniqueUserId);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 取得に成功
                if (e == null)
                {
                    // 未登録のデータ
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject(NCMBStorageKey);
                        obj[UniqueUserIdKey] = _uniqueUserId;
                        obj[UserNameKey] = userName;
                        obj[HighScoreKey] = score;
                        obj.SaveAsync();
                    }
                    // ハイスコア更新の場合
                    // スコアを更新
                    else if (Convert.ToInt32(objList[0][HighScoreKey]) < score)
                    {
                        objList[0][HighScoreKey] = score;
                        objList[0].SaveAsync();
                    }
                }
                else
                {
                    Debug.LogWarning($"ランキングの送信に失敗しました: {e}");
                }
                _sendBusy = false;
            });
        }

        /// <summary>
        /// ランキングの取得
        /// </summary>
        public void FetchRanking()
        {
            // 重複取得防止
            if (_fetchBusy)
            {
                Debug.Log("ランキングデータ取得中のため処理されません");
                return;
            }
            _fetchBusy = true;
            
            // ランキングリストの初期化
            // 接続中を表示
            var rectTrans = _rankingContentsPanel.gameObject.GetComponent<RectTransform>();
            ClearRecords();
            _statusText.text = "Loading ...";

            // データの取得
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(NCMBStorageKey);
            query.OrderByDescending(HighScoreKey);
            query.Limit = MaxRecordNum;
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 取得に成功
                if (e == null)
                {
                    // データが存在しない場合
                    if (objList.Count == 0)
                    {
                        _statusText.text = "No Data";
                        _fetchBusy = false;
                        return;
                    }

                    var rank = 1;
                    // 取得したデータのリスト
                    for (int i = 0; i < objList.Count; i++)
                    {
                        // スコア(順位)が一緒の場合
                        if (i == 0 || objList[i][HighScoreKey].ToString() == objList[i - 1][HighScoreKey].ToString())
                        {
                        }
                        else
                        {
                            rank++;
                        }

                        // ランキングレコードの表示
                        var record = Instantiate(_rankingRecord, _rankingContentsPanel);
                        rectTrans.sizeDelta =
                            new Vector2(rectTrans.sizeDelta.x, rectTrans.sizeDelta.y + _scrollPadding);

                        // 自分のスコアかの判定
                        bool isMyRecord = objList[i][UniqueUserIdKey].ToString() == _uniqueUserId;

                        record.SetRecord(objList[i][UserNameKey].ToString(), Convert.ToInt32(objList[i][HighScoreKey]),
                            rank, isMyRecord);
                        _records.Add(record);
                        _statusText.text = "";
                    }
                }
                else
                {
                    _statusText.text = "Failed";
                    Debug.LogWarning($"ランキングの取得に失敗しました: {e}");
                }
                _fetchBusy = false;
            });
        }

        /// <summary>
        /// レコードのクリア
        /// </summary>
        private void ClearRecords()
        {
            foreach (var t in _records)
            {
                Destroy(t.gameObject);
            }

            _records.Clear();
        }
    }
}

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

        private void Start()
        {
            SetUniqueId();
            InitUserName();
        }

        /// <summary>
        /// ゲーム開始時に呼ぶ関数
        /// 一意IDのセット
        /// </summary>
        public void SetUniqueId()
        {
            // 初回起動時のみ処理
            if (!PlayerPrefs.HasKey(UniqueUserIdKey))
            {
                Guid guid = Guid.NewGuid();
                PlayerPrefs.SetString(UniqueUserIdKey, guid.ToString());
                PlayerPrefs.Save();
            }
        }
        
        private void InitUserName()
        {
            if (!PlayerPrefs.HasKey(ClientUserNameKey))
            {
                // デフォルトのユーザ名をセット
                PlayerPrefs.SetString(ClientUserNameKey, DefaultUserName);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// 名前に変更があった場合データベースの名前も変更
        /// </summary>
        public void ChangeName(string name)
        {
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(NCMBStorageKey);
            string uniqueId = PlayerPrefs.GetString(UniqueUserIdKey);
            // 一意IDに紐づいたデータを検索
            query.WhereEqualTo(UniqueUserIdKey, uniqueId);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 取得に成功
                if (e == null)
                {
                    // 既にスコアデータが存在している場合のみ変更
                    if (objList.Count != 0)
                    {
                        objList[0][UserNameKey] = name;
                        objList[0].SaveAsync();
                    }
                }
            });
        }

        /// <summary>
        /// ランキングの送信
        /// </summary>
        public void SendRanking(int score)
        {
            // 一意IDに紐づいたデータを検索
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(NCMBStorageKey);
            string uniqueId = PlayerPrefs.GetString(UniqueUserIdKey);
            string userName = PlayerPrefs.GetString(ClientUserNameKey);
            query.WhereEqualTo(UniqueUserIdKey, uniqueId);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 取得に成功
                if (e == null)
                {
                    // 未登録のデータ
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject(NCMBStorageKey);
                        obj[UniqueUserIdKey] = uniqueId;
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
            });
        }

        /// <summary>
        /// ランキングの取得
        /// </summary>
        public void FetchRanking()
        {
            var rectTrans = _rankingContentsPanel.gameObject.GetComponent<RectTransform>();
            ClearRecords();
            _statusText.text = "Loading ...";
            // ランキングリストの初期化
            // 接続中を表示
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
                        return;
                    }

                    int rank = 1;
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

                        // 情報の表示
                        RankingRecord record = Instantiate(_rankingRecord, _rankingContentsPanel);
                        rectTrans.sizeDelta =
                            new Vector2(rectTrans.sizeDelta.x, rectTrans.sizeDelta.y + _scrollPadding);

                        // 自分のスコアかの判定
                        bool isMyRecord = false;
                        if (PlayerPrefs.HasKey(UniqueUserIdKey))
                        {
                            isMyRecord = objList[i][UniqueUserIdKey].ToString() == PlayerPrefs.GetString(UniqueUserIdKey);
                        }

                        record.SetRecord(objList[i][UserNameKey].ToString(), Convert.ToInt32(objList[i][HighScoreKey]),
                            rank, isMyRecord);
                        _records.Add(record);
                        _statusText.text = "";
                    }
                }
                else
                {
                    _statusText.text = "Failed";
                }
            });
        }

        private void ClearRecords()
        {
            for (int i = 0; i < _records.Count; i++)
            {
                Destroy(_records[i].gameObject);
            }

            _records.Clear();
        }
    }
}

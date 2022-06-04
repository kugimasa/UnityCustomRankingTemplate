using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCustomRankingTemplate.Scripts
{
    public class RankingRecord : MonoBehaviour
    {
        [Header("バッジ素材"), SerializeField] private List<Sprite> _badgeSprites = new List<Sprite>();
        [Header("バッジカラー"), SerializeField] private List<Color> _badgeColors = new List<Color>();
        [Header("ランキングバッジのサイズ"), SerializeField] private List<float> _badgeSize = new List<float>();
        [Header("自分のランキングデータカラー"), SerializeField] private Color _myColor;

        [SerializeField] private GameObject _rankBadge;
        [SerializeField] private TextMeshProUGUI _dataText;
        [SerializeField] private TextMeshProUGUI _rankText;
        private string _userName;
        private int _score;

        /// <summary>
        /// ランキングデータをUIにセット
        /// </summary>
        public void SetRecord(string userName, int score, int rank, bool isMyRecord)
        {
            _userName = userName;
            _score = score;
            _dataText.text = $"{userName} : {score}";
            if (isMyRecord)
            {
                _dataText.color = _myColor;
            }

            SetRankingBadge(rank);
        }

        /// <summary>
        /// バッジの設定
        /// </summary>
        private void SetRankingBadge(int rank)
        {
            // 末尾のインデックスで初期化
            var rankIndex = _badgeSize.Count - 1;
            // 特殊な順位のインデックス
            if (rank <= rankIndex)
            {
                rankIndex = rank - 1;
            }
            // バッジサイズの設定
            var size = _badgeSize[rankIndex];
            _rankBadge.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            // バッジ素材の設定
            var badgeImage = _rankBadge.GetComponent<Image>();
            badgeImage.sprite =_badgeSprites[rankIndex];
            badgeImage.color = _badgeColors[rankIndex];
            // 順位テキストの設定
            _rankText.text = rank.ToString();
            _rankText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        }
        
        /// <summary>
        /// レコードの保持しているデータを返す
        /// </summary>
        /// <returns>{0}: _userName, {1}: _score</returns>
        public (string, int) GetRecordData()
        {
            return (_userName, _score);
        }

    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCustomRankingTemplate.Scripts
{
    public class RankingRecord : MonoBehaviour
    {
        [Header("バッジカラー"), SerializeField] private Color[] _badgeColors = new Color[4];

        [Header("ランキングバッジのサイズ"), SerializeField]
        private float[] _badgeSize = new float[4];

        [Header("自分のランキングデータカラー"), SerializeField]
        private Color _myColor;

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
            if (rank <= 3)
            {
                _rankBadge.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(_badgeSize[rank - 1], _badgeSize[rank - 1]);
                _rankText.gameObject.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(_badgeSize[rank - 1], _badgeSize[rank - 1]);
                _rankBadge.GetComponent<Image>().color = _badgeColors[rank - 1];
            }
            else
            {
                _rankBadge.GetComponent<RectTransform>().sizeDelta = new Vector2(_badgeSize[3], _badgeSize[3]);
                _rankText.gameObject.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(_badgeSize[3], _badgeSize[3]);
                _rankBadge.GetComponent<Image>().color = _badgeColors[3];
            }

            _rankText.text = rank.ToString();
        }

    }
}
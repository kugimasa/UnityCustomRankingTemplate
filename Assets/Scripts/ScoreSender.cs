using UnityEngine;

namespace UnityCustomRankingTemplate.Scripts
{
    public class ScoreSender : MonoBehaviour
    {

        [SerializeField] private RankingManager _rankingManager;
        [SerializeField] private int _score;
        
        /// <summary>
        /// インスペクターで指定したスコアをデータベースに送信する
        /// </summary>
        public void SendScore()
        {
            _rankingManager.SendRanking(_score);
        }
    }
}
namespace UnityCustomRankingTemplate.Scripts
{
    public static class RankingUtils
    {
        // PlayerPrefs用のKey
        internal static readonly string ClientUserNameKey = "PlayerName";

        // NCMB用のキー
        // NCMBのデータストアキー
        internal static readonly string NCMBStorageKey = "UnityCustomRanking";

        // レコードに対応するキー
        internal static readonly string UniqueUserIdKey = "UniqueUserId";
        internal static readonly string HighScoreKey = "HighScore";

        internal static readonly string UserNameKey = "UserName";

        // 一度に取得するレコードの件数
        internal static readonly int MaxRecordNum = 100;
    }
}
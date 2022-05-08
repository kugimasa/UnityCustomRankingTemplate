using TMPro;
using UnityEngine;

namespace UnityCustomRankingTemplate.Scripts
{
    using static RankingUtils;
    public class NameForm : MonoBehaviour
    {
        [SerializeField] private RankingManager _rankingManager;
        [SerializeField] private TMP_InputField _nameField;

        private void Start()
        {
            InitUserName();
        }

        /// <summary>
        /// ユーザ名の更新
        /// </summary>
        public void UpdateUserName()
        {
            string newName = _nameField.text;
            // ユーザ名をローカルデータにセット
            PlayerPrefs.SetString(ClientUserNameKey, newName);
            PlayerPrefs.Save();
            // NCMB上の名前を更新
            _rankingManager.ChangeName(newName);
        }

        /// <summary>
        /// ユーザ名入力欄の初期化
        /// </summary>
        private void InitUserName()
        {
            if (!PlayerPrefs.HasKey(ClientUserNameKey))
            {
                // デフォルトのユーザ名をセット
                PlayerPrefs.SetString(ClientUserNameKey, DefaultUserName);
                PlayerPrefs.Save();
                _nameField.text = DefaultUserName;
            }
            else
            {
                _nameField.text = PlayerPrefs.GetString(ClientUserNameKey);
            }
        }
    }
}

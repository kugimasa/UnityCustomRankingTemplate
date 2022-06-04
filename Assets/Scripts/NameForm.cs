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
            SetUserNameText();
        }

        /// <summary>
        /// ユーザ名の更新
        /// </summary>
        public void UpdateUserName()
        {
            string newName = _nameField.text;
            // ユーザ名を更新
            _rankingManager.ChangeName(newName);
        }

        /// <summary>
        /// ユーザ名入力欄にテキストをセット
        /// </summary>
        private void SetUserNameText()
        {
            if (!PlayerPrefs.HasKey(ClientUserNameKey))
            {
                _nameField.text = DefaultUserName;
            }
            else
            {
                _nameField.text = PlayerPrefs.GetString(ClientUserNameKey);
            }
        }
    }
}

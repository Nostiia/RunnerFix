using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

namespace Assets.RunnerFix.Scripts
{
    internal class DbManager: MonoBehaviour
    {
        [SerializeField] private InputField _email;
        [SerializeField] private InputField _password;
        [SerializeField] private InputField _userName;
        [SerializeField] private InputField _confirmation;
        private int score;
        private string _userID;

        private DatabaseReference _dbReference;
        void Start()
        {
            _userID = SystemInfo.deviceUniqueIdentifier;
            _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void CreateUser()
        {
            if (_password == _confirmation)
            {
                User newUser = new User(_email.text, _userName.text, 0);
                string json = JsonUtility.ToJson(newUser);

                _dbReference.Child("users").Child(_userID).SetRawJsonValueAsync(json);
            }
        }
    }
}

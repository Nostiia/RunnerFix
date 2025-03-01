using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private Transform _leaderboardContent;
    [SerializeField] private GameObject _leaderboardEntryPrefab;
    [SerializeField] private Text _currentNickname;
    [SerializeField] private Text _currentScore;

    private DatabaseReference _database;
    private FirebaseAuth _auth;
    private string _currentUserID;
    async void Start()
    {
        _mainMenuCanvas.SetActive(true);
        _leaderboardPanel.SetActive(false);


        await InitializeFirebase();
        // Check if a user is logged in and get the current user ID
        _currentUserID = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        
    }

    async System.Threading.Tasks.Task InitializeFirebase()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        await dependencyTask;

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            _auth = FirebaseAuth.DefaultInstance;

            if (_auth == null)
            {
                Debug.LogError("FirebaseAuth is null. Ensure Firebase is correctly configured.");
                return;
            }

            string databaseUrl = "https://runnerfix-e5c72-default-rtdb.firebaseio.com/";
            _database = FirebaseDatabase.GetInstance(app, databaseUrl).RootReference;

            Debug.Log("Firebase initialized successfully.");
        }
        else
        {
            Debug.LogError("Could not resolve Firebase dependencies: " + dependencyTask.Result);
        }
    }

    private void FetchLeaderboardData()
    {
        foreach (Transform child in _leaderboardContent)
        {
            Destroy(child.gameObject);
        }
        _database.Child("users")
            .OrderByChild("score")  
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to retrieve leaderboard data: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    List<(string username, int score)> leaderboardData = new List<(string, int)>();

                    if (snapshot.Exists)
                    {
                        foreach (var childSnapshot in snapshot.Children)
                        {
                            string userId = childSnapshot.Key; 
                            string username = childSnapshot.Child("username").Value.ToString();
                            int score = int.Parse(childSnapshot.Child("score").Value.ToString());
                            leaderboardData.Add((username, score));
                        }
                        leaderboardData.Sort((a, b) => b.score.CompareTo(a.score));
                        foreach (var entry in leaderboardData)
                        {
                            GameObject _newEntry = Instantiate(_leaderboardEntryPrefab, _leaderboardContent);
                            Debug.Log($"Instantiated: {entry.username} - {entry.score}");
                            Text _usernameText = _newEntry.transform.Find("UserName")?.GetComponent<Text>();
                            Text _scoreText = _newEntry.transform.Find("MaxScore")?.GetComponent<Text>();

                            if (_usernameText != null && _scoreText != null)
                            {
                                _usernameText.text = entry.username;
                                _scoreText.text = entry.score.ToString();
                            }
                            else
                            {
                                Debug.LogError("Could not find UsernameText or ScoreText in the prefab.");
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No users data found.");
                    }
                }
            });
    }

    private void FetchCurrentUserInfo()
    {
        if (_database == null)
        {
            Debug.LogError("Database reference is null!");
            return;
        }
        if (string.IsNullOrEmpty(_currentUserID))
        {
            Debug.LogError("Current user ID is null or empty!");
            return;
        }

        _database.Child("users").Child(_currentUserID).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to retrieve current user data: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        List<(string username, int score)> currentUserData = new List<(string, int)>();

                        string username = snapshot.Child("username").Value?.ToString();
                        int score = int.TryParse(snapshot.Child("score").Value?.ToString(), out int parsedScore) ? parsedScore : 0;

                        if (!string.IsNullOrEmpty(username))
                        {
                            currentUserData.Add((username, score));
                        }
                        else
                        {
                            Debug.LogError("Invalid data format for current user.");
                            return;
                        }

                        // Update UI
                        foreach (var entry in currentUserData)
                        {
                            _currentNickname.text = entry.username;
                            _currentScore.text = entry.score.ToString();
                        }
                    }
                    else
                    {
                        Debug.Log("Current user data not found.");
                    }
                }
            });
    }


    public void ShowLeaderboard()
    {
        if (_database == null)
        {
            Debug.LogError("Database reference is null! Firebase might not be initialized.");
            return;
        }
        MainMenuUIManager mainMenuUIManager = FindObjectOfType<MainMenuUIManager>();
        _leaderboardPanel.SetActive(true);
        mainMenuUIManager.LeaderboardPanel(true);
        _mainMenuCanvas.SetActive(false);
        FetchLeaderboardData();
        FetchCurrentUserInfo();
       
    }
    public void BackToMain()
    {
        MainMenuUIManager mainMenuUIManager = FindObjectOfType<MainMenuUIManager>();
        _leaderboardPanel.SetActive(false);
        
        _mainMenuCanvas.SetActive(true);
        mainMenuUIManager.LeaderboardPanel(false);

        StartCoroutine(mainMenuUIManager.DelayInputDetection());
        foreach (Transform child in _leaderboardContent)
        {
            Destroy(child.gameObject);
        }
    }
}

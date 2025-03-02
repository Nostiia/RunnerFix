using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.RunnerFix.Scripts;
using Firebase.Extensions;

public class LoseScreenUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameMenuCanvas;
    private ObstacleInteractor _interactor;
    [SerializeField]private DeathController _deathController;

    [SerializeField] private GameObject _loseScreenCanvas;
    private AdManager _adManager;

    private FirebaseAuth _auth;
    private DatabaseReference _database;

    private async void Start()
    {
        _loseScreenCanvas.SetActive(false);
        _adManager = FindObjectOfType<AdManager>();
        _auth = FirebaseAuth.DefaultInstance;
        await InitializeFirebase();
    }
    private async System.Threading.Tasks.Task InitializeFirebase()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        await dependencyTask;

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            _auth = FirebaseAuth.DefaultInstance;
            string databaseUrl = "https://runnerfix-e5c72-default-rtdb.firebaseio.com/";
            _database = FirebaseDatabase.GetInstance(app, databaseUrl).RootReference;

            Debug.Log("Firebase initialized successfully.");
        }
        else
        {
            Debug.LogError("Could not resolve Firebase dependencies: " + dependencyTask.Result);
        }
    }

    public void ActivateLoseScreen()
    {
        _loseScreenCanvas.SetActive(true);
        
    }
    private void SaveScoreToFirebase(int _score)
    {
        if (_database == null)
        {
            Debug.LogError("Database reference is null.");
            return;
        }

        FirebaseUser currentUser = _auth.CurrentUser;

        if (currentUser != null)
        {
            string _userId = currentUser.UserId;
            string _userEmail = currentUser.Email;
            string _userName = string.IsNullOrEmpty(currentUser.DisplayName) ? "Anonymous" : currentUser.DisplayName;

            DatabaseReference userScoreRef = _database.Child("users").Child(_userId).Child("score");

            userScoreRef.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    int _currentMaxScore = 0;
                    if (task.Result.Exists)
                    {
                        int.TryParse(task.Result.Value.ToString(), out _currentMaxScore);
                    }

                    if (_score > _currentMaxScore)
                    {
                        Dictionary<string, object> _scoreData = new Dictionary<string, object>
                    {
                        { "email", _userEmail },
                        { "username", _userName },
                        { "score", _score }
                    };

                        _database.Child("users").Child(_userId).SetValueAsync(_scoreData).ContinueWithOnMainThread(updateTask =>
                        {
                            if (updateTask.IsCompleted)
                            {
                                Debug.Log($"New high score {_score} saved for user: {_userName}");
                            }
                            else
                            {
                                Debug.LogError("Failed to save high score: " + updateTask.Exception);
                            }
                        });
                    }
                    else
                    {
                        Debug.Log($"Score {_score} is lower than current max {_currentMaxScore}. Not updating.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to retrieve score: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("No authenticated user found.");
        }
    }


    public void MainMenu()
    {
        int _finalScore = FindObjectOfType<ScoreManager>().GetScore();
        SaveScoreToFirebase(_finalScore);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShowAdForRespawn()
    {
        _adManager.ShowRewardedAd();
    }
    public void Respawn()
    {
        _interactor = _deathController.GetObstacle();
        _interactor.DestroyInteractorObstacle();
        Time.timeScale = 1f;
        _loseScreenCanvas.SetActive(false);
    }
}

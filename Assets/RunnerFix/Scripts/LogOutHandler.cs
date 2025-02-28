using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Extensions; // Required for ContinueWithOnMainThread()

public class LogOutHandler : MonoBehaviour
{
    private FirebaseAuth _auth;
    private bool _isFirebaseInitialized = false; // Flag to check initialization

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                _auth = FirebaseAuth.DefaultInstance;
                _isFirebaseInitialized = true; // Firebase is ready
            }
            else
            {
                Debug.LogError("Firebase dependencies not resolved: " + task.Result);
            }
        });
    }

    public void LogoutUser()
    {
        if (!_isFirebaseInitialized || _auth == null)
        {
            Debug.LogError("FirebaseAuth is not initialized yet. Please wait.");
            return;
        }

        if (_auth.CurrentUser != null)
        {
            _auth.SignOut();
            Debug.Log("User logged out successfully");

            // Ensure FirebaseAuth updates before switching scenes
            SceneManager.LoadScene("AuthScene");
        }
        else
        {
            Debug.LogWarning("No user is currently logged in.");
        }
    }
}


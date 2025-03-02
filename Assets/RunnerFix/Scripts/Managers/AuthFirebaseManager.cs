using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class AuthFirebaseManager : MonoBehaviour
{
    [SerializeField] private GameObject _loginPanel, _registerPanel; // UI panels

    [Header("Login Fields")]
    [SerializeField] private TMP_InputField _loginEmailInput, _loginPasswordInput; 
    [SerializeField] private Button _loginButton, _goToRegisterButton; 
    [SerializeField] private TextMeshProUGUI _loginError;

    [Header("Register Fields")]
    [SerializeField] private TMP_InputField _registerEmailInput, _registerUsernameInput, _registerPasswordInput, _registerConfirmPasswordInput;
    [SerializeField] private Button _registerButton, _goToLoginButton;
    [SerializeField] private TextMeshProUGUI _registerError;

    private FirebaseAuth _auth;
    private FirebaseUser _currentUser;
    private DatabaseReference _databaseReference;

    private async void Start()
    {
        _registerPanel.SetActive(false);
        _loginPanel.SetActive(true);

        await InitializeFirebase();

        ShowLoginPanel();
        CheckForExistingSession();
    }

    private async Task InitializeFirebase()
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
            _databaseReference = FirebaseDatabase.GetInstance(app, databaseUrl).RootReference;

            Debug.Log("Firebase initialized successfully.");
        }
        else
        {
            Debug.LogError("Could not resolve Firebase dependencies: " + dependencyTask.Result);
        }
    }


    public void ShowLoginPanel()
    {
        _loginPanel.SetActive(true);
        _registerPanel.SetActive(false);
    }

    public void ShowRegisterPanel()
    {
        _loginPanel.SetActive(false);
        _registerPanel.SetActive(true);
    }

    public async void LoginUser()
    {
        string email = _loginEmailInput.text.Trim();
        string password = _loginPasswordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            _loginError.text = "Email and password cannot be empty.";
            return;
        }

        try
        {
            var authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            _currentUser = authResult.User;

            Debug.Log("User logged in: " + _currentUser.Email);
            _loginError.text = "Login successful!";

            SceneManager.LoadScene("SampleScene");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Login Failed: " + e.Message);
            _loginError.text = "Login Failed: " + e.Message;
        }
    }

    public async void RegisterUser()
    {
        string email = _registerEmailInput.text.Trim();
        string username = _registerUsernameInput.text.Trim();
        string password = _registerPasswordInput.text.Trim();
        string confirmPassword = _registerConfirmPasswordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            _registerError.text = "All fields are required!";
            return;
        }

        if (password != confirmPassword)
        {
            _registerError.text = "Passwords do not match!";
            return;
        }

        try
        {
            var authResult = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser newUser = authResult.User;

            // Set display name
            UserProfile profile = new UserProfile { DisplayName = username };
            await newUser.UpdateUserProfileAsync(profile);
            await newUser.ReloadAsync(); // Ensure the profile is updated

            Debug.Log("User registered: " + newUser.Email);
            _registerError.text = "Registration successful!";

            // Store user data in Firebase Database
            var userData = new System.Collections.Generic.Dictionary<string, object>
            {
                { "username", username },
                { "email", email },
                { "score", 0 }
            };
            await _databaseReference.Child("users").Child(newUser.UserId).SetValueAsync(userData);

            Debug.Log("User data stored in Firebase Database");

            SceneManager.LoadScene("SampleScene");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Registration Failed: " + e.Message);
            _registerError.text = "Registration Failed: " + e.Message;
        }
    }

    private void CheckForExistingSession()
    {
        _currentUser = _auth.CurrentUser;

        if (_currentUser != null)
        {
            Debug.Log("Existing session found. Logged in as: " + _currentUser.Email);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.Log("No existing session. Show login panel.");
        }
    }
}


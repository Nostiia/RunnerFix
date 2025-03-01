using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _gameMenuCanvas;
    private bool _gameStarted = false;
    private bool _leaderboard = false;
    void Start()
    {
        _gameMenuCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);
    }
    private void Update()
    {
        if (!_gameStarted && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            if (!IsPointerOverUI() && !_leaderboard) 
            {
                StartGame();
            }
        }
    }
    public void LeaderboardPanel(bool activated)
    {
        _leaderboard = activated;
    }
    public void StartGame()
    {
        if (_gameStarted) return;

        _gameStarted = true;
        _gameMenuCanvas.SetActive(true);
        _mainMenuCanvas.SetActive(false);
        FindObjectOfType<PlayerMovement>().isPlaying = true;
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
    private bool IsPointerOverUI()
    {
        // For mouse input
        if (EventSystem.current.IsPointerOverGameObject()) return true;

        // For touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            PointerEventData eventData = new PointerEventData(EventSystem.current) { position = touch.position };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0; // Returns true if the touch is on a UI element
        }

        return false;
    }

    public IEnumerator DelayInputDetection()
    {
        _gameStarted = true; // Temporarily block input
        yield return new WaitForSeconds(0.3f); // Short delay
        _gameStarted = false; // Re-enable input detection
    }


}

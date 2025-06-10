using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    
    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    public GameObject playerControllerObject;
    
    private bool isPaused = false;
    
    public bool IsPaused() => isPaused;
    
    
    void Awake()
    {
        Instance = this;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (isPaused)
        {
            Debug.Log("Game Paused");
            Debug.Log("Cursor State: " + Cursor.lockState);
            Debug.Log("Cursor Visible: " + Cursor.visible);
        }

        
        if (playerControllerObject != null)
            playerControllerObject.SetActive(false);

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
            
        if (GameTimer.Instance != null)
            GameTimer.Instance.PauseTimer();
    }

    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; 
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
        if (playerControllerObject != null)
            playerControllerObject.SetActive(true);

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
            
        if (GameTimer.Instance != null)
            GameTimer.Instance.ResumeTimer();
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu"); 
    }
    
    public void ExitGame()
    {
        Time.timeScale = 1f; 
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
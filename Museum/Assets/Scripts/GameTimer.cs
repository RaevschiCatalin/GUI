using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }   
    public float totalTime = 600f;
    public TextMeshProUGUI timerText;
    
    float remainingTime;
    bool isRunning = true;
    bool isPaused = false; 
    
    void Awake() => Instance = this;                        
    
    void Start()
    {
        if (timerText == null)                               
            timerText = GetComponent<TextMeshProUGUI>();
        remainingTime = totalTime;
    }
    
    void Update()
    {
        if (!isRunning || isPaused) return; 
        
        remainingTime = Mathf.Max(0, remainingTime - Time.deltaTime);
        UpdateTimerDisplay();
        
        if (remainingTime <= 0)
        {
            isRunning = false;
            OnTimeUp();
        }
    }
    
    void UpdateTimerDisplay()
    {
        int m = Mathf.FloorToInt(remainingTime / 60f);
        int s = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{m:00}:{s:00}";
    }
    
    public void Stop() => isRunning = false;
    
    public void PauseTimer() => isPaused = true;
    
    public void ResumeTimer() => isPaused = false;
    
    void OnTimeUp() 
    {
        Debug.Log("Time's up!");
      
    }
}
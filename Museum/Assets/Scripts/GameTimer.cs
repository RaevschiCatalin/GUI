using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
     public float totalTime = 600f;
    public TextMeshProUGUI timerText;
    private float remainingTime;
    private bool isRunning = true;

    void Start()
    {
        remainingTime = totalTime;
    }

    void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(0, remainingTime);

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (remainingTime <= 0)
        {
            isRunning = false;
            OnTimeUp();
        }
    }

    void OnTimeUp()
    {
        Debug.Log("Time's up!");
        
    }
}

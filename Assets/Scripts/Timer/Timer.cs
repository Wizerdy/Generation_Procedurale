using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timer = 120f;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private bool isTimerActive = false;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = timer;
        text.text = timer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerActive) {
            currentTime -= Time.deltaTime;
            int timeInSeconds = (int)currentTime;
            text.text = timeInSeconds.ToString();
            if (currentTime < 0) {
                Debug.Log("GameOver");
                // GAME OVER
            }
        }
    }

    public void StartTimer(bool state) {
        isTimerActive = state;
    }

    public void StopTimer() {
        isTimerActive = false;
    }
}

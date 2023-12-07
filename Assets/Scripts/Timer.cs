using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timer;
    float seconds;
    float minutes;

    [SerializeField] TextMeshProUGUI text;

    public bool start;

    // Start is called before the first frame update
    void Start()
    {
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        TimerCalculator();
    }

    void TimerCalculator()
    {
        if (start)
        {
            timer += Time.deltaTime;
            seconds = (int)(timer % 60);
            minutes = (int)((timer / 60) % 60);

            text.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    public void stopTimer()
    {
        start = false;
    }
}
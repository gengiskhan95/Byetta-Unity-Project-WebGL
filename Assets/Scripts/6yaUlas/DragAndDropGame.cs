using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropGame : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject inGameStage;
    public GameObject endGameStage;
    public GameObject confetti;

    [Header("High Score")]
    public string highScorePlayerPrefsKey = "HighScore6yaUlas";

    [Header("Timer")]
    public Timer timer;

    [Header("Game Settings")]
    [SerializeField] private int totalNumbers = 6;

    public GameObject ButtonEndGame;

    private int inRightPositionNumbers = 0;

    public static DragAndDropGame instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        timer.timer = 0;
        LoadSavedHighScore();
    }

    private void LoadSavedHighScore()
    {
        float savedTimer = PlayerPrefs.GetFloat(highScorePlayerPrefsKey, 0);
        if (savedTimer > 0)
        {
            timer.timer += savedTimer;
        }
    }

    public void NumbersInRightPosition()
    {
        inRightPositionNumbers++;
        if (inRightPositionNumbers == totalNumbers)
        {
            ButtonEndGame.SetActive(false);
            StartConfetti();
            timer.stopTimer();
            SaveHighScore();
            Invoke("EndGame", 3f);
        }
    }

    private void OnApplicationQuit()
    {
        SaveData(); // Uygulama kapatýldýðýnda veriyi kaydet
    }

    private void OnDisable()
    {
        SaveData(); // Obje inaktif olduðunda veriyi kaydet
    }

    private void OnEnable()
    {
        LoadSavedData(); // Obje tekrar aktif olduðunda kaydedilen veriyi yükle
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(highScorePlayerPrefsKey, timer.timer);
        PlayerPrefs.Save();
    }

    private void LoadSavedData()
    {
        if (PlayerPrefs.HasKey(highScorePlayerPrefsKey))
        {
            float savedTimerValue = PlayerPrefs.GetFloat(highScorePlayerPrefsKey);
            timer.timer = savedTimerValue;
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetFloat(highScorePlayerPrefsKey, timer.timer);
        PlayerPrefs.Save();
    }

    private void EndGame()
    {
        confetti.SetActive(false);
        inGameStage.SetActive(false);
        endGameStage.SetActive(true);
    }

    private void StartConfetti()
    {
        confetti.SetActive(true);
    }
}

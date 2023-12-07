using UnityEngine;

public class WordGameController : MonoBehaviour
{
    [Header("Game Settings")]
    public int wordscount;

    public GameObject[] keyboardObjects;

    public Timer timer;

    [SerializeField]
    [Header("Player Prefs Key")]
    private string highscorePlayerPrefsKey = "HighScoreCaprazBulmaca";

    [Header("Game Objects")]
    public GameObject InGame;
    public GameObject EndGame;
    public ParticleSystem confetti;

    public GameObject ButtonEndGame;

    public static WordGameController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        timer.timer = PlayerPrefs.GetFloat(highscorePlayerPrefsKey, 0f);
    }

    public void EndGameController()
    {
        wordscount--;

        // Bütün klavye objelerini deaktif hale getir
        foreach (var keyboardIndex in keyboardObjects)
        {
            keyboardIndex.SetActive(false);
        }

        if (wordscount <= 0)
        {
            ButtonEndGame.SetActive(false);
            StartSparkle();
            timer.stopTimer();
            PlayerPrefs.SetFloat(highscorePlayerPrefsKey, timer.timer);
            Invoke("EndingGame", 3f);
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
        PlayerPrefs.SetFloat(highscorePlayerPrefsKey, timer.timer);
        PlayerPrefs.Save();
    }

    private void LoadSavedData()
    {
        if (PlayerPrefs.HasKey(highscorePlayerPrefsKey))
        {
            float savedTimerValue = PlayerPrefs.GetFloat(highscorePlayerPrefsKey);
            timer.timer = savedTimerValue;
        }
    }

    private void EndingGame()
    {
        StopSparkle();
        InGame.SetActive(false);
        EndGame.SetActive(true);
    }

    private void StartSparkle()
    {
        confetti.gameObject.SetActive(true);
        confetti.Play();
    }

    private void StopSparkle()
    {
        confetti.Stop();
        confetti.gameObject.SetActive(false);
    }
}

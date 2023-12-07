using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TypingManager : MonoBehaviour
{
    [Header("Settings")]
    public List<Word> words;
    public GameObject keyboard;
    public TextMeshProUGUI display;
    public TMP_InputField inputField;
    public Timer timer;
    public ParticleSystem stars;
    public GameObject finishStars;
    public GameObject inGame;
    public GameObject endGame;

    public GameObject ButtonEndGame;

    [SerializeField] private string highScoreKey = "HighScoreHarfleriDegistir";
    private Word correctWord;

    private void Start()
    {
        float getTimerBeforeQuit = PlayerPrefs.GetFloat(highScoreKey, 0);
    }

    private void Update()
    {
        string input = inputField.text;
        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        if (correctWord == null)
        {
            correctWord = words.Find(word => word.IsCorrectWord(input.ToUpper()));
            if (correctWord != null)
            {
                inputField.DeactivateInputField();
                inputField.interactable = false;
                Invoke("ClearInputField", 0.66f);
                StartSparkle();
                Debug.Log("Typed: " + correctWord.text.ToUpper());
                correctWord.onTyped.Invoke();
                inputField.interactable = true;
            }
        }
        else if (correctWord != null && correctWord.IsCorrectWord(input.ToUpper()))
        {
            words.Remove(correctWord);
            correctWord = null;
            if (words.Count == 0)
            {
                ButtonEndGame.SetActive(false);
                keyboard.SetActive(false);
                CancelInvoke("ClearInputField");
                inputField.interactable = false;
                inputField.enabled = false;
                finishStars.gameObject.SetActive(true);
                timer.stopTimer();
                PlayerPrefs.SetFloat(highScoreKey, timer.timer);
                PlayerPrefs.Save();
                Invoke("EndingGame", 3f);
            }
        }

        display.text = input.ToUpper();
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
        PlayerPrefs.SetFloat(highScoreKey, timer.timer);
        PlayerPrefs.Save();
    }

    private void LoadSavedData()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            float savedTimerValue = PlayerPrefs.GetFloat(highScoreKey);
            timer.timer = savedTimerValue;
        }
    }

    private void ClearInputField()
    {
        inputField.text = "";
        inputField.ActivateInputField();
    }

    private void StartSparkle()
    {
        stars.gameObject.SetActive(true);
        stars.Play();
    }

    private void StopSparkle()
    {
        stars.Stop();
        finishStars.gameObject.SetActive(false);
        stars.gameObject.SetActive(false);
    }

    private void EndingGame()
    {
        StopSparkle();
        inGame.SetActive(false);
        endGame.SetActive(true);
    }
}

[System.Serializable]
public class Word
{
    public string text;
    public UnityEvent onTyped;

    public bool IsCorrectWord(string type)
    {
        return type == text;
    }
}

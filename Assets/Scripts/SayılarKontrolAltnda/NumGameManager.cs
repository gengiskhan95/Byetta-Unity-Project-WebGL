using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumGameManager : MonoBehaviour
{
    [Header("Settings")]
    public int checkNumbers = 4;
    public int correctNumbers;

    [Header("PlayerPrefs Key")]
    public string playerPrefsKey = "HighScoreSayilarKontrolAltinda";

    [Header("UI Elements")]
    public GraphicRaycaster graphicRaycaster;
    public Canvas canvas;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;

    [SerializeField] GameObject frame_Grid;
    public GameObject frame_Numbers;
    public Transform frame_NumbersTransform;
    public RectTransform highlight;

    [Header("Confetti")]
    public GameObject confetti;

    [Header("Timer")]
    public Timer timer;

    [Header("Game Data")]
    public StaticNumberData theGameData;

    [Header("Panels")]
    public GameObject InGamePanel;
    public GameObject EndGamePanel;

    public GameObject ImageWords;
    public GameObject ButtonEndGame;

    private List<string> numbers = new List<string>();

    public List<string> numbersList = new List<string>()
    {
        "62982","18824","19748","66199",
    };

    public static NumGameManager instance;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < 10; i++)
        {
            string s = i.ToString();
            numbers.Add(s);
        }

        InitializeGrid();
        GenerateGame();
    }

    private void Start()
    {
        timer.timer = 0;
        float getTimerBeforeQuit = PlayerPrefs.GetFloat(playerPrefsKey, 0);
        if (getTimerBeforeQuit > 0)
        {
            timer.timer += getTimerBeforeQuit;
        }
    }

    void Update()
    {
        highlight.SetAsLastSibling();

        if (checkNumbers == correctNumbers)
        {
            ButtonEndGame.SetActive(false);
            StartConfetti();
            timer.stopTimer();
            PlayerPrefs.SetFloat(playerPrefsKey, timer.timer);
            PlayerPrefs.Save();
            Invoke("EndGame", 5f);
        }
    }

    [Header("Resources")]
    [SerializeField] UINumber frame_Character;

    public void InitializeGrid()
    {
        float yPos = 400;
        for (int y = 0; y < 9; y++)
        {
            float xPos = -400;
            for (int x = 0; x < 9; x++)
            {
                UINumber frame_Character = Instantiate(this.frame_Character, frame_Grid.transform);
                frame_Character.SetNumber("");
                frame_Character.SetPosition(new Vector2(x, y));
                frame_Character.transform.localPosition = new Vector2(xPos, yPos);
                frame_Character.text.transform.SetParent(frame_NumbersTransform, true);

                numberss[x, y] = frame_Character;

                xPos += 100;
            }

            yPos -= 100;
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
        PlayerPrefs.SetFloat(playerPrefsKey, timer.timer);
        PlayerPrefs.Save();
    }

    private void LoadSavedData()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            float savedTimerValue = PlayerPrefs.GetFloat(playerPrefsKey);
            timer.timer = savedTimerValue;
        }
    }

    public UINumber[,] numberss = new UINumber[9, 9];
    public string[,] game = new string[9, 9];

    public void GenerateGame()
    {
        int c = 0;

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                string s = game[x, y];
                if (game[x, y] == null)
                {
                    s = theGameData.GameData[c].ToString();
                }
                numberss[x, y].SetNumber(s);
                game[x, y] = theGameData.GameData[c].ToString();
                c++;
            }
        }
    }

    public void EndGame()
    {
        confetti.gameObject.SetActive(false);
        InGamePanel.SetActive(false);
        EndGamePanel.SetActive(true);
    }

    public void StartConfetti()
    {
        confetti.gameObject.SetActive(true);
    }
}

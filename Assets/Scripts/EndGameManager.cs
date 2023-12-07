using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class EndGameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private string timerPlayerPrefsKey = "timerPlayerPrefsKey"; // Inspector'den anahtar deðerini elle girebilirsiniz
    [SerializeField] private float timer = 0; // Inspector'den baþlangýç deðerini elle girebilirsiniz

    [Header("Game Info")]
    [SerializeField] private string email;
    [SerializeField] private string gameName = "gameName";

    [Header("URLs")]
    [SerializeField] private string postPlayedUrl = "https://www.astrazeneca724.tv/services/played.php";
    [SerializeField] private string postAddScoreUrl = "https://www.astrazeneca724.tv/services/add_score.php";

    //[Header("URLs")]
    //[SerializeField] private string postPlayedUrl = "https://astra724tv.vidizayn.com/services/played.php";
    //[SerializeField] private string postAddScoreUrl = "https://astra724tv.vidizayn.com/services/add_score.php";

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI textToInfo;
    [SerializeField] private TextMeshProUGUI textToHighScore;

    public bool gamePlayed = false;

    private void Awake()
    {
        timer = PlayerPrefs.GetFloat(timerPlayerPrefsKey, timer); // PlayerPrefs ile baþlangýç deðerini alýr
    }

    private void OnEnable()
    {
        DetermineAndPostGameData();
    }

    private void DetermineAndPostGameData()
    {
        if (timer > 0)
        {
            DisplayTimer();
            email = PlayerPrefs.GetString("email");
            StartCoroutine(PostGameScore());
        }
    }

    private void DisplayTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        textToInfo.text = "Tebrikler " + minutes + " dakika " + seconds + " saniyede oyunu bitirdiniz.";
        textToHighScore.text = $"{minutes:00}:{seconds:00}";
    }

    private IEnumerator PostGameScore()
    {
        yield return StartCoroutine(PostData(postPlayedUrl, new Dictionary<string, string>
        {
            { "email", email },
            { "oyun_adi", gameName }
        }));

        yield return StartCoroutine(PostData(postAddScoreUrl, new Dictionary<string, string>
        {
            { "email", email },
            { "oyun_adi", gameName },
            { "skor", Mathf.FloorToInt(timer).ToString() }
        }));
    }

    private IEnumerator PostData(string url, Dictionary<string, string> formData)
    {
        WWWForm form = new WWWForm();
        foreach (var field in formData)
        {
            form.AddField(field.Key, field.Value);
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Data Sent Successfully!");
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
        PlayerPrefs.DeleteKey(timerPlayerPrefsKey);
        gamePlayed = true;
    }
}

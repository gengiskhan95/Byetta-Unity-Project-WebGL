using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Linq;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI[] HighScoreNames;
    public TextMeshProUGUI[] HighScores;

    public int seconds;
    public int minutes;

    public string getUrl = "https://www.astrazeneca724.tv/services/get_score.php";
    //public string getUrl = "https://astra724tv.vidizayn.com/services/get_score.php";

    [System.Serializable]
    public class Oyun
    {
        public string isim;
        public int skor;
    }

    [System.Serializable]
    public class ScoresList
    {
        public Oyun[] oyun;
    }

    private ScoresList myScoresList = new ScoresList();

    IEnumerator GetRequest(string oyunadi)
    {
        WWWForm form = new WWWForm();
        form.AddField("oyun_adi", oyunadi);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(getUrl, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string rawresponse = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                Debug.Log(rawresponse);

                if (string.IsNullOrEmpty(rawresponse) || rawresponse.Trim() == "[]" || rawresponse == "0")
                {
                    ClearHighScores();
                    Debug.Log("Received JSON data is empty or an empty array.");
                }

                else
                {
                    try
                    {
                        myScoresList = JsonUtility.FromJson<ScoresList>(rawresponse);
                        UpdateHighScores();
                    }
                    catch (System.Exception)
                    {
                        Debug.LogError("JSON parsing error: The received data is not valid JSON.");
                    }
                }
            }
            else
            {
                Debug.LogError("Network request error: " + webRequest.error);
            }
        }
    }

    void ClearHighScores()
    {
        for (int i = 0; i < HighScoreNames.Length; i++)
        {
            HighScoreNames[i].text = " ";
            HighScores[i].text = " ";
        }
    }

    void UpdateHighScores()
    {
        ClearHighScores();

        var sortedScores = myScoresList.oyun.OrderBy(entry => entry.skor).ToArray();

        // Sýralanmýþ skorlarý yazdýr
        for (int i = 0; i < Mathf.Min(sortedScores.Length, HighScoreNames.Length); i++)
        {
            int score = sortedScores[i].skor;
            int seconds = score % 60;
            int minutes = (score / 60) % 60;

            HighScoreNames[i].text = (i + 1) + ". " + sortedScores[i].isim;
            HighScores[i].text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    public void CaprazBulmaca()
    {
        StartCoroutine(GetRequest("CaprazBulmaca"));
    }

    public void EkleyeEkleyeBuyukResme()
    {
        StartCoroutine(GetRequest("EkleyeEkleyeBR"));
    }

    public void HarfleriDegistir()
    {
        StartCoroutine(GetRequest("HarfleriDegistir"));
    }

    public void BenzerHastalariBul()
    {
        StartCoroutine(GetRequest("BenzerHastalariBul"));
    }

    public void SayilariKontrolAltinaAl()
    {
        StartCoroutine(GetRequest("SayilarKontrolAltinda"));
    }

    public void AltiyaUlas()
    {
        StartCoroutine(GetRequest("6yaUlas"));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GameSelectorScript : MonoBehaviour
{
    public string email;
    public string postplayedUrl = "https://www.astrazeneca724.tv/services/is_played.php";
    //public string postplayedUrl = "https://astra724tv.vidizayn.com/services/is_played.php";

    public Button game1;
    public Button game2;
    public Button game3;
    public Button game4;
    public Button game5;
    public Button game6;

    public GameObject game1ok;
    public GameObject game2ok;
    public GameObject game3ok;
    public GameObject game4ok;
    public GameObject game5ok;
    public GameObject game6ok;



    private void Awake()
    {
        email = PlayerPrefs.GetString("email");
        StartCoroutine(HandleGameSelection());

        //webRequest.GetRequestHeader
    }

    IEnumerator HandleGameSelection()
    {
        yield return StartCoroutine(PostPlayed("CaprazBulmaca"));
        yield return StartCoroutine(PostPlayed("EkleyeEkleyeBR"));
        yield return StartCoroutine(PostPlayed("HarfleriDegistir"));
        yield return StartCoroutine(PostPlayed("BenzerHastalariBul"));
        yield return StartCoroutine(PostPlayed("SayilarKontrolAltinda"));
        yield return StartCoroutine(PostPlayed("6yaUlas"));

        // Coroutine'ler tamamlandýðýnda buraya geleceðiz
        // Bu noktadan sonra kullanýcýnýn tuþa basmasýna izin verebilirsiniz
    }


    IEnumerator PostPlayed(string oyunadi)
    {
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("oyun_adi", oyunadi);

        using (UnityWebRequest w = UnityWebRequest.Post(postplayedUrl, form))
        {
            yield return w.SendWebRequest();

            while (!w.isDone)
            {
                yield return null;
            }

            if (w.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(w.error);
            }
            else
            {
                Debug.Log("Data Send Successfully!");

                //string str = System.Text.Encoding.Default.GetString(w.downloadHandler.data);
                string state = w.downloadHandler.text;
                Debug.Log(state);
                if (state.Contains("1") || state.Contains("true"))
                {
                    if (oyunadi == "CaprazBulmaca")
                    {
                        game1ok.SetActive(true);
                        game1.interactable = false;
                    }

                    else if (oyunadi == "EkleyeEkleyeBR")
                    {
                        game2ok.SetActive(true);
                        game2.interactable = false;
                    }

                    else if (oyunadi == "HarfleriDegistir")
                    {
                        game3ok.SetActive(true);
                        game3.interactable = false;
                    }

                    else if (oyunadi == "BenzerHastalariBul")
                    {
                        game4ok.SetActive(true);
                        game4.interactable = false;
                    }

                    else if (oyunadi == "SayilarKontrolAltinda")
                    {
                        game5ok.SetActive(true);
                        game5.interactable = false;
                    }

                    else if (oyunadi == "6yaUlas")
                    {
                        game6ok.SetActive(true);
                        game6.interactable = false;
                    }
                }
            }
        }
    }
}
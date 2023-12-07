using UnityEngine;

public class LogIn : MonoBehaviour
{
    public string email;

    void Start()
    {
        CheckURLForEmail();
        SaveEmailToPlayerPrefs();
    }

    private void CheckURLForEmail()
    {
        string url = Application.absoluteURL;
        int questionMarkIndex = url.IndexOf("?");

        if (questionMarkIndex != -1)
        {
            string[] urlParts = url.Split('=');
            if (urlParts.Length > 1)
            {
                string useremail = urlParts[1];
                Debug.Log("User email: " + useremail);

                email = useremail;
                Debug.Log("Email: " + email);
            }
        }
    }

    private void SaveEmailToPlayerPrefs()
    {
        PlayerPrefs.SetString("email", email);
        Debug.Log("Saved email: " + PlayerPrefs.GetString("email"));
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.Networking;
//using System;

//public class LogIn : MonoBehaviour
//{
//    public string email;
//    public string postURL = "https://astra724tv.vidizayn.com/oyun?email=YWxpLngucGFyaWx0aUBnbWFpbC5jb20==";

//    void Start()
//    {
//        int pm = postURL.IndexOf("?");
//        if (pm != -1)
//        {
//            string useremail = postURL.Split('=')[1];
//            Debug.Log("usermail = " + useremail);

//            email = useremail;
//            PlayerPrefs.SetString("email", email);
//            Debug.Log(PlayerPrefs.GetString("email"));
//        }
//    }

//    //public class getCheckGames
//    //{
//    //    public string statement;
//    //}
//}


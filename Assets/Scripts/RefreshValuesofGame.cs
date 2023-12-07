using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshValuesofGame : MonoBehaviour
{
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

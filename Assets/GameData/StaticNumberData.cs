using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Number Game Data", menuName = "Sayýlar Kontrol Altýnda")]
public class StaticNumberData : ScriptableObject
{
    public List<string> GameData = new List<string>();
}

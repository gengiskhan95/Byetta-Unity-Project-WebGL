using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [SerializeField] private string word;

    public List<CellController.State> GetStates(string msg)
    {
        var result = new List<CellController.State>();

        if (msg.Length != word.Length)
        {
            // Harf sayıları eşit değilse direkt olarak hatalı kabul edebilirsiniz.
            for (int i = 0; i < msg.Length; i++)
            {
                result.Add(CellController.State.Fail);
            }
        }
        else
        {
            // Harf sayıları eşitse sırayla kontrol et.
            for (int i = 0; i < msg.Length; i++)
            {
                char currentChar = msg[i];
                if (currentChar == word[i])
                {
                    result.Add(CellController.State.Correct);
                }
                else
                {
                    result.Add(CellController.State.Fail);
                }
            }
        }

        return result;
    }
}

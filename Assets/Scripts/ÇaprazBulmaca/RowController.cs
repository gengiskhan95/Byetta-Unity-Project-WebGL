using System.Collections.Generic;
using UnityEngine;

public class RowController : MonoBehaviour
{
    [SerializeField] private List<CellController> cells = new List<CellController>();

    public int CellAmount => cells.Count;

    public void UpdateText(string msg)
    {
        char[] arrayChar = msg.ToCharArray();

        for (int i = 0; i < cells.Count; i++)
        {
            char content = i < arrayChar.Length ? arrayChar[i] : ' ';
            cells[i].UpdateText(content);
        }
    }

    public void UpdateState(List<CellController.State> states)
    {
        for (int i = 0; i < cells.Count && i < states.Count; i++)
        {
            CellController.State state = states[i];
            cells[i].UpdateState(state);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum DirectionCheck
{
    Horizontal,
    Vertical,
    Cross,
    None
}

public class UINumber : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int numbersCheck;

    public TextMeshProUGUI text;

    Vector2 pos = Vector2.one * -1;

    public void SetNumber(string s)
    {
        text.text = s;
    }

    public void SetPosition(Vector2 pos)
    {
        this.pos = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateHighlight(eventData, out string number);

        Debug.Log(number);

        if (NumGameManager.instance.numbersList.Contains(number))
        {
            if (number == NumGameManager.instance.numbersList[0])
            {
                NumGameManager.instance.text1.fontStyle = FontStyles.Strikethrough;
            }
            else if (number == NumGameManager.instance.numbersList[1])
            {
                NumGameManager.instance.text2.fontStyle = FontStyles.Strikethrough;
            }
            else if (number == NumGameManager.instance.numbersList[2])
            {
                NumGameManager.instance.text3.fontStyle = FontStyles.Strikethrough;
            }
            else if (number == NumGameManager.instance.numbersList[3])
            {
                NumGameManager.instance.text4.fontStyle = FontStyles.Strikethrough;
            }

            CopyHighlight();
            Debug.Log("Correct");
            NumGameManager.instance.correctNumbers += 1;
        }

        array.Clear();

        NumGameManager.instance.highlight.sizeDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateHighlight(eventData, out _);
    }

    void UpdateHighlight(PointerEventData eventData, out string number)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        NumGameManager.instance.graphicRaycaster.Raycast(eventData, raycastResults);

        UINumber start = this;
        UINumber end = null;

        // raycastResults listesi boþ deðilse ve en az bir sonuç varsa end deðiþkenine doðru deðeri atayýn
        if (raycastResults.Count > 0)
        {
            end = raycastResults[0].gameObject.GetComponent<UINumber>();
        }

        // end deðiþkeni null ise veya yönde geçerli bir iþlem yapýlamýyorsa
        if (end == null || !IsValidDirection(start.pos, end.pos, out _))
        {
            number = null;
            return;
        }

        Vector2 a = start.transform.localPosition;
        Vector2 b = end.transform.localPosition;

        NumGameManager.instance.highlight.transform.localPosition = (a + b) / 2;
        NumGameManager.instance.highlight.sizeDelta = new Vector2(100 + Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2)), 100);

        NumGameManager.instance.highlight.right = a - b;

        number = GetNumber(start.pos, end.pos);
    }

    bool IsValidDirection(Vector2 a, Vector2 b, out DirectionCheck direction)
    {
        bool flag1 = a.x == b.x;
        bool flag2 = a.y == b.y;

        Vector2 crossCheck = (a - b).normalized;
        crossCheck.x = Mathf.Abs(crossCheck.x);
        crossCheck.y = Mathf.Abs(crossCheck.y);

        bool flag3 = crossCheck == Vector2.one * Mathf.Sqrt(2) * 0.5f;

        if (flag2)
            direction = DirectionCheck.Horizontal;
        else if (flag1)
            direction = DirectionCheck.Vertical;
        else if (flag3)
            direction = DirectionCheck.Cross;
        else
            direction = DirectionCheck.None;

        return flag1 || flag2 || flag3;
    }

    string GetNumber(Vector2 a, Vector2 b)
    {
        if (!IsValidDirection(a, b, out DirectionCheck direction))
            return null;

        string number = "";

        if (direction == DirectionCheck.Horizontal)
        {
            if (a.x < b.x)
            {
                for (int x = (int)Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
                {
                    number += NumGameManager.instance.numberss[x, (int)a.y].text.text;
                }
            }
            else if (a.x > b.x)
            {
                for (int x = (int)Mathf.Max(a.x, b.x); x >= Mathf.Min(a.x, b.x); x--)
                {
                    number += NumGameManager.instance.numberss[x, (int)a.y].text.text;
                }
            }
        }
        else if (direction == DirectionCheck.Vertical)
        {
            if (a.y < b.y)
            {
                for (int y = (int)Mathf.Min(a.y, b.y); y <= Mathf.Max(a.y, b.y); y++)
                {
                    number += NumGameManager.instance.numberss[(int)a.x, y].text.text;
                }
            }
            else if (a.y > b.y)
            {
                for (int y = (int)Mathf.Max(a.y, b.y); y >= Mathf.Min(a.y, b.y); y--)
                {
                    number += NumGameManager.instance.numberss[(int)a.x, y].text.text;
                }
            }
        }

        if (direction == DirectionCheck.Cross)
        {
            if (a.x == a.y && b.x == b.y && a.x < b.x && a.y < b.y)
            {
                for (int x = (int)Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
                {
                    number += NumGameManager.instance.numberss[x, x].text.text;
                }
            }

            else if (a.x > a.y && b.x > b.y && b.x - a.x == b.y - a.y && a.x - b.x < 0 && a.y - b.y < 0)
            {
                float diffy = a.y - a.x;

                for (int x = (int)Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
                {
                    number += NumGameManager.instance.numberss[x, x + (int)diffy].text.text;
                }
            }

            else if (a.x < a.y && b.x < b.y && b.x - a.x == b.y - a.y && a.x - b.x < 0 && a.y - b.y < 0)
            {
                float diffy = a.y - a.x;

                for (int x = (int)Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
                {
                    number += NumGameManager.instance.numberss[x, x + (int)diffy].text.text;
                }
            }

            if (a.x == a.y && b.x == b.y && a.x > b.x && a.y > b.y)
            {
                for (int x = (int)Mathf.Max(a.x, b.x); x >= Mathf.Min(a.x, b.x); x--)
                {
                    number += NumGameManager.instance.numberss[x, x].text.text;
                }
            }

            else if (a.x > a.y && b.x > b.y && b.x - a.x == b.y - a.y && a.x - b.x > 0 && a.y - b.y > 0)
            {
                float diffy = a.y - a.x;

                for (int x = (int)Mathf.Max(a.x, b.x); x >= Mathf.Min(a.x, b.x); x--)
                {
                    number += NumGameManager.instance.numberss[x, x + (int)diffy].text.text;
                }
            }

            else if (a.x < a.y && b.x < b.y && b.x - a.x == b.y - a.y && a.x - b.x > 0 && a.y - b.y > 0)
            {
                float diffy = a.y - a.x;

                for (int x = (int)Mathf.Max(a.x, b.x); x >= Mathf.Min(a.x, b.x); x--)
                {
                    number += NumGameManager.instance.numberss[x, x + (int)diffy].text.text;
                }
            }

            if (a.x >= b.x && a.y <= b.y && a.x + a.y == b.x + b.y && a.x - b.x >= 0 && a.y - b.y <= 0 && a.x + a.y == 8)
            {
                float diffy = a.y;

                for (int x = (int)Mathf.Max(a.x, b.x); x >= Mathf.Min(a.x, b.x); x--)
                {
                    number += NumGameManager.instance.numberss[x, (int)diffy].text.text;
                    diffy += 1f;
                }
            }

            else if (a.x >= b.x && a.y <= b.y && a.x + a.y == b.x + b.y && a.x - b.x >= 0 && a.y - b.y <= 0 && a.x + a.y < 8)
            {
                float diffy = a.y;

                for (int x = (int)Mathf.Max(a.x, b.x); x >= Mathf.Min(a.x, b.x); x--)
                {
                    number += NumGameManager.instance.numberss[x, (int)diffy].text.text;
                    diffy += 1f;
                }
            }

            else if (a.x >= b.x && a.y <= b.y && a.x + a.y == b.x + b.y && a.x - b.x >= 0 && a.y - b.y <= 0 && a.x + a.y > 8)
            {
                float diffy = a.y;

                for (int x = (int)Mathf.Max(a.x, b.x); x >= Mathf.Min(a.x, b.x); x--)
                {
                    number += NumGameManager.instance.numberss[x, (int)diffy].text.text;
                    diffy += 1f;
                }
            }

            if (a.x <= b.x && a.y >= b.y && a.x + a.y == b.x + b.y && a.x - b.x <= 0 && a.y - b.y >= 0 && a.x + a.y == 8)
            {
                float diffy = a.y;

                for (int x = (int)Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
                {
                    number += NumGameManager.instance.numberss[x, (int)diffy].text.text;
                    diffy -= 1f;
                }
            }

            else if (a.x <= b.x && a.y >= b.y && a.x + a.y == b.x + b.y && a.x - b.x <= 0 && a.y - b.y >= 0 && a.x + a.y < 8)
            {
                float diffy = a.y;

                for (int x = (int)Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
                {
                    number += NumGameManager.instance.numberss[x, (int)diffy].text.text;
                    diffy -= 1f;
                }
            }

            else if (a.x <= b.x && a.y >= b.y && a.x + a.y == b.x + b.y && a.x - b.x <= 0 && a.y - b.y >= 0 && a.x + a.y > 8)
            {
                float diffy = a.y;

                for (int x = (int)Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
                {
                    number += NumGameManager.instance.numberss[x, (int)diffy].text.text;
                    diffy -= 1f;
                }
            }

        }

        return number;
    }

    public List<string> array = new List<string>();

    List<Color> highlightColors = new List<Color>()
    {
        new Color32(101, 217, 143, 255),
        new Color32(101, 217, 143, 255),
    };

    void CopyHighlight()
    {
        Image image = Instantiate(NumGameManager.instance.highlight, NumGameManager.instance.frame_Numbers.transform).GetComponent<Image>();
        image.color = highlightColors[Random.Range(0, highlightColors.Count)];
        image.transform.position = NumGameManager.instance.highlight.position;
        image.transform.SetAsFirstSibling();
    }
}



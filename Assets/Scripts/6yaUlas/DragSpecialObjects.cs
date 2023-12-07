using UnityEngine;

public class DragSpecialObjects : MonoBehaviour
{
    [SerializeField]
    private int countid;

    [Header("Special Number Objects")]
    [SerializeField]
    private GameObject Num130;

    [SerializeField]
    private GameObject Num270;

    private NumbersSlot numbersSlot;
    private DragAndDrop dragAndDrop;

    public static DragSpecialObjects instance;

    private void Awake()
    {
        instance = this;
        numbersSlot = NumbersSlot.instance;
        dragAndDrop = DragAndDrop.instance;
    }

    public void EnableSpecialNumber()
    {
        countid++;
        if (countid == 2)
        {
            Num130.GetComponent<DragAndDrop>().enabled = true;
        }
        else if (countid == 4)
        {
            Num270.GetComponent<DragAndDrop>().enabled = true;
        }
    }
}

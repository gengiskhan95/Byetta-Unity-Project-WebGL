using UnityEngine;
using TMPro;

public class CellController : MonoBehaviour
{
    [SerializeField] private Color colorNone = Color.white;
    [SerializeField] private Color colorCorrect = Color.green;
    [SerializeField] private Color colorFail = Color.red;

    private TextMeshProUGUI text;
    private Animator animator;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponentInChildren<Animator>();
    }

    public void UpdateText(char msg)
    {
        text.SetText(msg.ToString().ToUpper());
    }

    public void UpdateState(State state)
    {
        text.color = GetColor(state);

        switch (state)
        {
            case State.Correct:
                animator.SetTrigger("CorrectTrig");
                break;
            case State.Fail:
                animator.SetTrigger("WrongTrig");
                break;
        }
    }

    private Color GetColor(State state)
    {
        return state switch
        {
            State.None => colorNone,
            State.Correct => colorCorrect,
            State.Fail => colorFail,
            _ => colorNone, // Fallback to colorNone if the state is not recognized
        };
    }

    public enum State
    {
        None,
        Correct,
        Fail,
    }
}

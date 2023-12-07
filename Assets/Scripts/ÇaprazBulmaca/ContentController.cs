using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ContentController : MonoBehaviour
{
    [SerializeField] private List<RowController> rows;
    [SerializeField] private WordManager wordManager;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private ParticleSystem sparkle;

    private WordGameController wordGameController;
    private int currentIndex;

    private void Start()
    {
        wordGameController = WordGameController.instance;
        inputField.onValueChanged.AddListener(OnUpdateContent);
        inputField.onSubmit.AddListener(OnSubmit);
    }

    private void OnUpdateContent(string msg)
    {
        var currentRow = rows[currentIndex];
        currentRow.UpdateText(msg);
    }

    private bool UpdateState()
    {
        var states = wordManager.GetStates(inputField.text.ToUpper());
        rows[currentIndex].UpdateState(states);

        foreach (var state in states)
        {
            if (state != CellController.State.Correct)
                return false;
        }

        return true;
    }

    private void AddListeners()
    {
        inputField.interactable = true;
        inputField.onValueChanged.AddListener(OnUpdateContent);
        inputField.onSubmit.AddListener(OnSubmit);
    }

    private void RemoveListeners()
    {
        inputField.interactable = false;
        inputField.onValueChanged.RemoveListener(OnUpdateContent);
        inputField.onSubmit.RemoveListener(OnSubmit);
    }

    private void OnSubmit(string msg)
    {
        inputField.DeactivateInputField();

        if (!IsEnough())
        {
            Debug.Log("Yeterli Değil");
            ClearInputField();
            inputField.ActivateInputField();
            return;
        }

        var isWin = UpdateState();

        RemoveListeners();

        if (isWin)
        {
            StartSparkle();
            Invoke("StopSparkle", 1.5f);
            question.fontStyle = FontStyles.Strikethrough;
            Debug.Log("Kelimeyi Doğru Bildin");
            wordGameController.EndGameController();
            inputField.interactable = false;
        }
        else
        {
            Invoke("ClearInputField", 2f);
            inputField.ActivateInputField();
        }

        AddListeners();
    }

    private void ClearInputField()
    {
        inputField.text = "";
        inputField.ActivateInputField();
    }

    private bool IsEnough()
    {
        return inputField.text.Length == rows[currentIndex].CellAmount;
    }

    private void StartSparkle()
    {
        sparkle.gameObject.SetActive(true);
        sparkle.Play();
    }

    private void StopSparkle()
    {
        sparkle.Stop();
        sparkle.gameObject.SetActive(false);
    }
}

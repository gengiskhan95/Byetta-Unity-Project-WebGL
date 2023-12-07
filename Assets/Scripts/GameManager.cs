using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject endGamePanel;

    [Header("PlayerPrefs Key")]
    [SerializeField] private string playerPrefsKey = "playerPrefsKey";

    [SerializeField] EndGameManager EGM;

    private float timer;

    private void Start()
    {
        timer = PlayerPrefs.GetFloat(playerPrefsKey, 0);
        DetermineAndSetGameState();
    }

    private void OnEnable()
    {
        timer = PlayerPrefs.GetFloat(playerPrefsKey, 0);
        DetermineAndSetGameState();
    }

    private void DetermineAndSetGameState()
    {
        if (EGM.gamePlayed == true)
        {
            ShowPanel(endGamePanel);
        }
        else if (IsGameInProgress())
        {
            ShowPanel(inGamePanel);
        }
        else if (IsGameEnded())
        {
            ShowPanel(endGamePanel);
        }
        else
        {
            ShowPanel(startPanel);
        }
    }

    private bool IsGameInProgress()
    {
        return inGamePanel != null && inGamePanel.activeSelf;
    }

    private bool IsGameEnded()
    {
        return endGamePanel != null && endGamePanel.activeSelf && timer > 0;
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (panelToShow != null)
        {
            startPanel.SetActive(panelToShow == startPanel);
            inGamePanel.SetActive(panelToShow == inGamePanel);
            endGamePanel.SetActive(panelToShow == endGamePanel);
        }
        else
        {
            Debug.LogError("Panel to show is null!");
        }
    }
}

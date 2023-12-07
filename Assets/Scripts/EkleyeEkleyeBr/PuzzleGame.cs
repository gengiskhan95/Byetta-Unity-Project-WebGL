using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleGame : MonoBehaviour
{
    public static PuzzleGame instance;

    public GameObject SelectedPiece;
    public int inRightPositionPieces = 0;
    public int totalPieces = 24;
    public int orderInLine = 1;
    public Timer timer;
    public GameObject inGamePuzzlePanel;
    public GameObject endGamePuzzlePanel;
    public ParticleSystem confetti;
    private PuzzlePiece puzzlePieceGO;

    public GameObject ButtonEndGame;

    [Header("PlayerPrefs")]
    [SerializeField] private string playerPrefsKey = "HighScoreEkleyeEkleyeBR";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timer.timer = 0;
        float getTimerBeforeQuit = PlayerPrefs.GetFloat(playerPrefsKey, 0);
        if (getTimerBeforeQuit > 0)
        {
            timer.timer += getTimerBeforeQuit;
        }
        puzzlePieceGO = PuzzlePiece.instance;
    }

    public void PieceInRightPosition()
    {
        inRightPositionPieces++;
        if (inRightPositionPieces == totalPieces)
        {
            ButtonEndGame.SetActive(false);
            StartConfetti();
            timer.stopTimer();
            PlayerPrefs.SetFloat(playerPrefsKey, timer.timer);
            Invoke("EndGame", 3f);
        }
    }

    private void OnApplicationQuit()
    {
        SaveData(); // Uygulama kapatýldýðýnda veriyi kaydet
    }

    private void OnDisable()
    {
        SaveData(); // Obje inaktif olduðunda veriyi kaydet
    }

    private void OnEnable()
    {
        LoadSavedData(); // Obje tekrar aktif olduðunda kaydedilen veriyi yükle
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(playerPrefsKey, timer.timer);
        PlayerPrefs.Save();
    }

    private void LoadSavedData()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            float savedTimerValue = PlayerPrefs.GetFloat(playerPrefsKey);
            timer.timer = savedTimerValue;
        }
    }

    private void EndGame()
    {
        confetti.Stop();
        confetti.gameObject.SetActive(false);
        inGamePuzzlePanel.SetActive(false);
        endGamePuzzlePanel.SetActive(true);
    }

    public void StartConfetti()
    {
        confetti.gameObject.SetActive(true);
        confetti.Play();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null && hit.transform.CompareTag("Puzzle"))
            {
                PuzzlePiece puzzlePiece = hit.transform.GetComponent<PuzzlePiece>();
                if (!puzzlePiece.inRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    puzzlePiece.selected = true;
                    SelectedPiece.GetComponent<SortingGroup>().sortingOrder = orderInLine;
                    orderInLine++;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.GetComponent<PuzzlePiece>().selected = false;
                SelectedPiece = null;
            }
        }

        if (SelectedPiece != null)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = new Vector3(mousePoint.x, mousePoint.y, 100);

            // Kameranýn kenarlarýna ulaþan objenin hareketini durdur.
            float halfWidth = SelectedPiece.GetComponent<SpriteRenderer>().bounds.extents.x;
            float halfHeight = SelectedPiece.GetComponent<SpriteRenderer>().bounds.extents.y;

            float leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + halfWidth;
            float rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - halfWidth;
            float bottomBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + halfHeight;
            float topBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - halfHeight;

            newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);
            newPosition.y = Mathf.Clamp(newPosition.y, bottomBoundary, topBoundary);

            SelectedPiece.transform.position = newPosition;
        }
    }
}

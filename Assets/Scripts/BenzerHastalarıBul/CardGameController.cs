using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardGameController : MonoBehaviour
{
    [Header("Game Settings")]
    public int columns = 4;
    public int rows = 4;
    public GameObject GamePanel;
    public GameObject EndGame;
    public Timer timer;
    public ParticleSystem confetti;

    public GameObject ButtonEndGame;

    [Header("High Score")]
    [SerializeField] private string highScoreKey = "HighScoreBenzerHastalariBul";

    [Header("Card Images")]
    [SerializeField] private MainImage gameObjectImage;
    [SerializeField] private Sprite[] images;

    private MainImage mainImage;

    private int[] RandomizeArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }

    private void Start()
    {
        timer.timer = 0;
        float savedTimer = PlayerPrefs.GetFloat(highScoreKey, 0);
        if (savedTimer > 0)
        {
            timer.timer += savedTimer;
        }

        mainImage = MainImage.Instance;

        int[] locations = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        locations = RandomizeArray(locations);

        Vector2 objectSize = GetObjectSizeForAspectRatio();
        float offsetX = objectSize.x + 0.1f;
        float offsetY = objectSize.y + 0.1f;

        int totalObjects = rows * columns;
        int numRows = (totalObjects + columns - 1) / columns;

        float startPositionX = -(columns - 1) * offsetX * 0.5f;
        float startPositionY = (numRows - 1) * offsetY * 0.5f;

        Camera mainCamera = Camera.main;
        Vector3 centerScreen = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        float minX = bottomLeft.x + objectSize.x * 0.5f;
        float maxX = topRight.x - objectSize.x * 0.5f;
        float minY = bottomLeft.y + objectSize.y * 0.5f;
        float maxY = topRight.y - objectSize.y * 0.5f;

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                int index = j * columns + i;

                if (index >= totalObjects)
                {
                    break;
                }

                MainImage gameImage = i == 0 && j == 0 ? gameObjectImage : Instantiate(gameObjectImage, GamePanel.transform);
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id]);

                float positionX = centerScreen.x + startPositionX + (offsetX * i);
                float positionY = centerScreen.y + startPositionY - (offsetY * j);
                positionX = Mathf.Clamp(positionX, minX, maxX);
                positionY = Mathf.Clamp(positionY, minY, maxY);

                gameImage.transform.position = new Vector3(positionX, positionY, gameObjectImage.transform.position.z);
            }
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if (gameObject.activeInHierarchy)
        {
            ArrangeCards();
        }
    }

    private void ArrangeCards()
    {
        MainImage[] existingCards = GamePanel.GetComponentsInChildren<MainImage>();
        Vector2 objectSize = GetObjectSizeForAspectRatio();
        float offsetX = objectSize.x + 0.1f;
        float offsetY = objectSize.y + 0.1f;

        int totalObjects = rows * columns;
        int numRows = (totalObjects + columns - 1) / columns;
        float startPositionX = -(columns - 1) * offsetX * 0.5f;
        float startPositionY = (numRows - 1) * offsetY * 0.5f;
        Vector3 centerScreen = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        float minX = bottomLeft.x + objectSize.x * 0.5f;
        float maxX = topRight.x - objectSize.x * 0.5f;
        float minY = bottomLeft.y + objectSize.y * 0.5f;
        float maxY = topRight.y - objectSize.y * 0.5f;

        for (int i = 0; i < existingCards.Length; i++)
        {
            int columnIndex = i % columns;
            int rowIndex = i / columns;

            float positionX = centerScreen.x + startPositionX + (offsetX * columnIndex);
            float positionY = centerScreen.y + startPositionY - (offsetY * rowIndex);
            positionX = Mathf.Clamp(positionX, minX, maxX);
            positionY = Mathf.Clamp(positionY, minY, maxY);

            existingCards[i].transform.position = new Vector3(positionX, positionY, existingCards[i].transform.position.z);
        }
    }

    private Vector2 GetObjectSizeForAspectRatio()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float objectWidth;
        float objectHeight;

        if (Screen.height > Screen.width)
        {
            objectWidth = Screen.width / 4 + 40;
            objectHeight = Screen.height / 5 + 50f;
            Debug.Log("Screen.height > Screen.width - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else if (Screen.width <= 950)
        {
            objectWidth = Screen.width / 5 + 20f;
            objectHeight = Screen.height / 4 + 115f;
            Debug.Log("Screen.width <= 950 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else if (Screen.width <= 975)
        {
            objectWidth = Screen.width / 5 + 20f;
            objectHeight = Screen.height / 4 + 125f;
            Debug.Log("Screen.width <= 975 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else if (Screen.width < 1080)
        {
            objectWidth = Screen.width / 5 + 20f;
            objectHeight = Screen.height / 4 + 125f;
            Debug.Log("Screen.width < 1080 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else
        {
            objectWidth = Screen.width / 5 + 20f;
            objectHeight = Screen.height / 4 + 112.5f;
            Debug.Log("Screen.width >= 1080 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }

        // Custom resolutions
        if (Screen.width == 1920 && Screen.height == 1080)
        {
            objectWidth = Screen.width / 8 + 50f;
            objectHeight = Screen.height / 5 + 50f;
            Debug.Log("1920x1080 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else if (Screen.width == 1366 && Screen.height == 768)
        {
            objectWidth = Screen.width / 6 + 50f;
            objectHeight = Screen.height / 4 + 50f;
            Debug.Log("1366x768 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else if (Screen.width == 2560 && Screen.height == 1440)
        {
            objectWidth = Screen.width / 9 + 50f;
            objectHeight = Screen.height / 7 + 50f;
            Debug.Log("2560x1440 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else if (Screen.width == 3840 && Screen.height == 2160)
        {
            objectWidth = Screen.width / 14 + 50f;
            objectHeight = Screen.height / 10 + 50f;
            Debug.Log("3840x2160 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
        }
        else
        {
            // Aspect ratio-based adjustments
            if (aspectRatio < 8f / 16f) // 8:16'dan küçük
            {
                objectWidth = Screen.width / 4 + 32f;
                objectHeight = Screen.height / 5 + 40f;
                objectWidth = Mathf.Clamp(objectWidth, 100, 119);
                objectHeight = Mathf.Clamp(objectHeight, 50, 149);
                Debug.Log("Aspect ratio: 8:16'dan küçük - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
            }
            else if (aspectRatio < 10f / 16f) // 10:16'dan küçük
            {
                objectWidth = Screen.width / 4 + 40f;
                objectHeight = Screen.height / 5 + 40f;
                objectWidth = Mathf.Clamp(objectWidth, 119, 129);
                objectHeight = Mathf.Clamp(objectHeight, 50, 139);
                Debug.Log("Aspect ratio: 10:16'dan küçük - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
            }
            else if (aspectRatio > 16f / 9f) // 16:9'dan büyük
            {
                objectWidth = Screen.width / 5 + 20f;
                objectHeight = Screen.height / 4 + 120f;
                objectWidth = Mathf.Clamp(objectWidth, 210, 300);
                objectHeight = Mathf.Clamp(objectHeight, 50, 250);
                Debug.Log("Aspect ratio: 16:9'dan büyük - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
            }
            else if (aspectRatio > 16f / 10f) // 16:10'dan büyük
            {
                objectWidth = Screen.width / 5 + 20f;
                objectHeight = Screen.height / 4 + 115f;
                objectWidth = Mathf.Clamp(objectWidth, 200, 300);
                objectHeight = Mathf.Clamp(objectHeight, 50, 230);
                Debug.Log("Aspect ratio: 16:10'dan büyük - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
            }
            else
            {
                // Other aspect ratios
                if (Screen.height > Screen.width)
                {
                    objectWidth = Screen.width / 5 + 20f;
                    objectHeight = Screen.height / 4 + 110f;
                    objectWidth = Mathf.Clamp(objectWidth, 119, 140);
                    objectHeight = Mathf.Clamp(objectHeight, 129, 160);
                    Debug.Log("Aspect ratio: Diðer 1 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
                }
                else
                {
                    objectWidth = Screen.width / 5 + 20f;
                    objectHeight = Screen.height / 4 + 110f;
                    objectWidth = Mathf.Clamp(objectWidth, 210, 250);
                    objectHeight = Mathf.Clamp(objectHeight, 230, 265);
                    Debug.Log("Aspect ratio: Diðer 2 - Object Width: " + objectWidth + " - Object Height: " + objectHeight);
                }
            }
        }

        return new Vector2(objectWidth, objectHeight);
    }



    private MainImage firstOpen;
    private MainImage secondOpen;
    private int score = 0;

    private void UpdateScoreText()
    {
        // Your code for updating the score text, if needed
    }

    public bool CanOpenSecondCard => secondOpen == null;

    public void ImageOpened(MainImage gameImage)
    {
        if (firstOpen == null)
        {
            firstOpen = gameImage;
            firstOpen.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (CanOpenSecondCard)
        {
            secondOpen = gameImage;
            StartCoroutine(CheckForMatch());
        }
    }

    private IEnumerator CheckForMatch()
    {
        if (firstOpen.ImageID == secondOpen.ImageID)
        {
            score++;
            UpdateScoreText();

            firstOpen.SetMatched(true);
            secondOpen.SetMatched(true);
            firstOpen.StartGlow();
            secondOpen.StartGlow();
            firstOpen.GetComponent<BoxCollider2D>().enabled = false;
            secondOpen.GetComponent<BoxCollider2D>().enabled = false;

            if (score == 8)
            {
                ButtonEndGame.SetActive(false);
                PlayerPrefs.SetFloat(highScoreKey, timer.timer);
                EndGameSequence();
            }
        }
        else
        {
            firstOpen.GetComponent<BoxCollider2D>().enabled = false;
            secondOpen.GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(1);
            firstOpen.GetComponent<BoxCollider2D>().enabled = true;
            secondOpen.GetComponent<BoxCollider2D>().enabled = true;
            firstOpen.CloseCard();
            secondOpen.CloseCard();
        }

        firstOpen = null;
        secondOpen = null;
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
        PlayerPrefs.SetFloat(highScoreKey, timer.timer);
        PlayerPrefs.Save();
    }

    private void LoadSavedData()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            float savedTimerValue = PlayerPrefs.GetFloat(highScoreKey);
            timer.timer = savedTimerValue;
        }
    }

    private void EndGameSequence()
    {
        StopCoroutine(CheckForMatch());
        StartCoroutine(ShowConfettiAndEndGame());
    }
    private IEnumerator ShowConfettiAndEndGame()
    {
        confetti.gameObject.SetActive(true);
        confetti.Play();

        // Stop the timer and wait for 3 seconds before ending the game
        timer.stopTimer();
        yield return new WaitForSeconds(3f);
        StopConfettiAndEndGame();
    }

    private void StopConfettiAndEndGame()
    {
        confetti.Stop();
        confetti.gameObject.SetActive(false);

        GamePanel.SetActive(false);
        EndGame.SetActive(true);
    }
}

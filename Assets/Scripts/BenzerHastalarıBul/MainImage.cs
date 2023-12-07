using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainImage : MonoBehaviour
{
    [SerializeField] private Image imageCard;
    [SerializeField] private CardGameController gameController;
    [SerializeField] private Sprite faceSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private ParticleSystem glow;

    private bool coroutineAllowed;
    private bool isFlipped;
    private bool isMatched;

    public bool IsMatched => isMatched;

    public void SetMatched(bool matched)
    {
        isMatched = matched;
    }


    public static MainImage Instance { get; private set; }

    private void Awake()
    {
        coroutineAllowed = true;
        isFlipped = false;
        Instance = this;
    }

    private void OnMouseDown()
    {
        if (gameController.CanOpenSecondCard && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }

    public void CloseCard()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        isFlipped = false;
        StartCoroutine(RotateCardBack());
    }

    public IEnumerator RotateCard()
    {
        coroutineAllowed = false;

        for (float i = 0f; i <= 180; i += 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                imageCard.sprite = faceSprite;
                if (!isFlipped)
                {
                    gameController.ImageOpened(this);
                    isFlipped = true;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;
    }

    private IEnumerator RotateCardBack()
    {
        for (float i = 180f; i >= 0; i -= 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                imageCard.sprite = backSprite;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private int _imageID;
    public int ImageID => _imageID;

    public void ChangeSprite(int id, Sprite spriteImage)
    {
        _imageID = id;
        faceSprite = spriteImage;
    }

    public void StartGlow()
    {
        glow.gameObject.SetActive(true);
        glow.Play();
    }

    public void StopGlow()
    {
        glow.Stop();
        glow.gameObject.SetActive(false);
    }
}

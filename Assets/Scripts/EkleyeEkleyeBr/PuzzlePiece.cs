using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzlePiece : MonoBehaviour
{
    public Vector3 rightPosition;
    public bool inRightPosition;
    public bool selected;

    public float diffWithRightPosition = 50f;

    public GameObject startLocation;
    public PuzzleGame puzzleGameGO;
    public ParticleSystem sparkle;
    public static PuzzlePiece instance;

    private void Awake()
    {
        sparkle = GetComponentInChildren<ParticleSystem>();
        sparkle.gameObject.SetActive(false);
        instance = this;
    }

    private void Start()
    {
        puzzleGameGO = PuzzleGame.instance;
        rightPosition = transform.position;

        // Find the LocationStart GameObject within the children of this GameObject.
        Transform locationStartTransform = transform.Find("LocationStart");
        if (locationStartTransform != null)
        {
            startLocation = locationStartTransform.gameObject;
        }
        else
        {
            Debug.LogError("LocationStart GameObject not found as a child of this GameObject!");
        }

        SetStartingLocation();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, rightPosition) < diffWithRightPosition)
        {
            if (!selected && !inRightPosition)
            {
                PlaceInRightPosition();
            }
        }

        ClampPosition();
    }

    private void SetStartingLocation()
    {
        transform.position = startLocation.transform.position;
    }

    private void PlaceInRightPosition()
    {
        transform.position = rightPosition;
        inRightPosition = true;
        StartGlow();
        Invoke("StopGlow", 1f);
        GetComponent<SortingGroup>().sortingOrder = 0;
        puzzleGameGO.PieceInRightPosition();
        enabled = false;
    }

    private void ClampPosition()
    {
        Vector3 newPosition = transform.position;

        // Kameranýn kenarlarýna ulaþan objenin hareketini durdur.
        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        float halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        float leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + halfWidth;
        float rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - halfWidth;
        float bottomBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + halfHeight;
        float topBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - halfHeight;

        newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);
        newPosition.y = Mathf.Clamp(newPosition.y, bottomBoundary, topBoundary);

        transform.position = newPosition;
    }

    public void StartGlow()
    {
        sparkle.gameObject.SetActive(true);
        sparkle.Play();
    }

    public void StopGlow()
    {
        sparkle.Stop();
        sparkle.gameObject.SetActive(false);
    }
}

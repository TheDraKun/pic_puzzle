
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PuzzleController : MonoBehaviour
{
    public UIManager uiManager;
    public Texture2D puzzleImage;
    
    [Range(3, 6)]
    public int rows = 3;
    [Range(3, 6)]
    public int columns = 3;

    public GameObject piecePrefab;
    public Transform pieceContainer;
    public float shuffleTime = 0.5f;
    public TextMeshProUGUI winText;

    private List<GameObject> pieces = new List<GameObject>();

    public void RegenerateGrid()
    {
        if (puzzleImage == null) return;

        var gridLayout = pieceContainer.GetComponent<GridLayoutGroup>();
        var containerRect = pieceContainer.GetComponent<RectTransform>();

        if (gridLayout != null && containerRect != null)
        {
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = columns;

            float cellWidth = (containerRect.rect.width - (gridLayout.spacing.x * (columns - 1)) - gridLayout.padding.left - gridLayout.padding.right) / columns;
            float cellHeight = (containerRect.rect.height - (gridLayout.spacing.y * (rows - 1)) - gridLayout.padding.top - gridLayout.padding.bottom) / rows;

            gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        }

        CreatePuzzlePieces();
    }

    void CreatePuzzlePieces()
    {
        pieces.Clear();
        foreach (Transform child in pieceContainer) {
            Destroy(child.gameObject);
        }

        int pieceWidth = puzzleImage.width / columns;
        int pieceHeight = puzzleImage.height / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject piece = Instantiate(piecePrefab, pieceContainer);
                piece.name = $"Piece_{x}_{y}";

                Rect rect = new Rect(x * pieceWidth, (rows - 1 - y) * pieceHeight, pieceWidth, pieceHeight);
                Sprite sprite = Sprite.Create(puzzleImage, rect, new Vector2(0.5f, 0.5f));
                piece.GetComponent<Image>().sprite = sprite;

                pieces.Add(piece);

                piece.GetComponent<PieceController>().Setup(this);
            }
        }

        if (winText != null) winText.gameObject.SetActive(false);
    }

    public void Shuffle()
    {
        StartCoroutine(ShufflePieces());
    }

    IEnumerator ShufflePieces()
    {
        if (winText != null) winText.gameObject.SetActive(false);

        yield return new WaitForSeconds(shuffleTime);

        int pieceCount = pieceContainer.childCount;
        for (int i = 0; i < pieceCount; i++)
        {
            int randomIndex = Random.Range(i, pieceCount);
            pieceContainer.GetChild(i).SetSiblingIndex(randomIndex);
        }

        CheckForWin();
    }

    public void SwapPieces(GameObject piece1, GameObject piece2)
    {
        if (piece1 == piece2) return;

        int index1 = piece1.transform.GetSiblingIndex();
        int index2 = piece2.transform.GetSiblingIndex();

        piece1.transform.SetSiblingIndex(index2);
        piece2.transform.SetSiblingIndex(index1);

        LayoutRebuilder.ForceRebuildLayoutImmediate(pieceContainer.GetComponent<RectTransform>());

        CheckForWin();
    }

    public void CheckForWin()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieceContainer.GetChild(i).gameObject != pieces[i])
            {
                if (winText != null) winText.gameObject.SetActive(false);
                return;
            }
        }

        StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence()
    {
        if (winText != null) winText.gameObject.SetActive(true);
        Debug.Log("You Win!");

        yield return new WaitForSeconds(3f);

        if (uiManager != null) 
        {
            uiManager.ShowSetupPanel();
        }
    }
}


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PieceController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private PuzzleController puzzleController;
    private CanvasGroup canvasGroup;
    private GameObject ghostPiece;

    public void Setup(PuzzleController controller)
    {
        puzzleController = controller;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ghostPiece = new GameObject("GhostPiece");
        ghostPiece.transform.SetParent(transform.root);
        ghostPiece.transform.SetAsLastSibling();

        var image = ghostPiece.AddComponent<Image>();
        image.sprite = GetComponent<Image>().sprite;
        image.rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        image.color = new Color(1, 1, 1, 0.6f);
        image.raycastTarget = false; // This is the fix!

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostPiece != null)
        {
            ghostPiece.transform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject originalPiece = eventData.pointerDrag;
        if (originalPiece != null && originalPiece != gameObject)
        {
            puzzleController.SwapPieces(originalPiece, gameObject);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghostPiece != null)
        {
            Destroy(ghostPiece);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}

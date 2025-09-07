using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject setupPanel;
    public GameObject gamePanel;
    public Slider rowsSlider;
    public Slider colsSlider;
    public Button playButton;
    public TextMeshProUGUI rowsText;
    public TextMeshProUGUI colsText;

    public PuzzleController puzzleController;

    private void Start()
    {
        setupPanel.SetActive(true);
        gamePanel.SetActive(false);

        if (puzzleController != null)
        {
            rowsSlider.value = puzzleController.rows;
            colsSlider.value = puzzleController.columns;
        }

        playButton.onClick.AddListener(OnPlayButtonClick);
        rowsSlider.onValueChanged.AddListener(OnRowsSliderChanged);
        colsSlider.onValueChanged.AddListener(OnColsSliderChanged);

        OnRowsSliderChanged(rowsSlider.value);
        OnColsSliderChanged(colsSlider.value);
    }

    private void OnPlayButtonClick()
    {
        puzzleController.rows = (int)rowsSlider.value;
        puzzleController.columns = (int)colsSlider.value;

        puzzleController.RegenerateGrid();
        puzzleController.Shuffle();

        setupPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void ShowSetupPanel()
    {
        setupPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    private void OnRowsSliderChanged(float value)
    {
        if (rowsText != null) rowsText.text = "Rows: " + (int)value;
    }

    private void OnColsSliderChanged(float value)
    {
        if (colsText != null) colsText.text = "Cols: " + (int)value;
    }
}

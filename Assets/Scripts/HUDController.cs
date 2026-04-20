using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private TMP_Text rescuedText;
    [SerializeField] private TMP_Text diedText;
    [SerializeField] private TMP_Text timerText;

    [Header("End Screens")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    void Update()
    {
        rescuedText.text = "Rescued: " + gameManager.soldiersRescued;
        diedText.text = "Died: " + gameManager.soldiersDied;
        timerText.text = "Time: " + Mathf.CeilToInt(gameManager.timeRemaining) + "s";

        if (winPanel != null)
            winPanel.SetActive(gameManager.isWon);

        if (losePanel != null)
            losePanel.SetActive(gameManager.isGameOver);
    }
}

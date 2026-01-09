using TMPro;
using UnityEngine;

public class BingoUIManager : MonoBehaviour
{
    [Header("Turn Indicator UI")]
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private GameObject player1LockPanel;
    [SerializeField] private GameObject player2LockPanel;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI p1BingoText;
    [SerializeField] private TextMeshProUGUI p2BingoText;

    [Header("Win Screen")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winnerNameText;

    // Lookup array for score text. avoiding long if-else chains.
    // Index 0 = 0 lines, Index 5 = 5 lines (Bingo)

    private readonly string[] bingoSteps = { "", "B", "B I", "B I N", "B I N G", "B I N G O" };

    /// <summary>
    /// Updates the visual state of the board based on whose turn it is.
    /// Toggles the 'Lock Panels' to physically block input for the inactive player.
    /// </summary>
    /// <param name="isPlayer1Turn">True if it is Player 1's turn.</param>

    public void UpdateTurnDisplay(bool isPlayer1Turn)
    {
        // Update the text prompt
        if (turnText) turnText.text = isPlayer1Turn ? "Player 1's Turn" : "Player 2's Turn";

        // Toggle Lock Panels:
        // If it IS Player 1's turn, we unlock P1 (SetActive false) and lock P2 (SetActive true).
        if (player1LockPanel) player1LockPanel.SetActive(!isPlayer1Turn);
        if (player2LockPanel) player2LockPanel.SetActive(isPlayer1Turn);
    }

    /// <summary>
    /// Updates the "B I N G O" text progress based on the number of completed lines.
    /// </summary>
    /// <param name="p1Lines">Lines completed by Player 1.</param>
    /// <param name="p2Lines">Lines completed by Player 2.</param>

    public void UpdateBingoScore(int p1Lines, int p2Lines)
    {
        // Defensive Coding: Clamp the value between 0 and 5 
        // to prevent an 'IndexOutOfRangeException' if logic errors occur elsewhere.

        int safeP1 = Mathf.Clamp(p1Lines, 0, 5);
        int safeP2 = Mathf.Clamp(p2Lines, 0, 5);

        // Directly map the line count to the text string using the array

        if (p1BingoText) p1BingoText.text = bingoSteps[safeP1];
        if (p2BingoText) p2BingoText.text = bingoSteps[safeP2];
    }

    /// <summary>
    /// Activates the Game Over screen and sets the winner's name.
    /// </summary>

    public void ShowWinScreen(string winnerName)
    {
        if (winPanel) winPanel.SetActive(true);
        if (winnerNameText) winnerNameText.text = winnerName + " WINS!";
    }

    /// <summary>
    /// Resets all UI elements to their default 'Start of Game' state.
    /// </summary>

    public void ResetUI()
    {
        if (winPanel) winPanel.SetActive(false);
        if (p1BingoText) p1BingoText.text = "";
        if (p2BingoText) p2BingoText.text = "";
    }
}
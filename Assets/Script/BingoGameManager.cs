using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BingoGameManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform player1Panel;
    [SerializeField] private Transform player2Panel;

    [Header("Dependencies")]
    [SerializeField] private BingoUIManager uiManager;

    // Separate lists to track each player's grid state independently
    private List<GridCell> p1Cells = new List<GridCell>();
    private List<GridCell> p2Cells = new List<GridCell>();

    private bool isPlayer1Turn = true;
    private bool isGameOver = false;

    void Start() => StartGame();

    /// <summary>
    /// Resets the game state, clears the UI, and regenerates the grids.
    /// </summary>
    public void StartGame()
    {
        isPlayer1Turn = true;
        isGameOver = false;

        if (uiManager) uiManager.ResetUI();

        // Generate grids for both players and store references in their respective lists

        CreateGrid(player1Panel, 1, p1Cells);
        CreateGrid(player2Panel, 2, p2Cells);

        if (uiManager) uiManager.UpdateTurnDisplay(isPlayer1Turn);
    }

    /// <summary>
    /// Instantiates the grid buttons and assigns random numbers 1-25.
    /// </summary>

    private void CreateGrid(Transform parentPanel, int playerID, List<GridCell> targetList)
    {
        // cleanup existing buttons to prevent duplicates
        foreach (Transform child in parentPanel) Destroy(child.gameObject);
        targetList.Clear();

        // Generate a list of numbers 1-25 and shuffle them
        List<int> randomNumbers = Enumerable.Range(1, 25)
                                            .OrderBy(x => UnityEngine.Random.value)
                                            .ToList();

        foreach (int num in randomNumbers)
        {
            GameObject obj = Instantiate(cellPrefab, parentPanel);
            GridCell cell = obj.GetComponent<GridCell>();
            cell.Setup(num, playerID, this);
            targetList.Add(cell);
        }
    }

    /// <summary>
    /// Called when any button is clicked. Handles marking, win checking, and turn switching.
    /// </summary>

    public void NumberClicked(int selectedNumber)
    {
        if (isGameOver) return;

        // Mark the selected number on BOTH players' boards

        MarkNumberInList(p1Cells, selectedNumber);
        MarkNumberInList(p2Cells, selectedNumber);

        CheckGameStatus();

        // Switch turn only if the game hasn't ended

        if (!isGameOver)
        {
            isPlayer1Turn = !isPlayer1Turn;
            if (uiManager) uiManager.UpdateTurnDisplay(isPlayer1Turn);
        }
    }

    private void MarkNumberInList(List<GridCell> cells, int number)
    {
        // Find the cell with the matching number and mark it

        foreach (GridCell cell in cells)
        {
            if (cell.GetMyNumber() == number)
            {
                cell.MarkAsSelected();
                return; // Optimization: Stop searching once found
            }
        }
    }

    /// <summary>
    /// Calculates the score (lines completed) for both players and checks for a winner.
    /// </summary>

    private void CheckGameStatus()
    {
        int p1Lines = CountLines(p1Cells);
        int p2Lines = CountLines(p2Cells);

        if (uiManager) uiManager.UpdateBingoScore(p1Lines, p2Lines);

        // Win Condition: 5 Lines completed

        if (p1Lines >= 5) GameOver("Player 1");
        else if (p2Lines >= 5) GameOver("Player 2");
    }

    private void GameOver(string winner)
    {
        isGameOver = true;
        if (uiManager) uiManager.ShowWinScreen(winner);
    }

    /// <summary>
    /// Iterates through rows and columns to count how many complete lines exist.
    /// </summary>

    private int CountLines(List<GridCell> cells)
    {
        int linesFound = 0;

        // Check 5 Rows (Horizontal)
        // Rows start at indices 0, 5, 10... and step by 1

        for (int i = 0; i < 5; i++)
        {
            if (CheckLine(cells, i * 5, 1)) linesFound++;
        }

        // Check 5 Columns (Vertical)
        // Columns start at indices 0, 1, 2... and step by 5

        for (int i = 0; i < 5; i++)
        {
            if (CheckLine(cells, i, 5)) linesFound++;
        }

        return linesFound;
    }

    /// <summary>
    /// Helper function to check if a sequence of 5 cells are all marked.
    /// </summary>
    /// <param name="start">The starting index in the list.</param>
    /// <param name="step">The increment step (1 for row, 5 for column).</param>

    private bool CheckLine(List<GridCell> cells, int start, int step)
    {
        for (int i = 0; i < 5; i++)
        {
            int index = start + (i * step);
            if (!cells[index].IsSelected()) return false;
        }
        return true;
    }
}
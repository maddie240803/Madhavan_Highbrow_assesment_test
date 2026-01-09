using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GridCell : MonoBehaviour
{
    [Header("UI References")]
    // Public references to be assigned in the Prefab Inspector
    public TextMeshProUGUI label;
    public Button myButton;
    public Image bgImage;

    // Internal state variables
    private int myNumber;
    private int ownerID; // 1 for Player 1, 2 for Player 2
    private BingoGameManager manager; // Reference to the main controller
    private bool isMarked = false;

    /// <summary>
    /// Initializes the cell with data and resets its visual state.
    /// Acts like a Constructor for this MonoBehaviour.
    /// </summary>
    /// <param name="num">The Bingo number (1-25) assigned to this cell.</param>
    /// <param name="owner">Which player owns this cell.</param>
    /// <param name="theManager">Reference to the Game Manager to report clicks.</param>

    public void Setup(int num, int owner, BingoGameManager theManager)
    {
        myNumber = num;
        ownerID = owner;
        manager = theManager;

        // Update the UI Label
        label.text = num.ToString();

        // Reset state for a new game
        isMarked = false;
        myButton.interactable = true;
        bgImage.color = Color.white;

        // Clean up previous events to prevent "double click" errors on restart
        myButton.onClick.RemoveAllListeners();

        // Subscribe the OnClick method to the button's event
        myButton.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Triggered when the button is pressed.
    /// Delegates the actual logic to the GameManager (Centralized Logic).
    /// </summary>

    void OnClick()
    {
        // We pass 'myNumber' so the Manager knows WHICH number was picked
        manager.NumberClicked(myNumber);
    }

    /// <summary>
    /// Updates the visual state to show this cell has been selected.
    /// </summary>

    public void MarkAsSelected()
    {
        // Defensive check: If already marked, do nothing
        if (isMarked) return;

        isMarked = true;

        // Visual feedback for the player
        bgImage.color = Color.gray;

        
    }

    // Getters to allow the Manager to read private data safely (Encapsulation)

    public int GetMyNumber() => myNumber;
    public int GetOwnerID() => ownerID;
    public bool IsSelected() => isMarked;
}
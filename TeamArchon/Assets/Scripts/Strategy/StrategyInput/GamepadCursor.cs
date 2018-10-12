using UnityEngine;

public class GamepadCursor : MonoBehaviour {
    #region Members
    public int player;
    public Color pressedColor;
    public SpriteRenderer cursorImage;
    #endregion

    #region Properties
    public GameObject ClickedObject { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes variables and gets components.
    /// </summary>
    private void Start() {
        cursorImage = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Handles the position and input from a gamepad during
    /// the strategy portion of the game.
    /// </summary>
    private void Update() {
        UpdateMovement();

        if (Click()) {
            OnClick();
        }
        else {
            Reset();
        }
    }

    /// <summary>
    /// Updates the position of the cursor on the screen.
    /// </summary>
    private void UpdateMovement() {
        float xInput = 0, yInput = 0;

        // Get input based on player axis
        switch (player) {
            case 0:
                xInput = hInput.GetAxis("Player1XAxis");
                yInput = hInput.GetAxis("Player1YAxis");
                break;
            case 1:
                xInput = hInput.GetAxis("Player2XAxis");
                yInput = hInput.GetAxis("Player2YAxis");
                break;
            case 2:
                xInput = hInput.GetAxis("Player3XAxis");
                yInput = hInput.GetAxis("Player3YAxis");
                break;
            case 3:
                xInput = hInput.GetAxis("Player4XAxis");
                yInput = hInput.GetAxis("Player4YAxis");
                break;
            case 4:
                xInput = hInput.GetAxis("Player5XAxis");
                yInput = hInput.GetAxis("Player5YAxis");
                break;
            case 5:
                xInput = hInput.GetAxis("Player6XAxis");
                yInput = hInput.GetAxis("Player6YAxis");
                break;
            case 6:
                xInput = hInput.GetAxis("Player7XAxis");
                yInput = hInput.GetAxis("Player7YAxis");
                break;
            case 7:
                xInput = hInput.GetAxis("Player8XAxis");
                yInput = hInput.GetAxis("Player8YAxis");
                break;
        }

        // Set the new position of the cursor
        transform.position = new Vector3(
            transform.position.x + xInput,
            transform.position.y,
            transform.position.z - yInput);
    }

    /// <summary>
    /// Determines whether or not a cursor has clicked.
    /// </summary>
    /// <returns>True if click input, else false</returns>
    private bool Click() {
        switch (player) {
            case 0: return hInput.GetButton("Player1A");
            case 1: return hInput.GetButton("Player2A");
            case 2: return hInput.GetButton("Player3A");
            case 3: return hInput.GetButton("Player4A");
            case 4: return hInput.GetButton("Player5A");
            case 5: return hInput.GetButton("Player6A");
            case 6: return hInput.GetButton("Player7A");
            case 7: return hInput.GetButton("Player8A");
            default: return false;
        }
    }

    /// <summary>
    /// Activates the clicked behavior on any strategy object that has a valid click behavior.
    /// </summary>
    private void OnClick() {
        // Raycast down and hit object that has been clicked on
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1000);

        // Handle hit object if it has a strategy behavior (which handles "clicks")
        GamepadBehavior behavior;
        if (hit.collider != null && hit.collider.gameObject != null &&
           (behavior = hit.collider.gameObject.GetComponent<GamepadBehavior>()) != null) {
            ClickedObject = hit.collider.gameObject;
            behavior.OnClick(this);
        }

        // Change the cursor's color to show that the player clicked
        cursorImage.color = pressedColor;
    }

    /// <summary>
    /// Resets the cursor to the default state.
    /// </summary>
    private void Reset() {
        // Reset the color of the cusor
        cursorImage.color = Color.white;
    }
    #endregion
}

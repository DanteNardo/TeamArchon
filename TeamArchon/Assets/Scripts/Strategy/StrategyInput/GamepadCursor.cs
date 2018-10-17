using UnityEngine;

public class GamepadCursor : MonoBehaviour {
    #region Members
    public float sensitivity;
    public Player player;
    #endregion

    #region Properties
    public GameObject ClickedObject { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes variables and gets components.
    /// </summary>
    private void Start() {

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
        switch (player.PlayerNum) {
            case 0:
                xInput = hInput.GetAxis("Joy1RightXAxis");
                yInput = hInput.GetAxis("Joy1RightYAxis");
                break;
            case 1:
                xInput = hInput.GetAxis("Joy2RightXAxis");
                yInput = hInput.GetAxis("Joy2RightYAxis");
                break;
            case 2:
                xInput = hInput.GetAxis("Joy3RightXAxis");
                yInput = hInput.GetAxis("Joy3RightYAxis");
                break;
            case 3:
                xInput = hInput.GetAxis("Joy4RightXAxis");
                yInput = hInput.GetAxis("Joy4RightYAxis");
                break;
            case 4:
                xInput = hInput.GetAxis("Joy5RightXAxis");
                yInput = hInput.GetAxis("Joy5RightYAxis");
                break;
            case 5:
                xInput = hInput.GetAxis("Joy6RightXAxis");
                yInput = hInput.GetAxis("Joy6RightYAxis");
                break;
            case 6:
                xInput = hInput.GetAxis("Joy7RightXAxis");
                yInput = hInput.GetAxis("Joy7RightYAxis");
                break;
            case 7:
                xInput = hInput.GetAxis("Joy8RightXAxis");
                yInput = hInput.GetAxis("Joy8RightYAxis");
                break;
        }

        // Set the new position of the cursor
        transform.position = new Vector3(
            transform.position.x + xInput * sensitivity,
            transform.position.y,
            transform.position.z - yInput * sensitivity);
    }

    /// <summary>
    /// Determines whether or not a cursor has clicked.
    /// </summary>
    /// <returns>True if click input, else false</returns>
    private bool Click() {
        switch (player) {
            case 0: return hInput.GetButtonDown("Joy1A");
            case 1: return hInput.GetButtonDown("Joy2A");
            case 2: return hInput.GetButtonDown("Joy3A");
            case 3: return hInput.GetButtonDown("Joy4A");
            case 4: return hInput.GetButtonDown("Joy5A");
            case 5: return hInput.GetButtonDown("Joy6A");
            case 6: return hInput.GetButtonDown("Joy7A");
            case 7: return hInput.GetButtonDown("Joy8A");
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
    }

    /// <summary>
    /// Resets the cursor to the default state.
    /// </summary>
    private void Reset() {

    }
    #endregion
}

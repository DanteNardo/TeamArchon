using UnityEngine;

/// <summary>
/// Acts as a virtual mouse cursor, but is controlled with a gamepad.
/// </summary>
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
        // Only update the cursor if it is this player's turn
        if (MasterGame.Instance.CurrentPlayer == player) {
            UpdateMovement();

            if (ClickA()) {
                OnClickA();
            }
            else if (ClickY()) {
                OnClickY();
            }
            else {
                Reset();
            }
        }
    }

    /// <summary>
    /// Updates the position of the cursor on the screen.
    /// </summary>
    private void UpdateMovement() {
        float xInput = 0, yInput = 0;
        
        // Get input based on player axis
        switch (player.JoystickValue) {
            case 0:
                xInput = hInput.GetAxis("Joy1LeftXAxis");
                yInput = hInput.GetAxis("Joy1LeftYAxis");
                break;
            case 1:
                //Debug.Log(player.JoystickValue);
                xInput = hInput.GetAxis("Joy2LeftXAxis");
                yInput = hInput.GetAxis("Joy2LeftYAxis");
                break;
            case 2:
                xInput = hInput.GetAxis("Joy3LeftXAxis");
                yInput = hInput.GetAxis("Joy3LeftYAxis");
                break;
            case 3:
                xInput = hInput.GetAxis("Joy4LeftXAxis");
                yInput = hInput.GetAxis("Joy4LeftYAxis");
                break;
            case 4:
                xInput = hInput.GetAxis("Joy5LeftXAxis");
                yInput = hInput.GetAxis("Joy5LeftYAxis");
                break;
            case 5:
                xInput = hInput.GetAxis("Joy6LeftXAxis");
                yInput = hInput.GetAxis("Joy6LeftYAxis");
                break;
            case 6:
                xInput = hInput.GetAxis("Joy7LeftXAxis");
                yInput = hInput.GetAxis("Joy7LeftYAxis");
                break;
            case 7:
                xInput = hInput.GetAxis("Joy8LeftXAxis");
                yInput = hInput.GetAxis("Joy8LeftYAxis");
                break;
        }

        // Set the new position of the cursor
        transform.position = new Vector3(
            transform.position.x + xInput * sensitivity,
            transform.position.y,
            transform.position.z + yInput * sensitivity);
    }

    /// <summary>
    /// Determines whether or not a cursor has clicked A.
    /// </summary>
    /// <returns>True if click input, else false</returns>
    private bool ClickA() {
        switch (player.JoystickValue) {
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
    /// Determines whether or not a cursor has clicked Y.
    /// </summary>
    /// <returns>True if click input, else false</returns>
    private bool ClickY() {
        switch (player.JoystickValue) {
            case 0: return hInput.GetButtonDown("Joy1Y");
            case 1: return hInput.GetButtonDown("Joy2Y");
            case 2: return hInput.GetButtonDown("Joy3Y");
            case 3: return hInput.GetButtonDown("Joy4Y");
            case 4: return hInput.GetButtonDown("Joy5Y");
            case 5: return hInput.GetButtonDown("Joy6Y");
            case 6: return hInput.GetButtonDown("Joy7Y");
            case 7: return hInput.GetButtonDown("Joy8Y");
            default: return false;
        }
    }

    /// <summary>
    /// Activates the clicked behavior on any strategy object that has a valid click behavior.
    /// </summary>
    private void OnClickA() {
        // Raycast down and hit object that has been clicked on
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1000);

        // Handle hit object if it has a gamepad behavior (which handles "clicks")
        GamepadBehavior behavior;
        if (hit.collider != null && hit.collider.gameObject != null &&
           (behavior = hit.collider.gameObject.GetComponent<GamepadBehavior>()) != null) {
            ClickedObject = hit.collider.gameObject;
            behavior.OnClick(this);
        }
    }

    /// <summary>
    /// Attempts to activate a piece's special ability.
    /// </summary>
    private void OnClickY() {
        if (!InputManager.Instance.SpecialAttempt) {
            InputManager.Instance.AttemptSpecial();
        }
        else {
            InputManager.Instance.SpecialAttemptMade();
        }
    }

    /// <summary>
    /// Resets the cursor to the default state.
    /// </summary>
    private void Reset() {

    }
    #endregion
}

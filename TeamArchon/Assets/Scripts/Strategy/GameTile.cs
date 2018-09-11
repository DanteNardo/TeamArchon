using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour {

    /// <summary>
    /// Generates an attempted move/place if the tile is clicked.
    /// </summary>
    private void OnMouseDown() {
        // Create an attempted move if there is a selected piece
        if (InputManager.Instance.Selected != null) {

            // Determine direction of move
            EDirection direction = DetermineDirection();

            // Generate potential move and save it in InputManager
            InputManager.Instance.AttemptMove(MoveGeneration.GenerateMove(InputManager.Instance.Selected, direction));
        }
    }

    /// <summary>
    /// Provides a direction based on tile position relative to InputManager.Instance.Selected.
    /// </summary>
    /// <returns>The direction to generate a potential move</returns>
    private EDirection DetermineDirection() {
        if (transform.position.x > InputManager.Instance.Selected.transform.position.x &&
            transform.position.z > InputManager.Instance.Selected.transform.position.z) {
            return EDirection.Northeast;
        }
        if (transform.position.x < InputManager.Instance.Selected.transform.position.x &&
            transform.position.z > InputManager.Instance.Selected.transform.position.z) {
            return EDirection.Northwest;
        }
        if (transform.position.x == InputManager.Instance.Selected.transform.position.x &&
            transform.position.z > InputManager.Instance.Selected.transform.position.z) {
            return EDirection.North;
        }
        if (transform.position.x > InputManager.Instance.Selected.transform.position.x &&
            transform.position.z < InputManager.Instance.Selected.transform.position.z) {
            return EDirection.Southeast;
        }
        if (transform.position.x < InputManager.Instance.Selected.transform.position.x &&
            transform.position.z < InputManager.Instance.Selected.transform.position.z) {
            return EDirection.Southwest;
        }
        if (transform.position.x == InputManager.Instance.Selected.transform.position.x &&
            transform.position.z < InputManager.Instance.Selected.transform.position.z) {
            return EDirection.South;
        }
        if (transform.position.x > InputManager.Instance.Selected.transform.position.x &&
            transform.position.z == InputManager.Instance.Selected.transform.position.z) {
            return EDirection.East;
        }
        if (transform.position.x < InputManager.Instance.Selected.transform.position.x &&
            transform.position.z == InputManager.Instance.Selected.transform.position.z) {
            return EDirection.West;
        }

        // Default return
        return EDirection.None;
    }
}

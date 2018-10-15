using UnityEngine;

/// <summary>
/// A wrapper around monobehavior that allows interaction
/// between a gamepad's cursor and this object.
/// </summary>
public abstract class GamepadBehavior : MonoBehaviour {
    #region Methods
    public abstract void OnClick(GamepadCursor cursor);
    #endregion
}

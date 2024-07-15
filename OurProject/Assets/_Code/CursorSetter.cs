using UnityEngine;

public class CursorSetter : MonoBehaviour {
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public void Awake() {
        Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode);
    }
}
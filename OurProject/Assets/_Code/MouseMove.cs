using UnityEngine;

public class MouseMove : MonoBehaviour {
    Vector3 handleToOriginVector;
    public bool isDragging;

    void OnMouseDown() {
        handleToOriginVector = transform.root.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    void OnMouseDrag() {
        transform.root.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + handleToOriginVector;
    }

    void OnMouseUp() {
        isDragging = false;
    }
}
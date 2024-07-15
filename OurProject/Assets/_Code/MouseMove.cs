using DG.Tweening;
using UnityEngine;

public class MouseMove : MonoBehaviour {
    Vector3 handleToOriginVector;
    [SerializeField] bool isDragging;
    [SerializeField] float smoothTime = 0.05f; // Smoothing factor
    [SerializeField] float afterReleaseDistanceMultiplier = 1.5f; // How far to continue based on the current speed
    [SerializeField] float afterReleaseTime = 0.5f;

    Vector3 velocity = Vector3.zero; // Needed for smooth damping
    Vector3 targetPosition;
    bool shouldUpdatePosition;
    Vector3 newValue;
    Vector3 lastFrameVelocity;

    Transform rootTransform;
    Vector3 originalScale; // To keep track of the original scale
    Quaternion originalRotation; // To keep track of the original rotation
    Tween scaleTween;
    Tween rotateTween;

    public void Start() {
        rootTransform = transform.root;
        targetPosition = rootTransform.position;
        newValue = targetPosition;
        lastFrameVelocity = Vector3.zero;

        originalScale = rootTransform.localScale;
        originalRotation = rootTransform.localRotation;
    }

    public void Update() {
        // speed += (target - current) * deltaTime * stiffness; speed *= pow(1 - damping, deltaTime); current += speed * deltaTime;
        if (!shouldUpdatePosition || !isDragging)
            return;

        rootTransform.position = newValue;
    }

    void OnMouseDown() {
        handleToOriginVector = rootTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;

        StartInteractiveEffects();
    }

    void OnMouseDrag() {
        if (!isDragging)
            return;

        // Vector3 cursorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + handleToOriginVector;

        var currentValue = rootTransform.position;
        newValue = Vector3.SmoothDamp(currentValue, targetPosition, ref velocity, smoothTime);

        // Track velocity to use for inertia
        lastFrameVelocity = (newValue - currentValue) / Time.deltaTime;

        if (currentValue == newValue) {
            shouldUpdatePosition = false;
            return;
        }

        shouldUpdatePosition = true;
    }

    void OnMouseUp() {
        isDragging = false;

        // Continue moving with inertia
        Vector3 extrapolatedTarget = rootTransform.position + lastFrameVelocity * afterReleaseDistanceMultiplier;
        rootTransform.DOMove(extrapolatedTarget, afterReleaseTime)
                      .SetEase(Ease.OutCubic) // Easing to simulate deceleration
                      .OnComplete(() => {
                          // Update targetPosition to the new position after the move completes
                          targetPosition = rootTransform.position;
                          newValue = targetPosition;
                      });
    }

    void StartInteractiveEffects() {
        // Calculate handleToOriginVector
        handleToOriginVector = rootTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;

        // Kill specific tweens if they are active
        scaleTween?.Kill(true); // Kill and reset the scale tween
        rotateTween?.Kill(true); // Kill and reset the rotate tween

        // Reset to original scale and rotation in case the tween didn't exist yet
        rootTransform.localScale = originalScale;
        rootTransform.localRotation = originalRotation;

        float randomRotationAngle = Random.Range(-10f, 10f);
        if (randomRotationAngle > 6f)
            randomRotationAngle = 10f;
        else if (randomRotationAngle < -6f)
            randomRotationAngle = -10f;

        // Start new tweens for scale and rotation
        scaleTween = rootTransform.DOScale(originalScale * 1.12f, 0.2f).SetLoops(2, LoopType.Yoyo);
        rotateTween = rootTransform.DORotateQuaternion(originalRotation * Quaternion.Euler(0, 0, randomRotationAngle), 0.2f)
                               .SetLoops(2, LoopType.Yoyo)
                               .SetEase(Ease.InOutSine);
    }
}
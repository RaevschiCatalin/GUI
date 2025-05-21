using UnityEngine;

public class DragController : MonoBehaviour
{
    public Camera cam;                    // assign in inspector
    public LayerMask draggableLayer;      // the “Draggable” layer

    [Tooltip("Higher = snappier follow")]
    public float smoothSpeed = 10f;

    private Rigidbody grabbedRb;
    private float grabDistance;
    private Vector3 grabOffset;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        // On mouse-down: try to pick up
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 30f, draggableLayer))
            {
                grabbedRb = hit.rigidbody;
                grabDistance = hit.distance;
                grabOffset = hit.transform.position - hit.point;
            }
        }

        // On mouse-up: release
        if (grabbedRb != null && Input.GetMouseButtonUp(0))
        {
            grabbedRb = null;
        }
    }

    void FixedUpdate()
    {
        // While holding: move via physics
        if (grabbedRb != null)
        {
            // Convert mouse to world position at original depth
            Vector3 screenPoint = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                grabDistance
            );
            Vector3 targetWorld = cam.ScreenToWorldPoint(screenPoint) + grabOffset;

            // Smoothly move the rigidbody; this preserves collision
            Vector3 smoothed = Vector3.Lerp(
                grabbedRb.position,
                targetWorld,
                Time.fixedDeltaTime * smoothSpeed
            );
            grabbedRb.MovePosition(smoothed);
        }
    }
}

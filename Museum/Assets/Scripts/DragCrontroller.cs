using UnityEngine;

public class DragController : MonoBehaviour
{
    [Header("References")]
    public Camera cam;               
    public LayerMask draggableLayer;   

    [Header("Drag Settings")]
    [Tooltip("Higher = tighter follow")]
    public float smoothSpeed = 10f;

    // runtime
    private Rigidbody grabbedRb;
    private SnapPoint grabbedSnap;
    private float grabDistance;
    private Vector3 grabOffset;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {

        TrySnapIntoPlace();
        // PICK UP
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, draggableLayer))
            {
                var rb = hit.rigidbody;
                if (rb == null) return;

                // grab any SnapPoint on this object
                var snap = rb.GetComponent<SnapPoint>()
                        ?? rb.GetComponentInChildren<SnapPoint>();

                // if it’s already snapped, don’t pick it up again
                if (snap != null && snap.isSnapped)
                    return;

                // begin dragging
                grabbedRb = rb;
                grabbedSnap = snap;
                grabDistance = hit.distance;
                grabOffset = hit.transform.position - hit.point;

                // turn off physics reactions while dragging
                grabbedRb.isKinematic = true;
                grabbedRb.useGravity = false;
            }
        }

        // RELEASE
        if (grabbedRb != null && Input.GetMouseButtonUp(0))
        {
            // if we didn’t snap, re-enable normal physics
            if (grabbedSnap == null || !grabbedSnap.isSnapped)
            {
                grabbedRb.isKinematic = false;
                grabbedRb.useGravity = true;
            }

            grabbedRb = null;
            grabbedSnap = null;
        }
    }

    void FixedUpdate()
    {
        
        if (grabbedRb != null)
        {
            // compute world-space target under mouse
            Vector3 screenPt = new Vector3(Input.mousePosition.x, Input.mousePosition.y, grabDistance);
            Vector3 worldPoint = cam.ScreenToWorldPoint(screenPt) + grabOffset;

            // smooth-move there
            Vector3 smoothed = Vector3.Lerp(
                grabbedRb.position,
                worldPoint,
                Time.fixedDeltaTime * smoothSpeed
            );
            grabbedRb.MovePosition(smoothed);

            if (grabbedSnap != null && !grabbedSnap.isSnapped)
            {
                float dist = Vector3.Distance(grabbedRb.position, grabbedSnap.target.position);
                if (dist <= grabbedSnap.threshold)
                {
                    SnapNow();
                }
            }
        }
    }

    private void TrySnapIntoPlace()
    {
        if (grabbedSnap == null) return;

        float dist = Vector3.Distance(grabbedRb.position, grabbedSnap.target.position);
        Debug.Log($"[DragController] Release-snap dist={dist:0.00}, thresh={grabbedSnap.threshold}");
        if (dist <= grabbedSnap.threshold)
            SnapNow();
    }

    private void SnapNow()
    {
        // move & rotate exactly
        grabbedRb.transform.SetPositionAndRotation(
            grabbedSnap.target.position,
            grabbedSnap.target.rotation
        );

        // lock it in place
        grabbedRb.isKinematic = true;
        grabbedRb.useGravity = false;
        //grabbedRb.transform.SetParent(grabbedSnap.target, true);
        grabbedSnap.isSnapped = true;
        PuzzleManager.Instance?.RegisterSnap(grabbedSnap);

        Debug.Log("[DragController] Bone snapped into place!");

        // stop dragging
        grabbedRb = null;
        grabbedSnap = null;
    }
}

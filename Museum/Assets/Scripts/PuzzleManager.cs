using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [Header("Objects that disappear when puzzle is solved")]
    [Tooltip("Drag the objects to disappear GameObject here.")]
    public GameObject[] objectsToDisable;

    [Header("Puzzle pieces")]
    [Tooltip("All SnapPoints that belong to this puzzle. Leave empty to auto-find.")]
    public SnapPoint[] snapPoints;

    private int snappedCount = 0;

    void Awake()
    {
        // quick singleton – only one manager allowed
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // If the user didn’t fill the list, find everything in the scene.
        if (snapPoints == null || snapPoints.Length == 0)
            snapPoints = FindObjectsOfType<SnapPoint>();
    }

    // Called by DragController when a piece locks in.
    public void RegisterSnap(SnapPoint snapped)
    {
        snappedCount++;
        Debug.Log($"[PuzzleManager] Piece snapped ({snappedCount}/{snapPoints.Length})");

        if (snappedCount >= snapPoints.Length)
            OpenDoor();
    }

    private void OpenDoor()
    {
        foreach (var go in objectsToDisable)
            if (go != null) go.SetActive(false);
        GameTimer.Instance?.Stop();

        Debug.Log("[PuzzleManager] Puzzle solved – all objects disabled.");
    }
}

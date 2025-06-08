using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    [Tooltip("Where this object should snap to")]
    public Transform target;

    [Tooltip("Max distance (world units) from target to snap")]
    public float threshold = 0.5f;

    [HideInInspector] public bool isSnapped = false;
}

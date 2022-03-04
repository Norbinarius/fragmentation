using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform ship;
    [SerializeField] private float followSpeed = 0.2f;
    [SerializeField] public Vector3 Offset {get; set;}
    public const int Z_OFFSET = -15;

    private void Start()
    {
        Offset = new Vector3(0, 0, Z_OFFSET);
    }

    private void FixedUpdate()
    {
       transform.position = Vector3.Lerp(transform.position, ship.position + Offset, followSpeed);
    }
}

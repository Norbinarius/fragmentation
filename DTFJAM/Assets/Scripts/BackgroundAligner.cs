using UnityEngine;

public class BackgroundAligner : MonoBehaviour
{
    private Transform background;
    private Transform ship;

    private void Start()
    {
        ship = GameObject.FindObjectOfType<ShipController>().transform;
        background = this.transform;
        ApplyPostion();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == Constants.Tags.PLAYER)
        {
            ApplyPostion();
        }
    }
    private void ApplyPostion()
    {
        background.position = ship.position;
    }
}

using UnityEngine;

public class EnemyRotator : Enemy
{
    [SerializeField] private float speed;
    private bool isDestroyed;
    public override void Destroy()
    {
        base.Destroy();
        isDestroyed = true;
    }
    private void FixedUpdate()
    {
        if(isDestroyed)
            return;
        
        this.transform.Rotate(new Vector3(0, 0, speed));
    }
}

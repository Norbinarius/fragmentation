using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyFollow : Enemy
{
    private SpriteRenderer spriteRenderer;
    private float followSpeed;
    public bool IsFollowAllowed {get; set;}
    private const float SPEED_UPPER_RANGE = 0.09f;
    private const float SPEED_LOWER_RANGE = 0.04f;
    
    public override void Start()
    {
        base.Start();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        followSpeed = Random.Range(SPEED_LOWER_RANGE, SPEED_UPPER_RANGE);
    }

    private void FixedUpdate()
    {
        if(IsFollowAllowed)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, ship.transform.position, followSpeed);

            float color = (Mathf.Sin(Time.time * 6f)+1)/4;
            spriteRenderer.color = new Color(color, color, color);
        } 
        else 
        {
            spriteRenderer.color = new Color(0,0,0);
        }
    }
}

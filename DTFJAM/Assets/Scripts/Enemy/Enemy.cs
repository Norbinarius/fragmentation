using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Explodable))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDestroyable
{
    public EnemyType enemyType;
    protected ShipController ship;
    private Explodable explodable;

    public virtual void Start()
    {
        ship = GameObject.FindObjectOfType<ShipController>();
        explodable = this.GetComponent<Explodable>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == Constants.Tags.PLAYER && !ship.IsDead)
        {
            ShipExplode();
        }
        if(col.tag == Constants.Tags.ENEMY && enemyType != EnemyType.arena && !ship.IsDead)
        {
            Destroy();
        }
    } 

    public virtual void Destroy()
    {
        explodable.explode();

        AudioManager.current.PlaySFX(AudioManager.current.kill);

        ResetShipDash();

        var arena = GameObject.FindObjectsOfType<ArenaBase>().Where(x => x.IsArenaActive).FirstOrDefault();
        arena?.enemyDestroy?.Invoke();
    } 

    public virtual void ShipExplode()
    {
        ship.DestroyWithRestart();
    } 

    private void ResetShipDash()
    {
        if(ship.dashCooldown != null)
            StopCoroutine(ship.dashCooldown);
            
        ship.IsDashActive = true;
    }
}

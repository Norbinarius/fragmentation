using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ArenaBase : MonoBehaviour
{
    [SerializeField] protected ShipController ship;
    [SerializeField] protected Arena arena;
    [SerializeField] protected CameraFollow cameraFollow;
    [SerializeField] protected ParticleSystem pSystem;
    public bool IsArenaActive {get; set;}
    protected bool isActivated;
    protected Enemy[] enemyCollisions;
    protected int destroyCount;
    public Action enemyDestroy;

    public virtual void Start()
    {
        enemyCollisions = arena.GetComponentsInChildren<Enemy>();
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == Constants.Tags.PLAYER && ship.Rb.velocity.magnitude < 0.3f && !isActivated)
        {
            ActivateArena();
            isActivated = true;
        }
    }
    public virtual void OnEnemyDestroy()
    {
        Debug.Log("Enemy Destroyed");
        destroyCount++;
    }

    public virtual void ActivateArena()
    {
        Debug.Log("Activated");
    }

    private void OnEnable()
    {
        enemyDestroy += OnEnemyDestroy;
    }
  
    private void OnDisable()
    {
        enemyDestroy -= OnEnemyDestroy;
    }
}

using System.Collections;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class ArenaActivator : ArenaBase
{  
    [SerializeField] private AudioClip music;
    private SpriteRenderer circle;
    private int followersCount; 
    private const int BORDER_COUNT = 4;
    private const float ARENA_OVERVIEW_CAMERA_OFFSET = -65f;
     
    [Header("X and Y Randomize bounds")]
    [SerializeField] private Vector2 xRangePositions;
    [SerializeField] private Vector2 yRangePositions;
    public Vector2 PosX { get { return xRangePositions;}}
    public Vector2 PosY { get { return yRangePositions;}}

    private void Awake()
    {
        circle = this.GetComponent<SpriteRenderer>();
    }

    public override void Start()
    {
        base.Start();

        followersCount = enemyCollisions.Count(a => a.enemyType == EnemyType.follower);
    }

    public override void OnEnemyDestroy()
    {
        base.OnEnemyDestroy();

        if(destroyCount == enemyCollisions.Count() - BORDER_COUNT)
        {
            StartCoroutine(DestroyAnimation());
        }
    }

    public override void ActivateArena()
    {
        base.ActivateArena();

        StartCoroutine(ActivateAnimation());
    }

    private IEnumerator ActivateAnimation()
    {
        IsArenaActive = true;
        GameProgresser.isArenaSpawnActive = true;

        pSystem.Stop();
        StopShip();
        StartCoroutine(AudioManager.current.music.Fade(0.5f, 0f));

        yield return new WaitForSeconds(0.5f);

        AudioManager.current.PlaySFX(AudioManager.current.arenaOpen, 0.5f);
        cameraFollow.Offset = new Vector3(0, 0, ARENA_OVERVIEW_CAMERA_OFFSET);
        arena.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        AudioManager.current.music.clip = music;
        AudioManager.current.music.Play();
        StartCoroutine(AudioManager.current.music.Fade(3f, 0.45f));

        ship.IsDead = false;
        cameraFollow.Offset = new Vector3(0, 0, CameraFollow.Z_OFFSET);

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while(followersCount != 0)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.5f, 5f));

            var followers = arena.GetComponentsInChildren<EnemyFollow>();
            followers.FirstOrDefault(a => !a.IsFollowAllowed).IsFollowAllowed = true;
            followersCount--;
        }
    }

    private IEnumerator DestroyAnimation()
    {
        StopShip();
        StartCoroutine(AudioManager.current.music.Fade(0.5f, 0f));

        GameProgresser.current.SaveProgress();

        yield return new WaitForSeconds(0.5f);

        cameraFollow.Offset = new Vector3(0, 0, ARENA_OVERVIEW_CAMERA_OFFSET);
        circle.enabled = false;

        yield return new WaitForSeconds(0.5f);

        AudioManager.current.PlaySFX(AudioManager.current.arenaExplode);

        foreach(var arenaBound in enemyCollisions.Where(x => x.enemyType == EnemyType.arena))
        {
            arenaBound.GetComponent<Explodable>().explode();
        }

        yield return new WaitForSeconds(4f);

        cameraFollow.Offset = new Vector3(0, 0, CameraFollow.Z_OFFSET);
        ship.IsDead = false;

        AudioManager.current.music.clip = AudioManager.current.calmMusic;
        AudioManager.current.music.Play();
        StartCoroutine(AudioManager.current.music.Fade(0.5f, 0.45f));
    }

    private void StopShip()
    {
        ship.Rb.velocity = Vector3.zero;
        ship.Rb.angularVelocity = 0;
        ship.IsDead = true;
    }

    public void SetEnabled(bool state)
    {
        circle.enabled = state;
        this.GetComponent<Collider2D>().enabled = state;

        if(state)
        {
            pSystem.Play();
        }
        else
        {
            pSystem.Stop();
        }
    }
}

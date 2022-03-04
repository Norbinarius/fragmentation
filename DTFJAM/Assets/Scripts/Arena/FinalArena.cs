using System.Collections;
using UnityEngine;
using System.Linq;

public class FinalArena : ArenaBase
{
    [SerializeField] private Animator animCanvas;

    public override void Start()
    {
        base.Start();
        isActivated = true;
    }

    public override void OnEnemyDestroy()
    {
        base.OnEnemyDestroy();

        if(destroyCount == enemyCollisions.Count())
        {
            isActivated = false;
            pSystem.Play();
        }
    }

    public override void ActivateArena()
    {
        base.ActivateArena();

        StartCoroutine(FinalAnimation());
    }

    private IEnumerator FinalAnimation(){
        pSystem.Stop();
        ship.Destroy();

        var shipLastPos = ship.transform.position;

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i <= 25; i+=5)
        {
           AudioManager.current.PlaySFX(AudioManager.current.arenaOpen, 0.3f);
           cameraFollow.transform.position = new Vector3(shipLastPos.x, shipLastPos.y, CameraFollow.Z_OFFSET - i);
           yield return new WaitForSeconds(0.6f); 
        }
        animCanvas.Play(Constants.Animations.FINAL_ANIMATION);

        yield return new WaitForSeconds(2.5f);

        GameProgresser.current.ResetProgress();

        AudioManager.current.music.SwapAndPlay(AudioManager.current.endMusic);
        StartCoroutine(AudioManager.current.music.Fade(0.5f, 0.45f));
    }
}

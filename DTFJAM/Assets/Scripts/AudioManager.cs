using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource engine, music;
    public AudioSource[] sfxs;

    public AudioClip calmMusic, endMusic;
    public AudioClip dash, kill, arenaExplode, arenaOpen, arenaClose, death;
    public static AudioManager current;
    private void Awake()
    {
        current = this;
    }

    public void PlaySFX(AudioClip audioClip, float volume = 1f)
    {
        sfxs[Random.Range(0, 3)].PlayOneShot(audioClip, volume);
    } 
}

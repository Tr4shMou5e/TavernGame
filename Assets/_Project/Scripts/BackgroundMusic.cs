using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    void Update() => PlayBackgroundMusic();
    void PlayBackgroundMusic()
    {
        if(!musicSource.isPlaying)
            SoundManager.PlayMusic(SoundType.BackgroundMusic, musicSource, 0.2f);
    }
}
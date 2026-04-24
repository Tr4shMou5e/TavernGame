using System;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    public static SoundManager Instance => instance;
    public AudioSource audioSource;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    public static void PlayMusic(SoundType sound, AudioSource source, float volume = 1f)
    {
        var clips = instance.soundList[(int)sound].Sounds;
        var clip = clips[Random.Range(0, clips.Length)];
        source.volume = volume;
        source.clip = clip;
        source.Play();
    }
    
    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        var clips = instance.soundList[(int)sound].Sounds;
        var clip = clips[Random.Range(0, clips.Length)];
        instance.audioSource.pitch = Random.Range(0.9f, 1.1f);
        instance.audioSource.PlayOneShot(clip, volume);
    }
    public static void PlaySound(SoundType sound, Vector3 position, float volume = 1f)
    {
        var clips = instance.soundList[(int)sound].Sounds;
        var clip = clips[Random.Range(0, clips.Length)];
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
    
    #if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
    #endif
}
[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds => sounds;
    [SerializeField] public string name;
    [SerializeField] private AudioClip[] sounds;
}
public enum SoundType
{
    Footstep,
    Oven,
    WinSound,
    LoseSound,
    ScoreSound,
    BackgroundMusic,
    BuySound,
    DoorOpen,
    DoorClose
}

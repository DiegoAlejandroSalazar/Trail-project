using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private Sound[] musicSounds, sfxSounds;
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField][Range(0f, 1f)] float pitchRandomness;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMusic("Background");
    }
    public void PlayMusic(string name)
    {
        if (this == null) return;

        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("sound not found" + s.name);
            return;
        }
        else
        {
            musicSource.clip = s.audioClip;
            musicSource.Play();
        }
    }
    public void PlaySfx(string name, bool randomPitch, float volume, float forcedPitch = 1f)
    {
        if (this == null) return;
        
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("sfx not found");
            return;
        }
        else
        {
            if (randomPitch)
            {
                sfxSource.pitch = UnityEngine.Random.Range(pitchRandomness, pitchRandomness * 2);
            }
            else
            {
                sfxSource.pitch = forcedPitch;
            }
            sfxSource.PlayOneShot(s.audioClip, volume);
        }
    }
}

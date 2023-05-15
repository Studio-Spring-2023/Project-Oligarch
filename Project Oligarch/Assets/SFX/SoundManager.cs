using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public bool loop;
    [Range(0f, 1f)] public float volume = 1f;
    [HideInInspector] public AudioSource source; // Add AudioSource reference
}

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Create AudioSources and assign them to each sound
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
        }
    }

    public void PlaySound(int index, Vector3 sourcePosition)
    {
        if (index >= 0 && index < sounds.Length)
        {
            Sound sound = sounds[index];
            AudioSource audioSource = sound.source;

            audioSource.clip = sound.clip;
            audioSource.loop = sound.loop;
            audioSource.volume = sound.volume;
            audioSource.pitch = 1f;

            audioSource.transform.position = sourcePosition;
            audioSource.Play();
        }
    }

    public void StopAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void StopSound(int index)
    {
        if (index >= 0 && index < sounds.Length)
        {
            Sound sound = sounds[index];
            sound.source.Stop();
        }
    }

    public void SetSoundSpeed(int index, float newSpeed)
    {
        if (index >= 0 && index < sounds.Length)
        {
            Sound sound = sounds[index];
            sound.source.pitch = newSpeed;
        }
    }

    public void SetSoundVolume(int index, float volume)
    {
        if (index >= 0 && index < sounds.Length)
        {
            Sound sound = sounds[index];
            sound.volume = volume;
            sound.source.volume = volume;
        }
    }
}

using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip[] clips;
    public bool loop;
    [Range(0f, 1f)] public float volume = 1f;
    [HideInInspector] public AudioSource source;
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
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            if (sound.clips.Length > 0)
            {
                int randomIndex = Random.Range(0, sound.clips.Length);
                sound.source.clip = sound.clips[randomIndex];
            }

            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
        }
    }

    public void PlaySound(AudioSource audioSource, float maxDistance)
    {
        if (sounds.Length > 0)
        {
            Sound sound = sounds[Random.Range(0, sounds.Length)];

            if (sound.clips.Length > 0)
            {
                int randomIndex = Random.Range(0, sound.clips.Length);
                audioSource.clip = sound.clips[randomIndex];
            }

            audioSource.loop = sound.loop;
            audioSource.volume = sound.volume;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = maxDistance;

            audioSource.Play();

            Debug.Log("Playing sound: " + audioSource.clip.name);
        }
    }

    public void StopAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void StopSound(int index, GameObject obj)
    {
        if (index >= 0 && index < sounds.Length)
        {
            Sound sound = sounds[index];
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip == sound.source.clip)
            {
                audioSource.Stop();
            }
        }
    }

    public void SetSoundSpeed(AudioSource audioSource, float newSpeed)
    {
        if (audioSource != null)
        {
            audioSource.pitch = newSpeed;
        }
    }

    public void SetSoundVolume(AudioSource audioSource, float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}

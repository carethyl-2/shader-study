
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioComponent OwnerComponent;
    AudioSource AudioSource;
    int index;

    public void PlaySound(AudioClip _clip, float _pitch, float _volume, int _index, bool _2DSound, AudioComponent _owner)
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = _clip;
        AudioSource.pitch = _pitch;
        AudioSource.volume = _volume;

        if (_2DSound) AudioSource.spatialBlend = 0.0f;

        AudioSource.Play();

        OwnerComponent = _owner;

        index = _index;
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioSource)
        {
            if (!AudioSource.isPlaying)
            {
                // If sound is looping
                if (OwnerComponent.m_loopSound)
                {
                    // Replay sound with same index
                    OwnerComponent.PlaySoundFromIndex(index);
                }

                Destroy(gameObject);
            }
        }
    }
}

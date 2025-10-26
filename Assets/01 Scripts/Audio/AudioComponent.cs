/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2024 Media Design School
File Name : AudioComponent.cs
Description : Provides function for playing and randomizing sounds.
Author : Kieran Bishop
Mail : kieran.bishop@mds.ac.nz
*/

using UnityEngine;

public class AudioComponent : MonoBehaviour
{
    [SerializeField] AudioClip[] m_audioClips;
    [SerializeField] float m_pitch = 1.0f;
    [SerializeField] float m_randomPitchOffset = 0.15f;
    [SerializeField] bool m_playOnStart = false;
    [SerializeField] bool m_2DSound = false;
    public bool m_loopSound = false;

    int selectedIndex;
    AudioClip selectedClip;
    float selectedVolume = 1.0f;
    float selectedPitch;

    /// <summary>
    /// Execution begins here.
    /// </summary>
    private void Start()
    {
        if (m_playOnStart)
        {
            PlaySound();
        }
    }

    /// <summary>
    /// Play selected sound from clips list
    /// </summary>
    public void PlaySoundFromIndex(int _index)
    {
        // Select specified clip
        selectedIndex = _index;

        // Create sound player
        PlaySound();
    }

    /// <summary>
    /// Play random sound from clips list
    /// </summary>
    public void PlayRandomSound()
    {
        // Choose random clip
        selectedIndex = Random.Range(0, m_audioClips.Length);

        // Create sound player
        PlaySound();
    }

    /// <summary>
    /// Play sound with current settings.
    /// </summary>
    void PlaySound()
    {
        // Select clip
        selectedClip = m_audioClips[selectedIndex];

        // Set random pitch
        selectedPitch = m_pitch + Random.Range(-m_randomPitchOffset, m_randomPitchOffset);

        // Select volume
        selectedVolume = 1.0f;

        // Create sound player obejct and play sound
        GameObject audioPlayerObject = Instantiate(GameManager.Instance.audioPlayerPrefab, transform.position, transform.rotation);
        audioPlayerObject.GetComponent<AudioPlayer>().PlaySound(selectedClip, selectedPitch, selectedVolume, selectedIndex, m_2DSound, this);
    }
}

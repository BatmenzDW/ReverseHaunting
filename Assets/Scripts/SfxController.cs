using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SfxController : MonoBehaviour
{
    public List<AudioClip> audioClips;

    private AudioSource _audioSource;

    private AudioClip GetByName(string clipName) => audioClips.FirstOrDefault(clip => clip.name == clipName);

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string clipName)
    {
        _audioSource.clip = GetByName(clipName);
        _audioSource.Play();
    }
}

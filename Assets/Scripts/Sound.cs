using UnityEngine;
using UnityEngine.Audio;

[System.Serializable] // To make the class show up in the inspector
public class Sound 
{
    public string name; // Name of the sound
    public AudioClip clip; // The audio clip
    [Range(0f, 1f)]
    public float volume; // Volume of the sound
    public float pitch = 1f; // Pitch of the sound
    public bool loop; // Whether the sound should loop
    
    [HideInInspector]
    public AudioSource source; // The AudioSource for this sound, managed at runtime

}

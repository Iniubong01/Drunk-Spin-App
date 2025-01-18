using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DS_Settings : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;

    // Start is called before the first frame update
    void Start()
    {
        // Check the initial state of the toggle and set sound accordingly
        soundToggle.isOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1; // Default is sound on
        UpdateSoundState();

        // Add a listener to the toggle to update sound state when it's changed
        soundToggle.onValueChanged.AddListener(delegate { UpdateSoundState(); });
    }

    // Update the sound state based on the toggle's value
    private void UpdateSoundState()
    {
        bool isSoundOn = soundToggle.isOn;
        PlayerPrefs.SetInt("SoundEnabled", isSoundOn ? 1 : 0); // Save the state

        if (isSoundOn)
        {
            DS_SoundManager.instance.Play("Theme"); // Resume sound
        }
        else
        {
            DS_SoundManager.instance.Stop("Theme"); // Mute sound
        }

        AudioListener.volume = isSoundOn ? 1 : 0; // Adjust global volume (optional)
    }
} 
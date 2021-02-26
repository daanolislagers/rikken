using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneDropdownScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown dropdown;

    void Start()
    {
        foreach (var device in Microphone.devices)
        {
            dropdown.options.Add(new Dropdown.OptionData(device.ToString()));
        }
        dropdown.value = PlayerPrefs.GetInt("MicID");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMic()
    {
        PlayerPrefs.SetInt("MicID", dropdown.value);
        PlayerPrefs.SetString("Mic", Microphone.devices[dropdown.value]);
    }
}

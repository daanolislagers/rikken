using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNameChangeScript : MonoBehaviour
{

    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        input.text = PlayerPrefs.GetString("RoomName");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRoomName()
    {
        PlayerPrefs.SetString("MicID", input.text);
    }
}

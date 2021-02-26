using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCanvasScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

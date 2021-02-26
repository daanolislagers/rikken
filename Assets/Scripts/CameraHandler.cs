using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    Vector3 handoffset = new Vector3(0.4f, 0f, 0f);
    Vector3 handonerotation = new Vector3(-75f, 0f, 0f);
    Vector3 handthreerotation = new Vector3(-75f, 180f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(int player)
    {
        Vector3 newPos;
        Vector3 newRotation;
        switch (player)
        {
            case 2:
                newPos = new Vector3(-1.198195f, 1.6576f, 0f);
                newRotation = new Vector3(37.1f, 90f, 0f);
                SetHandOffset();
                break;
            case 3:
                newPos = new Vector3(0f, 1.6576f, 1.198195f);
                newRotation = new Vector3(37.1f, 180f, 0f);
                break;
            case 4:
                newPos = new Vector3(1.198195f, 1.6576f, 0f);
                newRotation = new Vector3(37.1f, -90f, 0f);
                SetHandOffset();
                break;
            default:
                newPos = new Vector3(0f, 1.6576f, -1.198195f);
                newRotation = new Vector3(37.1f, 0f, 0f);
                break;
        }

        iTween.MoveTo(this.gameObject, newPos, 1.5f);
        iTween.RotateTo(this.gameObject, newRotation, 1.5f);
    }

    public void SetHandOffset()
    {
        //GameObject.Find("Player1Hand").transform.position = GameObject.Find("Player1Hand").transform.position - handoffset;
        iTween.RotateTo(GameObject.Find("Player1Hand").gameObject, handonerotation, 0.5f);
        //GameObject.Find("Player3Hand").transform.position = GameObject.Find("Player3Hand").transform.position + handoffset;
        iTween.RotateTo(GameObject.Find("Player3Hand").gameObject, handthreerotation, 0.5f);
    }

}

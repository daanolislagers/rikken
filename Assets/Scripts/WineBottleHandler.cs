using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineBottleHandler : MonoBehaviourPunCallbacks
{

    public Camera mainCam;
    public Vector3 orgPos = new Vector3(-0.6329032f, 0.8383175f, -0.3923076f);
    public Vector3 orgRot = new Vector3(0f, 145.092f, 0);
    public Vector3 newPos = new Vector3(-0.383f, 1.508f, -0.275f);
    public Vector3 newPosOffset = new Vector3(-0, 0, -0);
    public Vector3 newRot = new Vector3(0f, 145.092f, -121.676f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    photonView.RPC("AnimateBottle", RpcTarget.Others);
                    AnimateBottle();
                }
            }
        }*/
    }

    public void AnimateBottleToGlass(int glass)
    {
        //AnimateBottle(glass);
        photonView.RPC("AnimateBottle", RpcTarget.All, glass);
    }



    [PunRPC]
    public void AnimateBottle(int glass)
    {
        GameObject glassObject = GameObject.Find("Glass" + glass);


        iTween.MoveTo(this.gameObject, iTween.Hash(
                                     "position", glassObject.transform.position + newPosOffset,
                                     "time", 1,
                                     "easeType", iTween.EaseType.easeOutSine));
        iTween.RotateTo(this.gameObject, iTween.Hash(
                         "rotation", newRot,
                         "time", 1,
                         "easeType", iTween.EaseType.easeOutSine));

        iTween.MoveTo(this.gameObject, iTween.Hash(
                         "position", orgPos,
                         "time", 1,
                         "delay", 2.5,
                         "easeType", iTween.EaseType.easeOutSine));
        iTween.RotateTo(this.gameObject, iTween.Hash(
                         "rotation", orgRot,
                         "time", 1,
                         "delay", 2.5,
                         "easeType", iTween.EaseType.easeOutSine));
    }
}

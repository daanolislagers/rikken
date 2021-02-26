using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandHandler : MonoBehaviour
{
    public Camera camera;
    public Vector3 offset = new Vector3(0f, 0.01f, 0f);
    public GameManager manager;
    public int handId;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < transform.childCount; i++) 
        {
            Transform child = transform.GetChild(i);
            child.localRotation = Quaternion.identity;
            Vector3 offset = new Vector3(i * 0.06f, 0, 0);
            child.localPosition = Vector3.zero + offset;
            child.localScale = Vector3.one / 100;
        }


        //check if current player owns hand
        if (manager.CurrentPlayer == handId && manager.GameHasStarted)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Card" && hit.collider.gameObject.transform.parent == this.gameObject.transform)
                {
                    if (Input.GetMouseButtonDown(0))
                    { // if left button pressed...
                        manager.PlayerPlayCard(hit.collider.gameObject.transform.GetSiblingIndex());
                    }
                    hit.collider.gameObject.transform.position += offset;
                    //Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
    }

}

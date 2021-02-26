using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickHandler : MonoBehaviour
{

    public Camera mainCam;
    public Vector3 offset = new Vector3(0f, 0.01f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localRotation = Quaternion.identity;
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one / 100;
        }
        
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Card" && hit.collider.gameObject.transform.parent == this.gameObject.transform)
            {
                if(hit.collider.gameObject.transform.parent == this.gameObject.transform)
                    hit.collider.gameObject.transform.localScale = Vector3.one / 100 * 1.5f;
            }
        }
        
        
    }
}

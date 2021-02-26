using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.02f, 0);
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        Debug.Log("MouseOver");
        this.transform.position += offset;
    }

    void OnMouseExit()
    {
        Debug.Log("MouseExit");
        this.transform.position -= offset;
    }

    void OnMouseDown()
    {
        // this object was clicked - do something
        Debug.Log("MouseDown");
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickStackHandler : MonoBehaviour
{
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
            if(child.tag == "Card")
            {
                child.localRotation = Quaternion.identity;
                child.localPosition = Vector3.zero;
                child.localScale = Vector3.one / 100;
            }
            if (child.tag == "Deck")
            {
                if (transform.childCount == 2)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
}

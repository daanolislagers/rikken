using Rikken;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSuitHandler : MonoBehaviour
{

    public GameManager manager;
    public Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardSuit()
    {
        switch (dropdown.value)
        {
            case 1:
                this.manager.setTrump(CardSuit.Hearts, true);
                break;
            case 2:
                this.manager.setTrump(CardSuit.Cloves, true);
                break;
            case 3:
                this.manager.setTrump(CardSuit.Spades, true);
                break;
            case 4:
                this.manager.setTrump(CardSuit.Diamonds, true);
                break;
            default:
                this.manager.setTrump(CardSuit.Hearts, false);
                break;
        }
    }
}

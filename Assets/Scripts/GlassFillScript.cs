using UnityEngine;

public class GlassFillScript : MonoBehaviour
{

    public Camera mainCam;
    public int GlassID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject.Find("WineBottle").GetComponent<WineBottleHandler>().AnimateBottleToGlass(GlassID);
                }
            }
        }
    }
}

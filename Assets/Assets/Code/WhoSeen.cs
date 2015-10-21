using UnityEngine;
using System.Collections;

public class WhoSeen : MonoBehaviour {

    public bool isStarted = false;


    public Queue SeenObjs = new Queue();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPreRender ()
    {
        if (Time.frameCount % 10000 == 0)
        {
            Debug.Log("In OnPreColl at Frame: " + Time.frameCount.ToString());
        }

       
        

    }
}

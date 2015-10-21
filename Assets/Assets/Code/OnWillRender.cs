using UnityEngine;
using System.Collections;

public class OnWillRender : MonoBehaviour {
    int kount = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        kount++;
	}

    void OnWillRenderObject ()
    {
        if (kount % 100 == 0)
        {

            Debug.Log("Seen by Camera: " + Camera.current.name.ToString());
        }
    }
}

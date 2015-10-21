using UnityEngine;
using System.Collections;
using System.Threading;
public class EndingApp : MonoBehaviour {

    GameObject maincameraObj;
    GameObject camera1;
	GameObject camera2; 
	string cameraState;
    NetMQPubSub pubsub;

    // Use this for initialization
    void Start () {

        

        try {
				camera1 = GameObject.Find("Main Camera");
                
        }
		catch 
			{
				Debug.Log("miss finding object 1");
			}
		try {
			camera2 = GameObject.Find("2ND Camera");
			}
		catch 
			{
				Debug.Log("miss finding object 2");
			}


		camera1.GetComponent<Camera>().enabled = true;
		camera2.GetComponent<Camera>().enabled =false;
		cameraState = "Main";
        pubsub = camera1.GetComponent<NetMQPubSub>();
    }  
	
	// Update is called once per frame
	public void Update () {
		
		if (Input.GetKeyDown(KeyCode.C)) {
			if (cameraState != "Main") {
				camera1.GetComponent<Camera>().enabled = true;
				camera2.GetComponent<Camera>().enabled =false;
				cameraState = "Main";}
			else
				{
				camera1.GetComponent<Camera>().enabled = false;
				camera2.GetComponent<Camera>().enabled =true;
				cameraState = "2ND";}

			Debug.Log("IN kEY cODE c");
		}
        if (pubsub.finalend)
        {
            Debug.Log("button press Detected");
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

        }


    }
    public void quitGame()
    {
        Debug.Log("NetMQ stopped xxx");
        maincameraObj = GameObject.Find("Main Camera");
        NetMQPubSub pubsub = maincameraObj.GetComponent<NetMQPubSub>();
        Debug.Log("pubsub = " + pubsub.ToString());
        Thread.Sleep(100);
        Debug.Log(" ");
        pubsub.endflag = 1; ;
    }

    public void stopNetMQ()
    {
        Debug.Log("NetMQ stopped xxx");
        maincameraObj = GameObject.Find("Main Camera");
        NetMQPubSub pubsub = maincameraObj.GetComponent<NetMQPubSub>();
        Debug.Log("pubsub = " + pubsub.ToString());
        Thread.Sleep(100);
        Debug.Log(" ");
        pubsub.endflag = 1; ;
    }


}

using UnityEngine;
using System.Collections;

public class SeeIt : MonoBehaviour {
    GameObject cameraObj, myParent;
    Camera cameraMain;
    
    Renderer rend;
    
    string myname;
    
    int kount;
    public int messagerate = 100;
   

    
    // Use this for initialization
    void Start () {

        

        cameraObj = GameObject.Find("Main Camera");

        cameraMain = cameraObj.GetComponent<Camera>();
        myParent = transform.root.gameObject;
        myname = gameObject.name;

        Debug.Log("my name: " + myname + " my root: " + myParent.name);
        if (cameraObj != null)
        {
            Debug.Log(myname + " Found camera OBJ: " + cameraObj.name.ToString());
            Debug.Log(myname + " Found Camera Component: " + cameraMain.name.ToString());
        }

        else
        {
            Debug.Log(myname + " Camera Not Found");
        }

        
        
        
        rend = gameObject.GetComponent<Renderer>();
    

        
    }

   

    // Update is called once per frame
    void Update() {

              
        kount++;
        RootParams script;
        script = myParent.GetComponent<RootParams>();
        bool inFrustum;
      
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraMain);
       
        inFrustum = GeometryUtility.TestPlanesAABB(planes, rend.bounds);

        if (!Physics.Linecast(transform.position, cameraMain.transform.position) && inFrustum)
        {
            script.visiblecorners++;
            script.totalcorners++;
        }
        else
        {

            script.totalcorners++;

        }

       
       
	}
}

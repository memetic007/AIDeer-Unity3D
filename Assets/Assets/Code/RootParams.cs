using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class RootParams : MonoBehaviour {

    public string TYPE;
    public string SUBTYPE;
    public string COLOR;
    public string SIZE;
    
    public int visiblecorners;
    public int totalcorners;
    public float ratiocorners;
    GameObject cameraObj;
    string myname;
    string shortname;

    Camera cameraMain;
    WhoSeen script;
    SetAttrib getattributes;
    

    GameObject sensekount,vizratio;
    GameObject whoami;

	// Use this for initialization
	void Start () {

        Dictionary<string, string> returndict = new Dictionary<string, string>();


        visiblecorners = 0;
        cameraObj= GameObject.Find("Main Camera");
        sensekount = GameObject.Find("SensorKountText");
        vizratio = GameObject.Find("VizRatioText");
        cameraMain = cameraObj.GetComponent<Camera>();
        script = cameraObj.GetComponent<WhoSeen>();
        myname = gameObject.name;
        getattributes = cameraObj.GetComponent<SetAttrib>();
        shortname = myname;

        shortname = shortname.Replace(" ", "_");
        shortname = shortname.Replace("(", "");
        shortname = shortname.Replace(")", "");

        if (!getattributes.namesShortLong.ContainsKey(shortname))
        {
            getattributes.namesShortLong.Add(shortname, myname);
        }
        //setting atrribuites TYPE, SUBTYPE, COLOR, SIZE etc

        

        returndict = getattributes.getAttrib(myname);

        TYPE = returndict["TYPE"] + " ";
        SUBTYPE = returndict["SUBTYPE"] + " ";
        COLOR = returndict["COLOR"] + " ";
        SIZE = returndict["SIZE"] + " ";



    }
	
	// Update is called once per frame
	void Update () {
        
	}

   void LateUpdate ()
    {
        string qstring;
        
        double cornerratio= 0;
        // Debug.Log("sensekount = " + sensekount.GetComponent<Text>().Text;

        cornerratio = (double)visiblecorners / (double)totalcorners;
        cornerratio = Math.Round(cornerratio, 2);

        // sensekount.GetComponent<Text>().text = "sensor kount: " + visiblecorners.ToString();
        // vizratio.GetComponent<Text>().text = "Viz Ratio: " + cornerratio.ToString();


        if (visiblecorners > 0)
        {

            string zmqchannelID = "444 U OBJ ";
            string framecountstr = Time.frameCount.ToString() + " ";




            float locx = (float)Math.Round(transform.position.x, 2);
            float locy = (float)Math.Round(transform.position.y, 2);
            float locz = (float)Math.Round(transform.position.z, 2);
            string locxstr = " " + locx.ToString() + " ";
            string locystr = " " + locy.ToString() + " ";
            string loczstr = " " + locz.ToString() + " ";



            qstring = zmqchannelID + framecountstr + shortname + " " + TYPE + SUBTYPE + COLOR + SIZE + locxstr + locystr + loczstr;
            // qstring = "Object: " + myname + " QueueItems: " + script.SeenObjs.Count.ToString() + " Frame: " + Time.frameCount.ToString() + " Sensors: " + visiblecorners.ToString() + " total: " + totalcorners.ToString() + " ratio " + cornerratio.ToString(); ;
            script.SeenObjs.Enqueue(qstring);

            // Debug.Log("enquing " + qstring);
        }
      

        visiblecorners = 0;
        totalcorners = 0;
    }
}

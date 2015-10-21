using UnityEngine;
using System.Collections;
using NetMQ;
using NetMQ.Sockets;
using AsyncIO;
using System.Threading;
using System;
using System.Text;

public class NetMQPubSub : MonoBehaviour
{
    public Queue attentionMsgs = new Queue();
    int sentmessagekount;
    int framekount;
    public int endflag;
    public bool finalend = false;
    WhoSeen whoseen;
    bool receivelaunched = false;
    int loopkount = 0;
    GameObject myParent;
    SetAttrib setattrib;
    string nameAttnGameObject;
    int attnKount;
    string oldattnGameObject;

    void Awake()
    
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 40;
        Application.runInBackground = true;
    }
    // Use this for initialization
    void Start()
    {

        string timestring;
        nameAttnGameObject = "";
        oldattnGameObject = "";
        myParent = transform.root.gameObject;
        whoseen = GetComponent<WhoSeen>();
        setattrib = GetComponent<SetAttrib>();

        timestring = DateTime.Now.ToString();
        Debug.Log("NetMQPubStarting at: " + timestring);
        endflag = 0;
        framekount = 0;
        // StartCoroutine("TalkZMQ");
        StartCoroutine("TalkZMQ");
        attnKount = 0;
    }




    // Update is called once per frame
    void Update()
    {
        int tester = 1;
        string message;
        framekount++;
        
        string newattnGameObject = "";
        string activeattnGameObject = "";

        while (attentionMsgs.Count > 0)
        {


            tester++;
            message = attentionMsgs.Dequeue().ToString();
            string[] msgtokens = message.Split(' ');
            if (msgtokens[3] == "OPENCOGSTARTED")

            {
                whoseen.isStarted = true;
            }
            else
            {
                newattnGameObject = processAttentionMsg(message);
            }
        }
        if (framekount < 20 || attnKount > 4)
        {
            activeattnGameObject = newattnGameObject;
            attnKount = 0;
        }
        else
        {
            activeattnGameObject = oldattnGameObject;
            attnKount++;
        }

        // activeattnGameObject = newattnGameObject;
      
        if (activeattnGameObject.Length > 0 && activeattnGameObject != "NULL")
        {
            Debug.Log("in Update Frame: " + Time.frameCount.ToString() + " >" + activeattnGameObject);
            // need to fix this so only find a new GameObj in attention
            oldattnGameObject = activeattnGameObject;
            tester++;
                

            GameObject attnObj = GameObject.Find(activeattnGameObject);
            myParent = transform.root.gameObject;
            Vector3 previousCameraRotation = transform.rotation.eulerAngles;
            transform.LookAt(attnObj.transform);
            
        }
       

    }
    IEnumerator TalkZMQ()
    {
        string timestring;
        
        using (var context = NetMQContext.Create())
        {
            using (var pubSocket = context.CreatePublisherSocket())
            {
                using (var subSocket = context.CreateSubscriberSocket())
                {
                    using (var poller = new Poller())
                    {

                        int aaa = 0;
                        aaa++;   //for debugging


                        pubSocket.Options.SendHighWatermark = 10000;
                        subSocket.Options.ReceiveHighWatermark = 10000;

                        try
                        {
                            pubSocket.Connect("tcp://localhost:5559");
                        }
                        catch
                        {
                            Debug.Log("pubSocket failed to connect or bind");
                        }


                        try
                        {
                            subSocket.Connect("tcp://localhost:5562");
                        }
                        catch
                        {
                            Debug.Log("subSocket failed to connect ");
                        }

                        subSocket.Subscribe("");

                        poller.AddSocket(subSocket);
                        
                        if (receivelaunched == false)
                        {
                            Debug.Log("ready to launch receiver");

                            subSocket.ReceiveReady += (s, a) =>
                            {
                                

                                loopkount++;

                                string messagestring = a.Socket.ReceiveString();

                                attentionMsgs.Enqueue(messagestring);
                                if (loopkount < 100000)
                                {
                                    Debug.Log("received by netMQPub-Sub" + messagestring);
                                }
                                
                                receivelaunched = true;
                               
                            };
                            poller.PollTillCancelledNonBlocking();
                            Debug.Log("NetMQPub: Sub Poller launched");
                        }


                        while (true) 
                        {
                            string ostring;
                            yield return new WaitForEndOfFrame ();

                            if (!whoseen.isStarted)
                            
                            {
                                if (endflag == 1)
                                {

                                    Debug.Log("Before quit poller");

                                    try
                                    {
                                        poller.CancelAndJoin();
                                        Debug.Log("poller successfully quit");
                                    }
                                    catch (Exception)
                                    {
                                        Debug.Log("error in poller cancel");
                                    }


                                    Debug.Log("exiting co-routine");
                                    finalend = true;
                                    yield break;
                                }


                                string zmqchannelID = "444 U STARTOPENCOG ";
                                string framecountstr = Time.frameCount.ToString() + " ";
                                ostring = zmqchannelID + framecountstr;
                                sentmessagekount = sendString(sentmessagekount, ostring, pubSocket);
                                // yield return null;
                                continue;
                            }

                            else
                            {
                                float locx = (float)Math.Round(transform.position.x, 2);
                                float locy = (float)Math.Round(transform.position.y, 2);
                                float locz = (float)Math.Round(transform.position.z, 2);
                                string locxstr = " " + locx.ToString() + " ";
                                string locystr = " " + locy.ToString() + " ";
                                string loczstr = " " + locz.ToString() + " ";
                                ostring = "444 U STARTFRAME " + Time.frameCount.ToString() + " " + locxstr + locystr + loczstr;
                                sentmessagekount = sendString(sentmessagekount, ostring, pubSocket);

                                while (whoseen.SeenObjs.Count > 0)
                                {

                                    ostring = whoseen.SeenObjs.Dequeue().ToString();
                                    sentmessagekount = sendString(sentmessagekount, ostring, pubSocket);

                                }

                                ostring = "444 U ENDFRAME " + Time.frameCount.ToString();
                                sentmessagekount = sendString(sentmessagekount, ostring, pubSocket);

                                if (endflag == 1)
                                {

                                    Debug.Log("Before quit poller");

                                    try
                                    {
                                        poller.CancelAndJoin();
                                        Debug.Log("poller successfully quit");
                                    }
                                    catch (Exception)
                                    {
                                        Debug.Log("error in poller cancel");
                                    }


                                    Debug.Log("exiting co-routine");
                                    finalend = true;
                                    yield break;
                                }


                                yield return null;
                            }


                        }
                    }
                }

            }
        }

        timestring = DateTime.Now.ToString();

        Debug.Log("DONE at: " + timestring);
        Debug.Log("Quiting at: " + timestring);

    }

    int sendString(int sentmessagekount,string ostring,  PublisherSocket pubSocket)
    {
        
        byte[] oarray;
        
        oarray = Encoding.ASCII.GetBytes(ostring);

        try
        {
            sentmessagekount++;
            pubSocket.Send(oarray);

            // Debug.Log("DQ=====> " + ostring + " messageKount = " + sentmessagekount.ToString());
            if (sentmessagekount < 3)
            {
                Thread.Sleep(1000);
            }
        }
        catch
        {

            Debug.Log("in NetMQPub sendString sending dequeued failed");
        }

        return sentmessagekount;

    }

    string processAttentionMsg(string message)
    {
        string longname, shortname, testname;

        // look up long name from short name

        string[] tokens = message.Split(' ');
        shortname = tokens[3];

        longname = "";

        if (!(shortname == "NULL"))
        {
            setattrib.namesShortLong.TryGetValue(shortname, out longname);

            try
            {
                if (longname.Length == 0)
                {
                    nameAttnGameObject = longname;
                    Debug.Log("Bad shortname: " + shortname);
                }
                else
                {

                    nameAttnGameObject = longname;

                    //  Debug.Log("Names ShortName: " + shortname + " LongName: " + longname);

                }
            }

            catch (Exception)
            {
                testname = longname;
            }
        }
        else
        {
            longname = shortname;
            nameAttnGameObject = longname;
            Debug.Log("NULL in processAttentionMsg");
        }


        return nameAttnGameObject;

       

        // Debug.Log("in Process Attention Frame: " + Time.frameCount.ToString() + " > " + message);
    }
}



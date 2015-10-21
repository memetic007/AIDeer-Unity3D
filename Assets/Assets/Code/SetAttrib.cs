using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class SetAttrib : MonoBehaviour
{
    public Dictionary<string, string> namesShortLong = new Dictionary<string, string>();

    // Use this for initialization
    void Start()
    {
        namesShortLong.Add("NULL", "NULL");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Dictionary<string, string> getAttrib(string objectName)

    {
        Dictionary<string, string> returndict = new Dictionary<string, string>();
        objectName = objectName.ToUpper();

        if (objectName.IndexOf("DEER") > -1)
        {
            returndict.Add("TYPE", "Animal");
            returndict.Add("SUBTYPE", "Deer");
            returndict.Add("COLOR", "Brown");
            returndict.Add("SIZE", "Large");
            return returndict;
        }



        if (objectName.IndexOf("GOAT") > -1)
        {
            returndict.Add("TYPE", "Animal");
            returndict.Add("SUBTYPE", "Goat");
            returndict.Add("COLOR", "White");
            returndict.Add("SIZE", "Medium");
            return returndict;
        }


        if (objectName.IndexOf("BLOCK") > -1)
        {
            returndict.Add("TYPE", "Inanimate");
            returndict.Add("SUBTYPE", "Wall");
            returndict.Add("COLOR", "Green");
            returndict.Add("SIZE", "Large");
            return returndict;
        }

        returndict.Add("TYPE", "NA");
        returndict.Add("SUBTYPE", "NA");
        returndict.Add("COLOR", "NA");
        returndict.Add("SIZE", "NA");
        return returndict;


    }




}

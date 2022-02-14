using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoLabel : MonoBehaviour
{
    public Text text;
    public Text text2;

    public string Distance
    {
        get
        {
           return text.text;
        }
        set
        {
            text.text = value;
        }
    }
    
    public string DistanceLeft
    {
        get
        {
            return text2.text;
        }
        set
        {
            text2.text = value;
        }
    }



    private void Start()
    {
        //Distance = "";
    }
}

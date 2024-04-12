using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class MarkerSize
{
	public MarkerSize(GameObject gameObject, float x, float y, float z) {
        this.gameObject = gameObject;
        this.x = x;
        this.y = y;
        this.z = z;
       
    }
    
     public GameObject gameObject { get; set; }
     public float x { get; set; }
     public float y { get; set; }
     public float z { get; set; }
}

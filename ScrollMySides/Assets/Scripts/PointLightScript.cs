using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightScript : MonoBehaviour {

    Light fire;
    float lightInt;
    public float minInt = 6F,  maxInt = 7F;

    // Use this for initialization
    void Start () {
        fire = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        lightInt = Random.Range(minInt, maxInt);
        fire.intensity = lightInt;
	}
}

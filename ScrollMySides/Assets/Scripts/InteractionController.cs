using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {


    public CameraFollow cf;

    public void OnInteract(GameObject obj)
    {
        if (obj.CompareTag("Relic"))
        {
            Destroy(obj);
            cf.ShakeCamera(0.4f, 4);
        }
    }


	// Use this for initialization
	void Awake ()
    {
    }
	
	// Update is called once per frame
	void Update () {
    }
}

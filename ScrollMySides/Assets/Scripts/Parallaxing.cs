using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds; // Array of back/foregrounds to be parallaxed
    private float[] parallaxScales; // proportion of the bg to move the camera by
    public float smoothing = 1f;    // how smooth the parallaxing is going to be

    private Transform cam;          // reference to the camera's main transform
    private Vector3 previousCamPos; // previous frame camera position

    // called before start. Great for references to gameObjects
    void Awake()
    {
        //setup cam reference
        cam = Camera.main.transform;
    }

	// Use this for initialization
	void Start () {
        //Previous frame had the current cam pos
        previousCamPos = cam.position;

        // assigning corresponding parallax scales
        parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z  * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < backgrounds.Length; i++)
        {
            // parallax is opposite to cam movement
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // set target exposition which is the current pos + parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target pos which is the bg's current pos w. it's target x pos
            Vector3 bgTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current pos and target pos
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, bgTargetPos, smoothing * Time.deltaTime);
        }

        // set previous cam pos to new pos
        previousCamPos = cam.position;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionController : MonoBehaviour {


    public CameraFollow cf;

    public void OnInteract(GameObject obj)
    {
        if (obj.CompareTag("Relic"))
        {
            Destroy(obj);
            cf.ShakeCamera(0.4f, 4);
        }

        if (obj.CompareTag("Elevator"))
        {
            obj.GetComponent<ElevatorActivate>().PlayerTrigger();
            obj.layer = LayerMask.NameToLayer("Default");
        }

        if (obj.CompareTag("Boulder") || obj.CompareTag("Kill") || obj.CompareTag("Despawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

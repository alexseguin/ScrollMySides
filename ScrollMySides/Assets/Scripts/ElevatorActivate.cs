using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorActivate : MonoBehaviour {

    public void PlayerTrigger()
    {
        
        PlatformController pc = GetComponentInParent<PlatformController>();
        pc.Activate();
    }
}

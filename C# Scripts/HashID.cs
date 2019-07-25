using UnityEngine;
using System.Collections;

public class HashID : MonoBehaviour {
    /*Information: 
    Stores the hash IDs for Mecanim, the animator. Useful for programming the animations to sync with the CPU Behavior established in that script. */
    static public int angularSpeedFloat;
    static public int speedFloat;
   // public int idle;
	void Awake()
    {
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        speedFloat = Animator.StringToHash("Speed");
      
       // idle = Animator.StringToHash("HumanoidIdle");
    }
    
    
}

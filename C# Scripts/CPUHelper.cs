using UnityEngine;
using System.Collections;

public class CPUHelper {
    //Helper class for CPUBehavior. Stores float dampening for improved animation. 
    public float dampSpeedTime = 0.1f;
    public float angulardampSpeed = 0.1f;
    public float angularResponseTime = 0.1f;
    HashID hash;
    Animator anim;
    public CPUHelper(Animator anim, HashID hash)
    {
        this.anim = anim;
        this.hash = hash;
    }
    public void Setup(float speed, float angle)
    {
        float angularSpeed = angle / angularResponseTime;
       // anim.SetFloat(hash.angularSpeedFloat, speed, dampSpeedTime, Time.deltaTime);
       // anim.SetFloat(hash.speedFloat, speed, angulardampSpeed, Time.deltaTime);
    }
}

using UnityEngine;
using System.Collections;

public class WaypointNavigation : MonoBehaviour {
    public bool occupied;
    HashID hash;
	// Use this for initialization
    void Awake()
    {
      //  occupied = false; 
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            occupied = true;
            
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "NPC")
        {
         //  other.GetComponent<Animator>().SetFloat("Speed", 0f, 0f, Time.deltaTime);
        }
    }
    void OnTriggerExit(Collider other)
    {
        occupied = false; 
    }
}

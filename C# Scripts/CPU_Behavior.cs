using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
//[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class CPU_Behavior : MonoBehaviour {
    /*This script controls the behavior of the CPUs in our VE */
    [HideInInspector]
    public int currentindex = 0; 
   // NavMeshAgent navigation_path;
    Animator anim;
    public List<GameObject> waypoints;
    PriorityQueue<GameObject, float> mypq;
   // public GameObject train;
   
   
    void Awake()
    { 
      //anim.computePositions
        //navigation_path = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        //Note: better to initialize: VERY slow!
        if(waypoints.Count == 0)
            waypoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("WayPoint"));
         
    }
	// Use this for initialization
	void Start () {
	    
	}
	private bool AreAllWayPointsAreOccupied()
    {
        foreach (GameObject go in waypoints)
            if (!go.gameObject.GetComponent<WaypointNavigation>().occupied) return false;
        return true;
    }
    private bool IsAtDestination()
    {
        return Vector3.Distance(gameObject.transform.position, waypoints[currentindex].transform.position) < .8;
    }
    public void Stop_CPU()
    {
       // navigation_path.Stop();
        anim.SetFloat("Speed", 0f, 1.5f, Time.deltaTime * 2);
       // anim.SetFloat("AngularSpeed", 0, 1f, Time.deltaTime);
      //  anim.SetFloat("AngularSpeed", navigation_path.angularSpeed);
    }
    public void Move_CPU(float speed)
    {
        anim.SetFloat("Speed", speed);
       // anim.SetFloat("AngularSpeed", navigation_path.angularSpeed);
    }
	// Update is called once per frame
	void Update () {
        /*
        if (!AreAllWayPointsAreOccupied()&& MyTrainMotion.trainStopped)
        {
                navigation_path.SetDestination(waypoints[currentindex].transform.position);
                if (!IsAtDestination())
                    Move_CPU(1.5f);
                else {
                    Stop_CPU();
                }
            
        }
        else {
            Stop_CPU();
        }
        */
    }
   

}

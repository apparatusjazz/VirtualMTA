using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeDirectory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool destReached = false;
    public static bool destinationReached(NavMeshAgent mNavMeshAgent)
    {

        if (!mNavMeshAgent.pathPending)
        {
            if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
            {                
                destReached = true;
                return destReached;
                
            }

        }
        return false;
    }
}



/*
 When NPC reaches waypoint, move NPC to train or platform directory folder.
 */
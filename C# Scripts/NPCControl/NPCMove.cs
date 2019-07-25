using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    public Transform _destination;

    NavMeshAgent _navMeshAgent;
    private Animator _animator;
    //public float speed;         //Walking speed


    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();      //Gets animator controller


        _navMeshAgent.destination = _destination.position;
        _animator.SetFloat("Forward", 0.5f);
    }

    void Update()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            //Debug.Log(_navMeshAgent.stoppingDistance + " " +  _navMeshAgent.remainingDistance);
            //_animator.SetFloat("Forward", 0.0f);
            _navMeshAgent.isStopped = false;
            //_animator.SetFloat("Forward", 0.0f);

        }

    }

  
    bool destReached = false;
    public void destinationReached(NavMeshAgent mNavMeshAgent)
    {

        if (!mNavMeshAgent.pathPending)
        {
            if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
            {
                destReached = true;
            }

        }
      
    }
}

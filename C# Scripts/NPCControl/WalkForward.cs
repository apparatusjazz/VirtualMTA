using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class WalkForward : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    NavMeshAgent _navMeshAgent;
    private Animator _animator;
    public float speed;         //Walking speed
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();      //Gets animator controller
        _navMeshAgent.speed = speed;                    //Sets the speed of the NPC to the speed set in the public variable
        rb = this.GetComponent<Rigidbody>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }

    }

    private void Update()
    {
        if (TrainDoors.areDoorsOpen == true)
        {
            //transform.position += transform.forward * Time.deltaTime;
            _animator.SetFloat("Forward", 0.5f);                    //Sets the forward variable to 0.5, the walking animation
            Vector3 targetVector = _destination.transform.position;
            Vector3 a = new Vector3(.5f, 0);
            //transform.Translate(targetVector * Time.deltaTime);
            transform.position += a * Time.deltaTime;


            //SetDestination();
        }


    }

    private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position; //Sets destination to waypoint object
            _navMeshAgent.SetDestination(targetVector);             //Moves agent to targetVector
            _animator.SetFloat("Forward", 0.5f);                    //Sets the forward variable to 0.5, the walking animation
        }
    }
}

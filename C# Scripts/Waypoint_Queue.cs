using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Waypoint_Queue<GameObject, Float>
{
    public PriorityQueue<GameObject, Float> pq;
    public Waypoint_Queue()
    {
        this.pq = new PriorityQueue<GameObject, Float>(OrderingConvention.Max);
    }

    public void Sort()
    {
        pq.Sort();
    }
}

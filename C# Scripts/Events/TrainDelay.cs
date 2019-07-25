using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDelay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }
   

    public static void delayWaitingOnPlatform()
    {
        Train1.secondsArriving = Random.Range(25f, 40f);
    }
    public static void delayWaitingOnTrain()
    {
        Train1.secondsTrainLeave = Random.Range(30f, 50f);

        //Play delay announcement
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        AppearWhenDoorsClose();
    }
    public void AppearWhenDoorsClose()
    {
        if (TrainDoors.areDoorClosed)
        {
            transform.gameObject.SetActive(true);
        }
    }
}

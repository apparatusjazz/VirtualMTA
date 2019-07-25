using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disapear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        DisapearWhenDoorsClose();
    }
    public void DisapearWhenDoorsClose()
    {
        if (TrainDoors.areDoorClosed)
        {
            transform.gameObject.SetActive(false);
        }
    }
}

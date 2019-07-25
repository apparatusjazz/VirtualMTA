using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayUI : MonoBehaviour
{
    private bool displayLabel = false;

    void Start()
    {
        //GameValues.Difficulty = GameValues.Difficulties.Hard; //This is for testing purposes only
        transform.Find("DelayText").gameObject.SetActive(false);
        StartCoroutine(FlashLabel());

    }

    IEnumerator FlashLabel()
    {

        if(GameValues.Difficulty == GameValues.Difficulties.Hard)
        {
            if (Random.Range(1f, 100f) <= 100f)
            {
                TrainDelay.delayWaitingOnPlatform();

                yield return new WaitForSeconds(Train1.secondsArriving - 20);

                for (int i = 0; i < 10; i++)
                {
                    transform.Find("DelayText").gameObject.SetActive(true);
                    yield return new WaitForSeconds(.5f);
                    transform.Find("DelayText").gameObject.SetActive(false);
                    yield return new WaitForSeconds(.5f);
                }
            }
            /*
            if (Random.Range(1f, 100f) <= 50f)
            {
                TrainDelay.delayWaitingOnTrain();
                for (int i = 0; i < 5; i++)
                {
                    transform.Find("DelayText").gameObject.SetActive(true);
                    yield return new WaitForSeconds(.5f);
                    transform.Find("DelayText").gameObject.SetActive(false);
                    yield return new WaitForSeconds(1f);
                }
            }
            */

        }
        transform.gameObject.SetActive(false);

    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        switch (GameValues.Difficulty)
        {
            case GameValues.Difficulties.Easy:
                transform.gameObject.SetActive(false);
                break;

            case GameValues.Difficulties.Medium:
                transform.Find("TrackPeople").gameObject.SetActive(false);
                transform.Find("OtherWoman").gameObject.SetActive(true);
                break;

            case GameValues.Difficulties.Hard:
                
                transform.gameObject.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

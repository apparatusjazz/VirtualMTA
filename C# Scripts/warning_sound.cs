using UnityEngine;

public class warning_sound : MonoBehaviour
{
    public AudioClip warning;   
    void Start()
    {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = warning;
    }

    void OnCollisionEnter(Collision col) 
    {
        // if (col.collider.tag == "Player")
        Debug.Log("1234");
            GetComponent<AudioSource>().Play();
    }
}
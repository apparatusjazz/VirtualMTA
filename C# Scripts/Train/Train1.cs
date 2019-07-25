using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Train1 : MonoBehaviour {

	//Handles train animation in demo scene

	public AudioSource mainPASystem;
	public AudioClip trainArriving;
	public AudioClip ambient;
	public AudioClip announcement;
	public AudioClip leaving;
    public bool new_station = false;
    public static float secondsArriving;
    public static float secondsTrainLeave;

	[HideInInspector]
	public bool trainStopped = false;

	private Light[] lights;
	private Transform startPos;
	private bool playerInside = false;
	AudioSource current;

	Animator anim;

	TrainDoors td;
    GameObject[] walls;

    void Start () { 
        secondsArriving = 25f; //20      //Time until train starts arriving at station
        secondsTrainLeave = 13f;//13     //Time until train leaves the station
        walls = GameObject.FindGameObjectsWithTag("wall");
        anim = GetComponent<Animator>();
		td = GetComponent<TrainDoors>();
		current = GetComponent<AudioSource>();
		current = GetComponent<AudioSource>();
		lights = GetComponentsInChildren<Light> ();
		startPos = this.transform;
        if (!new_station)
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
        }
        else
        {
            //if (Camera.main.GetComponent<CameraShake>() == null)
            //{
               // Camera.main.gameObject.AddComponent<CameraShake>();
               // CameraShake cs = Camera.main.GetComponent<CameraShake>();
               // cs.shakeAmount = 0.01f;
               // cs.enabled = false;
               // Destroy(cs, 15f);
           // }
           // FindObjectOfType<CameraShake>().enabled = true;
        }
	}


    IEnumerator Example()
    {
        yield return new WaitForSeconds(15f); 

    }
    public void Remove_Barriers()               //Removes invisible wall when train arrives
    {
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<Collider>().enabled = false;
        }
    }


        public void TrainDepart() {
		if (playerInside) 		mainPASystem.volume = 0.3f;
		StartCoroutine (leaveStation ());
		StartCoroutine (Depart ());
	}

	public void TrainLaunch() {
        if (!new_station)
        {
            StartCoroutine(LaunchTrain(secondsArriving));
        }
        else
        {
            StartCoroutine(LaunchTrain(0f));
        }
	}

	IEnumerator LaunchTrain(float s) {
        if (secondsArriving > 20)
        {
            yield return new WaitForSeconds(s - 20);        //calculate extra time needed if delay occurs to play audio
        }
        mainPASystem.Stop();
		mainPASystem.clip = trainArriving;
		mainPASystem.Play();
		yield return new WaitForSeconds(s - (s - 20));
		foreach (Light light in lights) {
			light.enabled = true;
		}
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = true;
		}
		anim.SetTrigger("LaunchTrain");
    }

	void OnTriggerEnter(Collider o) {
		td.doorsMoving = false;
		playerInside = true;
		o.transform.SetParent(this.transform);
		mainPASystem.volume = 0.4f;
		current.volume = 1;
		current.spatialBlend = 0;
	}

	void OnTriggerExit(Collider o) {
		o.transform.SetParent(null);
		playerInside = false;
		current.volume = 0.6f;
		mainPASystem.volume = 0.9f;
		current.spatialBlend = 1;
	}

	public void MakeAnnouncementInsideTrain(){
		if (!playerInside) {
			current.spatialBlend = 1;
		}
		current.clip = announcement;
		current.Play ();
	}

	IEnumerator leaveStation() {
		td.CloseDoors();
		yield return new WaitForSeconds(3f);
		current.Stop();
		current.clip = leaving;
		current.Play();
		td.SecureDoors();
		td.doorsMoving = false;
	}

    IEnumerator DisableTrain()
    {
        yield return new WaitForSeconds(15f);
        this.transform.position = startPos.position;
        anim.SetTrigger("reset");
        mainPASystem.volume = 1;
    }


    IEnumerator Depart() {
		yield return new WaitForSeconds(4f);
		anim.SetTrigger("depart");
		mainPASystem.clip = ambient;
		mainPASystem.Play ();
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<Collider>().enabled = true;
        }
        if (playerInside)
        {
           // if (Camera.main.GetComponent<CameraShake>() == null)
            //{
            //    Camera.main.gameObject.AddComponent<CameraShake>();
            //    CameraShake cs = Camera.main.GetComponent<CameraShake>();
            //    cs.shakeAmount = 0.01f;
            //    cs.enabled = false;
           // }
           // FindObjectOfType<CameraShake>().enabled = true;
            if (SceneManager.GetActiveScene().name == "W4")
            {
                yield return new WaitForSeconds(11f);
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GrandSt");
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(secondsTrainLeave); //originally 13
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
        }
        else
        {
            Debug.Log("12345");
            StartCoroutine(DisableTrain());
        }
	}




}

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    private float initialy;
    [SerializeField]
    private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    private AudioSource m_AudioSource;
    private Camera cam;
    private bool isMoving;
    void Start () {
        initialy = gameObject.transform.position.y;
        m_AudioSource = GetComponent<AudioSource>();
        cam = GetComponentInChildren<Camera>();
    }
	bool Grounded()
    {
        return gameObject.transform.position.y == initialy;  
    }
    private void PlayFootStepAudio()
    {
        int index = Random.Range(0, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[index];
        if (isMoving)
        {
            m_AudioSource.Play();
        }
        else { m_AudioSource.Stop(); }
    }
    private IEnumerator checkifMoving()
    {
        Vector3 temp = gameObject.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 nextpos = gameObject.transform.position;
        isMoving = temp == nextpos ? true : false;

    }
    // Update is called once per frame
    void Update () {
        gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * .1f, 0, Input.GetAxis("Vertical") *.1f);
        PlayFootStepAudio();
	}
}

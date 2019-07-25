using UnityEngine;
using System.Collections;

public class Train_PA : MonoBehaviour {

	//If player enters the train, station PA's volume is lowered and announcement is made inside the train. It also parents player's transform to train. If player exits the train, it goes back to defaults.

	public AudioClip announcement;
	public AudioSource ambient;

	void OnTriggerEnter(Collider other) {
		ambient.volume = 0.1f;
		AudioSource.PlayClipAtPoint(announcement, other.transform.position);
		other.transform.SetParent(this.transform);
	}

	void OnTriggerExit(Collider other) {
		ambient.volume = 1;
		other.transform.SetParent(null);
	}
}

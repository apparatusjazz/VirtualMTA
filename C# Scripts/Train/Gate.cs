using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	//This script uses a trigger zone to detect entry from either directions, plays audio and disables collider to let player pass through.
	//Good base to get started with rotating the children (pivots) with Quaternion.Angle and check if player has valid ticket or not, etc.

	public AudioClip gateSound;
	public Transform[] gates;
	private BoxCollider[] boxColliders;

	void Start() {
		if (gates.Length < 1) {
			Debug.Log ("Gate has no rotating parts assigned in inspector. Ignoring this gate");
			Destroy (this);
		} else {
			boxColliders = new BoxCollider[gates.Length];
			for (int i = 0; i < gates.Length; i++) {
				boxColliders [i] = gates [i].GetComponentInChildren<BoxCollider> ();
			}
		}
	}

	void OnTriggerEnter() {
		StartCoroutine (openGate ());		
	}

	IEnumerator openGate(){
		AudioSource.PlayClipAtPoint (gateSound, this.transform.position);
		foreach (BoxCollider bc in boxColliders) {
			bc.enabled = false;
		}
		yield return new WaitForSeconds (3f);
		foreach (BoxCollider bc in boxColliders) {
			bc.enabled = true;
		}
	}
}

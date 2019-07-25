using UnityEngine;
using System.Collections;

public class ServiceDoor : MonoBehaviour {

	//Simple door opening script which looks for 'player' tagged gameobjects. It constantly checks distance between this and player's transform (not raycast based door recognition.)
	//If you set the door to static, the script will automatically remove itself and won't attempt to function.

	public float doorOpenAngle = 90.0f;
	public float doorClosedAngle = 0.0f;
	public float speed = 10.0f;

	public AudioClip openSound;
	public AudioClip closeSound;

	Quaternion doorOpen = Quaternion.identity;
	Quaternion doorClosed = Quaternion.identity;

	public Font font;

	bool doorStatus = false;
	bool playerInRange;

	Transform player;

	void Awake() {
		if (this.gameObject.isStatic) {
			Destroy (this);
		}
		doorOpen = Quaternion.Euler (0, doorOpenAngle, 0);
		doorClosed = Quaternion.Euler (0, doorClosedAngle, 0);
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
		}
		else {
			Debug.Log ("Servicedoor.cs couldn't find 'Player' tag in scene. Ignoring class..");
			Destroy (this);
		}
	}

	void Update() {
		if (Vector3.Distance (player.position, this.transform.position) < 3f) {
			playerInRange = true;
			if (Input.GetKeyDown (KeyCode.E)) {
				if (doorStatus) {
					StartCoroutine (this.moveDoor (doorClosed));
					if (closeSound != null) 					AudioSource.PlayClipAtPoint (closeSound, this.transform.position);
				} else {
					StartCoroutine (this.moveDoor (doorOpen));
					if (openSound != null) 					AudioSource.PlayClipAtPoint (openSound, this.transform.position);
				}
			}
		} else {
			playerInRange = false;
		}
	}

	IEnumerator moveDoor(Quaternion target) {
		while (Quaternion.Angle (transform.localRotation, target) > 0.5f) {
			transform.localRotation = Quaternion.Slerp (transform.localRotation, target, Time.deltaTime * speed);
			yield return null;
		}

		doorStatus = !doorStatus;
		yield return null;
	
	}

	void OnGUI() {
		if (playerInRange) {
			string message;
			if (!doorStatus) {
				message = "Press E to open";
			} else {
				message = "Press E to close";
			}
			GUIStyle style = new GUIStyle ();
			style.font = font;
			style.fontSize = 35;
			style.normal.textColor = Color.white;
			Rect rect = new Rect (Screen.width / 2 - 50, Screen.height / 2 - 22.5f, 200, 45);
			GUI.Label (rect, message, style);
		}
	}



}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class ParkedRailcar_Interaction : MonoBehaviour {

	//Used at the train showcase station on a dummy gameobject that has a trigger box collider.
	//It takes in an array of gameobjects (the trains), reparents them to itself, so later it can find Rightdoor and LeftDoor classes with GetComponentsInChildren.
	//When player is in trigger zone, it opens/closes the doors that have RightDoor and LeftDoor scripts assigned to them.
	//The method is exactly the same as on the animated train, movement happens with hardcoded 0.8f offset on Z axis, and doors get moved via Vector3.Slerp.

	public GameObject[] railCars;
	public float speed = 1.2f;
	public Font font;

	RightDoor[] rightDoors;
	LeftDoor[] leftDoors;
	bool opened = false;
	bool isMoving = false;

	public AudioClip doorSlideOpen;
	public AudioClip doorSlideClose;
	Vector3 playerPosition;
	bool inZone = false;

	void Start () {
		if (railCars.Length == 0) {
			Debug.LogError ("Parked Railcar class doesn't have any trains assigned in the inspector!");
			Destroy (this);
		}
		if (font == null) {
			Debug.Log ("Font not assigned for ParkedRailcar interactions class. UI message will not display properly.");
		}
		foreach (GameObject go in railCars) {
			go.transform.SetParent (this.transform);
		}
		rightDoors = GetComponentsInChildren<RightDoor> ();
		leftDoors = GetComponentsInChildren<LeftDoor> ();
	}

	void Update(){
		if (isMoving) {
			MoveDoors ();
		}
	}

	private void SetDoorVector(bool toOpen) {
		if (toOpen) {
			for (int i = 0; i < rightDoors.Length; i++) {
				rightDoors [i].SetDoorVector(0.8f);
				leftDoors [i].SetDoorVector(0.8f);
			}
		} else {
			for (int i = 0; i < rightDoors.Length; i++) {
				rightDoors [i].SetDoorVector(-0.8f);
				leftDoors [i].SetDoorVector(-0.8f);
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.CompareTag ("Player")) {
			inZone = true;
			if (Input.GetKeyDown(KeyCode.E) && !isMoving) {
				SetDoorVector (!opened);
				isMoving = !isMoving;
				StartCoroutine (snapDoorsInState ());
				playerPosition = other.transform.position;
				PlayAudio ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag ("Player")) {
			inZone = false;
		}
	}

	private void MoveDoors(){
		foreach (RightDoor r in rightDoors) {
			r.transform.position = Vector3.Slerp (r.transform.position, r.targetValue, Time.deltaTime * speed);
		}
		foreach (LeftDoor l in leftDoors) {
			l.transform.position = Vector3.Slerp (l.transform.position, l.targetValue, Time.deltaTime * speed);
		}
	}

	private void PlayAudio(){
		if (opened) {
			AudioSource.PlayClipAtPoint (doorSlideClose,playerPosition);
		} else {
			AudioSource.PlayClipAtPoint (doorSlideOpen, playerPosition);
		}
	}

	IEnumerator snapDoorsInState(){
		yield return new WaitForSeconds (2f);
		isMoving = false;
		opened = !opened;
		foreach (RightDoor r in rightDoors) {
			r.transform.position = r.targetValue;
		}
		foreach (LeftDoor l in leftDoors) {
			l.transform.position = l.targetValue;
		}

	}

	void OnGUI(){
		if (inZone) {
			GUIStyle style = new GUIStyle ();
			style.font = font;
			style.fontSize = 35;
			style.normal.textColor = Color.white;
			Rect rect = new Rect (Screen.width / 2 - 50, Screen.height / 2 - 22.5f, 200, 45);
			GUI.Label (rect, "Press E to open/close railcar", style);
		}

	}
	

}

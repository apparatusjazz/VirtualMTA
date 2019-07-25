using UnityEngine;
using System.Collections;

public class VendingMachine : MonoBehaviour {

	//Drops a soda from the machine when player presses the button by instantiating an assigned soda can prefab. Extend this or make an array for different sodas, etc.

	public GameObject sodaPrefab;
	public AudioClip vendingMachine;
	public Font font;
	bool inZone;

	void Start () {
		if (sodaPrefab == null || vendingMachine == null || font == null) {
			Debug.Log ("Soda machine instance has variables not assigned in the inspector. This soda machine will not work..");
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.CompareTag ("Player")) {
			inZone = true;
			if (Input.GetKeyDown (KeyCode.E)) {
				AudioSource.PlayClipAtPoint (vendingMachine, this.transform.position);
				GameObject go = (GameObject)Instantiate (sodaPrefab);
				go.transform.SetParent (this.transform);
				go.transform.localPosition = new Vector3 (0.2f, 0.55f, 0.3f);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag ("Player")) {
			inZone = false;
		}
	}

	void OnGUI(){
		if (inZone) {
			GUIStyle style = new GUIStyle ();
			style.font = font;
			style.fontSize = 35;
			style.normal.textColor = Color.white;
			Rect rect = new Rect (Screen.width / 2 - 50, Screen.height / 2 - 22.5f, 200, 45);
			GUI.Label (rect, "Press E for a pop", style);
		}

	}
}

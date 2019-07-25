using UnityEngine;
using System.Collections;

public class TicketMachine : MonoBehaviour {

	//Plays audio on keypress if player's at the machine.

	public AudioClip ticketing;
	public Font font;

	bool inZone;

	void OnTriggerStay(Collider o) {
		if (o.CompareTag("Player")) {
			inZone = true;
			if (Input.GetKeyDown(KeyCode.E)) {
				AudioSource.PlayClipAtPoint(ticketing, this.transform.position);
			}
		}
	}

	void OnTriggerExit() {
		inZone = false;
	}

	void OnGUI(){
		if (inZone) {
			GUIStyle style = new GUIStyle ();
			style.font = font;
			style.fontSize = 35;
			style.normal.textColor = Color.white;
			Rect rect = new Rect (Screen.width / 2 - 50, Screen.height / 2 - 22.5f, 200, 45);
			GUI.Label (rect, "Press E to interact", style);
		}
	}
}

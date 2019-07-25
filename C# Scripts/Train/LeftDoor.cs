using UnityEngine;
using System.Collections;

public class LeftDoor : MonoBehaviour {

	//Identifies left door. target value refers to the Z axis offset when opening/closing the railcar's door.

	[HideInInspector]
	public Vector3 targetValue;
		
	public void SetDoorVector(float amt) {
		targetValue = new Vector3(transform.position.x, transform.position.y, transform.position.z - amt);
	}
}

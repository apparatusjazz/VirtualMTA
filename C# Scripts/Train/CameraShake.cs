using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour {

	//Shakes the camera when inside the train.
			
	//Shake multiplier
	public float shakeAmount = 0.7f;
		
	Vector3 originalPos;

	void OnEnable()	{
		originalPos = this.transform.localPosition;
	}
	
	void Update() {
		this.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
	}
}
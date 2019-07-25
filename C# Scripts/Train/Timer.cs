using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	//This class launches the train and handles timing. It is currently set to match the provided demo audio clips, and is intended for demonstration purposes only.

	public Image bar;
	public Text text;
	public float delay;
	private Train1 train;
    public bool new_station = false;

	bool starting = true;
	bool isSet = false;

	void Start() {
		train = FindObjectOfType<Train1> ();
		Camera c = FindObjectOfType<Camera>();
		if (c != null) {
			c.allowHDR = true;
			c.allowMSAA = false;
		}
        if (new_station)
        {
            delay = 0f;
        }
	}

	void FixedUpdate() {
		if (starting) {
			if (!isSet) {
				bar.fillAmount = 1f;
				isSet = true;
			}
			bar.fillAmount -= 1.0f / delay * Time.deltaTime;
			text.text = "Train arrival time";
			if (bar.fillAmount <= 0.01f) {
				train.TrainLaunch ();
				starting = false;
				bar.fillAmount = 0.0f;
				bar.color = Color.yellow;
				text.text = "Train arriving shortly";
				isSet = false;
                delay = 30f;
			}
		} else {
			if (train.trainStopped) {
				text.text = "Leaving station";
				if (!isSet) {
					bar.color = Color.yellow;
					bar.fillAmount = 1f;
					StartCoroutine (InsideTrainAnnouncement ());
					isSet = true;
				}
				bar.fillAmount -= 1.0f / 20 * Time.deltaTime;
				if (bar.fillAmount <= 0.01f) {
					bar.fillAmount = 0f;
					train.TrainDepart ();
					train.trainStopped = false;
					isSet = false;
					starting = true;
				}

			}
		}
	}

	IEnumerator InsideTrainAnnouncement(){
		yield return new WaitForSeconds (6f);
		train.MakeAnnouncementInsideTrain ();
	}
}

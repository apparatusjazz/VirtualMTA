using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class FitbitHR : MonoBehaviour {

	//This class launches the train and handles timing. It is currently set to match the provided demo audio clips, and is intended for demonstration purposes only.

	public Image bar;
	public Text text;

    private double test;
    private int counter = 0;


    void CreateFile()
    {
        string path = Application.dataPath + "/time.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Time \n");
        }
        System.DateTime current = System.DateTime.Now;
        string content = current.ToString("HH:mm:ss") + "\n";

        File.AppendAllText(path, content);
    }


    void Start() {
        test = 90;
    }

	void FixedUpdate() {
        
        counter++;
        if (counter == 50)
        {

            test = UnityHttpListener.heartRate;
            //test = Random.Range(test-5f, test+5f);
            //if (test < 75)
             //   test = 80;
            //if (test > 130)
             //   test = 120;
            double test2 = test;
            double test3 = test2 / 150;
            text.text = ""+test2;

            //bar.fillAmount = test3;
            bar.color = Color.green;
            text.color = Color.green;
            

            if (test > 120f)
            {
                bar.color = Color.red;
                text.color = Color.red;
            }
            else if(test <= 120 & test > 100){
                bar.color = Color.yellow;
                text.color = Color.yellow;
            }
            counter = 0;

        }
    }

    private void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                RecordTime();
                CreateFile();
            }
        }
    }

    void RecordTime()
    {
            System.DateTime current = System.DateTime.Now;
            Debug.Log(current.ToString("HH:mm:ss"));
    }

}

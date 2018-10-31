using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Attach to timer UI object

public class MainTimer : MonoBehaviour
{
    // text object to modify
    private Text timerText;
    private float raceTime;

    public string timerStatement = "TIME: ";

	// Use this for initialization
	void Start ()
    {
        timerText = GetComponent<Text>();
        raceTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        raceTime += Time.deltaTime;
        timerText.text = timerStatement + raceTime;
	}
}

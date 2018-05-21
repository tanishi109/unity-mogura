using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

	public float TimerSeconds;
	private Text displayTime;

	// Use this for initialization
	void Start ()
	{
		displayTime = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		TimerSeconds -= Time.deltaTime;
		var seconds = (int) TimerSeconds;
		displayTime.text = seconds.ToString();

		if (seconds <= 0)
		{
			SceneManager.LoadScene(2);
		}
	}
}

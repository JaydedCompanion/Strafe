using UnityEngine;
using System.Collections;

public class Pauser : MonoBehaviour {

	public bool Paused;

	private float UntouchedTimeScale;

	// Use this for initialization
	void Start () {
		
		UntouchedTimeScale = Time.timeScale;
		
	}
	
	// Update is called once per frame

	void Update () {

		//Call the TogglePause function each time the ESC key is pressed down.

		if (Input.GetButtonDown("Cancel")) {

			Debug.Log ("ESC Detected");

			TogglePause();

		}

		if (Paused) {

			Time.timeScale = 0;

		} else {

			Time.timeScale = UntouchedTimeScale;

		}

		if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Pauser>().Paused == false){
			
			
			
		}
	
	}

	//The actual toggling is put in a public funtion, so that it can easily be called from other scripts and UI elements.

	public void TogglePause() {

		Paused = !Paused;

	}

}

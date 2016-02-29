using UnityEngine;
using System.Collections;

public class Pauser : MonoBehaviour {

	public bool Paused;

	// Use this for initialization
	void Start () {
		

		
	}
	
	// Update is called once per frame

	void Update () {

		if (Input.GetButton("Cancel")) {

			TogglePause();

		}

		if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Pauser>().Paused == false){
			
			
			
		}
	
	}

	public void TogglePause() {

		Paused = !Paused;

	}

}

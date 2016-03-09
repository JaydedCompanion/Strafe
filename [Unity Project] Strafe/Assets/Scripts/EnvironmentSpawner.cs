using UnityEngine;
using System.Collections;

public class EnvironmentSpawner : MonoBehaviour {

	/*
	 * The plan so far, is to detect the camera's position and base everything off of that. 
	 * When the camera is coming close to the "edge of the world", have it generate another instance of ground
	 * When a previous instance of the ground is far enough from the camera, delete it.
	 * Have an array of objects 
	 */

	public GameObject[] Objects;

	public float CameraLife;

	public float CameraViewStart;
	public float CameraViewEnd;

	// Use this for initialization
	void Start () {
		

		
	}
	
	// Update is called once per frame

	void Update () {

		CameraLife = GameObject.FindGameObjectWithTag("MainCamera").transform.position.z;

		//Draw red lines where the camera's sight ends.

		Debug.DrawRay(new Vector3(100, 0, CameraLife - CameraViewStart), Vector3.left*200, Color.blue);
		Debug.DrawRay(new Vector3(100, 0, CameraLife + CameraViewEnd), Vector3.left*200, Color.blue);

		if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Pauser>().Paused == false){
			
			
			
		}
	
	}

}

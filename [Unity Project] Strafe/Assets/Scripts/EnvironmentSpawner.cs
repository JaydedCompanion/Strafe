using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentSpawner : MonoBehaviour {

	/*
	 * The plan so far, is to detect the camera's position and base everything off of that. 
	 * When the camera is coming close to the "edge of the world", have it generate another instance of ground
	 * When a previous instance of the ground is far enough from the camera, delete it.
	 * Have an array of objects 
	 */

	public float Frequency;
	public float RandRange;
	public float XRange;
	public float LastZCheckpoint;
	public float ZOffset;

	public List <GameObject> Objects = new List<GameObject>();
	private GameObject instObject;

	public float CameraLife;

	public float LocalCameraViewStart;
	public float LocalCameraViewEnd;

	public float CameraViewStart;
	public float CameraViewEnd;

	// Use this for initialization
	void Start () {
		

		
	}
	
	// Update is called once per frame

	void Update () {

		CameraLife = GameObject.FindGameObjectWithTag("MainCamera").transform.position.z;

		//Update the start and end camera floats with the offset of the camera's real position;

		CameraViewStart = LocalCameraViewStart + CameraLife;
		CameraViewEnd = LocalCameraViewEnd + CameraLife;

		//Draw red lines where the camera's sight ends.

		Debug.DrawRay(new Vector3(100, 0, CameraViewStart), Vector3.left*200, Color.blue);
		Debug.DrawRay(new Vector3(100, 0, CameraViewEnd), Vector3.left*200, Color.blue);

		//Detect if there is no decoration on the scene
		if (instObject == null) {

			UpdateDecor();

		}

		if (LastZCheckpoint <= CameraViewEnd) {

			UpdateDecor();

		}

		if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Pauser>().Paused == false){
			
			
			
		}
	
	}

	public void UpdateDecor () {

		float TargetZPos = CameraViewEnd;
		float TargetXPos = -XRange;

		while (TargetXPos <= XRange) {

			instObject = Instantiate (Objects[Random.Range(0, Objects.Capacity)],
				
				new Vector3 (
					Random.Range(TargetXPos - RandRange, TargetXPos + RandRange), 
					0, 
					Random.Range(TargetZPos - RandRange + ZOffset, TargetZPos + RandRange + ZOffset)
				), 
				Quaternion.Euler(Vector3.zero)

			) as GameObject;

			TargetXPos+=Frequency ;

		}

		LastZCheckpoint = CameraViewEnd + Frequency;

	}

}

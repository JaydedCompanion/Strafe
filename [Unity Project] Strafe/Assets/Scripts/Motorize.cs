/*
 * 
 * Apply this script to an object that you want to be rotated by an axis.
 * The main use of this script if for the porpeller on the fron to the plane
 * 
 */

using UnityEngine;
using System.Collections;

public enum Dir {

	Clockwise, CounterClockwise

}

public class Motorize : MonoBehaviour {

	public Transform Trans;
	public Dir Direction;
	public Vector3 Axis;
	public float Speed;

	// Use this for initialization
	void Start () {
		
		if (Trans == null) {

			Trans = transform;

		}
		
	}
	
	// Update is called once per frame

	void Update () {

		if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Pauser>().Paused == false){

			if (Direction == Dir.Clockwise) {

				Trans.Rotate(Axis, Speed * (Time.deltaTime * 35));

			}

			if (Direction == Dir.CounterClockwise) {

				Trans.transform.Rotate(Axis, -Speed * (Time.deltaTime * 35));

			}
			
		}
	
	}

}

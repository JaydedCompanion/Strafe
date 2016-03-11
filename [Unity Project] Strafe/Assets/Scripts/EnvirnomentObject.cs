using UnityEngine;
using System.Collections;

public class EnvirnomentObject : MonoBehaviour {

	public Vector2 SizeRange = Vector2.one;
	public Vector2 RotationRange = Vector2.one;

	private bool RandomizeSize;
	private bool RandomizeRot;

	// Use this for initialization
	void Start () {

		if (SizeRange != Vector2.one){

			RandomizeSize = true;

		}

		if (RotationRange != Vector2.one){

			RandomizeRot = true;

		}

		if (RandomizeRot) {

			transform.rotation = Quaternion.Euler(transform.rotation.x, Random.Range(RotationRange.x, RotationRange.y), transform.rotation.z);

		}

		if (RandomizeSize) {

			float tmp_ScaleFac = Random.Range(SizeRange.x, SizeRange.y);

			transform.localScale = new Vector3 (tmp_ScaleFac, tmp_ScaleFac, tmp_ScaleFac);

		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

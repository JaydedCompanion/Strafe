using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

	public string TargetTagName;
	public bool HandleGroundedAnim;
	public bool isActive;

	private Transform TargetTrans;
	private FlightState PlayerState;
	public float MixFac;

	// Use this for initialization
	void Start () {

		try {

			TargetTrans = GameObject.FindGameObjectWithTag(TargetTagName).transform;
		
		} catch {

			Debug.LogError("No such tag, or gameObject with that tag exists: " + TargetTagName);

		}
		
	}
	
	// Update is called once per frame

	void FixedUpdate () {

		if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Pauser>().Paused == false){
			
			if (isActive) {

				transform.position = new Vector3 (transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, TargetTrans.position.z, 0.25f));

				if (HandleGroundedAnim) {

					PlayerState = TargetTrans.gameObject.GetComponent<PlaneControl>().State;

					if (PlayerState == FlightState.Starting || PlayerState == FlightState.Idle) {

						MixFac = TargetTrans.position.y/TargetTrans.GetComponent<PlaneControl>().MaxHeight;

					} else if (PlayerState == FlightState.Game) {

						MixFac = 1;

						GetComponentInChildren<Animator>().SetBool("isFlying", true);

					}

					GetComponentInChildren<Animator>().SetFloat("MixFac", MixFac);

				}

			}
			
		}
	
	}

}

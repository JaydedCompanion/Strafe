using UnityEngine;
using System.Collections;

public enum FlightState {

	////--------------------/FLIGHT STATE DEFINITIONS:/--------------------////

	//"Idle" is as the game starts, before the player does anything

	//"Starting" is before the player reaches top speed & height. 
	//If the player touches the ground before moving onto "Game", they will simply land the plane,, and not crash

	//"Game" is during the main game. If the plane touches the ground at this state, it will crash.
	//The plane will also maintain a constant speed along the X axis (forward/backward) during this state

	//"Crash" will stop the player from controlling the plane, and will show the "Game Over" prompt

	Idle, Starting, Game, Crash

}

public class PlaneControl : MonoBehaviour {

	public bool DebugMaxHeight;
	public bool DebugMaxSteer;

	public FlightState State;

	public bool useTouch;
	public bool Powered;
	[Range (-1, 1)] public float Steer;

	[Range (10, 1000)] public float PullUpPower;
	[Range (1, 100)] public float TakeOffSpeed;
	[Range (1, 100)] public float Speed;
	[Range (1, 100)] public float SteerPower;
	[Range (1, 5)] public int ZToYInfluence;

	public float MaxHeight;
	public float MinHeight;
	public float LateralMax;

	public Motorize Porpeller;

	private bool DrawControlUI;
	private Rigidbody RB;

	// Use this for initialization
	void Start () {

		RB = gameObject.GetComponent<Rigidbody>();

		//Detect mobile platforms and assign the useTouch var appropiately

		if (Application.isMobilePlatform) {

			useTouch = true;

			DrawControlUI = true;

		} else {

			useTouch = false;

			DrawControlUI = false;

		}
	
	}

	void Update () {

		//Debug selected options

		if (DebugMaxHeight) {

			Debug.DrawLine (new Vector3 (5, MaxHeight, transform.position.z), new Vector3 (-5, MaxHeight, transform.position.z), Color.green);

		}

		if (DebugMaxSteer) {

			Debug.DrawLine (new Vector3 (LateralMax, transform.position.y, transform.position.z - 1), 
				new Vector3 (LateralMax, transform.position.y, transform.position.z + 1), Color.red);
			
			Debug.DrawLine (new Vector3 (LateralMax, 1, transform.position.z - 5), new Vector3 (LateralMax, 1, transform.position.z + 5), Color.red);

			Debug.DrawLine (new Vector3 (-LateralMax, transform.position.y, transform.position.z - 1), 
				new Vector3 (-LateralMax, transform.position.y, transform.position.z + 1), Color.red);
			
			Debug.DrawLine (new Vector3 (-LateralMax, 1, transform.position.z - 5), new Vector3 (-LateralMax, 1, transform.position.z + 5), Color.red);

		}

		//Begin Detecting Controls

		if (useTouch == true){

			if (Input.touchCount >= 1) {
				
				Powered = true;

			} else {

				Powered = false;

			}

			Steer = Mathf.Clamp (Input.gyro.attitude.z * 5, -1, 1);

		} else {

			if (Input.GetKeyDown(KeyCode.Space)){

				Powered = true;

			} else if (Input.GetKeyUp(KeyCode.Space)){
				 	
				Powered = false;

			}

			Steer = Input.GetAxis("Horizontal");

		}

		if (transform.position.x >= LateralMax) {

			Steer = Mathf.Clamp(Steer, -1, LateralMax - transform.position.x);

		} else if (transform.position.x <= -LateralMax) {
			
			Steer = Mathf.Clamp(Steer, -LateralMax - transform.position.x, 1);

		}
		//Finish Detecting Controls

		//--------------------//--------------------//

		//Begin Handling States. Look at the FlighState enumerator for info

		if (State == FlightState.Idle && Powered) {

			State = FlightState.Starting;

		}

		if (State == FlightState.Starting){

			if (transform.position.y >= MaxHeight){

				State = FlightState.Game;

			}

			if (transform.position.y >= MinHeight && !Powered){
				
				State = FlightState.Idle;

			}

		}

		if (State == FlightState.Game) {

			if (transform.position.y <= MinHeight) {

				State = FlightState.Crash;

			}

		}

		//Most action-related expressions will be found below

		if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Pauser>().Paused == false){

			//Use "Power" as Z (forward) velocity, which then gets converted to Y force, to add a more realistic take-off speed

			if (Powered && State == FlightState.Starting || Powered && State == FlightState.Idle) {

				//Add forwards force
				RB.AddForce (transform.forward * TakeOffSpeed * (Time.deltaTime*35));

				//Convert Z-Speed to Elevation force (Y Axis).
				RB.AddForce (Vector3.up * RB.velocity.z * ((MaxHeight + 5) - transform.position.y) * ZToYInfluence);

			}

			//Use "Power" as Y power AKA normal control, as well as a constant forwards speed

			if (State == FlightState.Game){

				//Maintain the plane at a constant Z-Speed. Lerp the value for a smooth transition in case the previous speed (from taking off) is dramatically different

				RB.velocity = new Vector3 (RB.velocity.x, RB.velocity.y, Mathf.Lerp (RB.velocity.z, Speed, 0.1f));

				if (Powered){

					//Add pullup force downwards (Change variable name in the future)
					RB.AddForce(-Vector3.up * PullUpPower * (Time.deltaTime*35));

				} else {

					//Bring the plane up to MaxHeight if the player isn't powering it

					if (transform.position.y < MaxHeight){
						RB.AddForce(Vector3.up * PullUpPower * (MaxHeight - transform.position.y) * 0.7f);
					}

				}

				//Use the "Steer" var to tell the plane what direction to go in (and animate accordingly)

				RB.AddForce(Vector3.right * Steer * SteerPower);

				int DCount = 0;

				while (DCount < transform.GetChild(0).childCount) {

					if (transform.GetChild(0).GetChild(DCount).name == "Z Pivot") {

						transform.GetChild(0).GetChild(DCount).GetComponent<Animator>().SetFloat("Steer", RB.velocity.x/5);

					}

					DCount++;

				}

			}

			if (State == FlightState.Crash) {

				//Stop the plane completely if it crashes, and disable the RigidBody component (make it kinematic);

				RB.velocity = Vector3.zero;
				RB.isKinematic = true;

			}

			//Set the speed of the porpeller to the current Z (forward) velocity

			Porpeller.Speed = RB.velocity.z * 10;

			//Make the plane look in the diraction that it is currently flying (upwards > 0º, downwards < 0º), which gives a much nicer flight effect, and will provide the gun aim

			if (State != FlightState.Crash && (RB.velocity.y + RB.velocity.z) > 0.1f) {

				int CCount = 0;

				while (CCount < transform.childCount) {

					if (transform.GetChild(CCount).name == "X Pivot") {

						transform.GetChild(CCount).rotation =  Quaternion.Euler(-RB.velocity.y * 3, 0, 0);

					}

					CCount++;

				}

			}

		}
	
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class Dot_Truck : System.Object
{
	public WheelCollider leftWheel;
	public GameObject leftWheelMesh;
	public WheelCollider rightWheel;
	public GameObject rightWheelMesh;
	public bool motor;
	public bool steering;
	public bool reverseTurn; 
	
}

public class Dot_Truck_Controller : MonoBehaviour {

	public float maxSteeringAngle;
	
	public float idleRPM = 800;
	public float maxRPM;
	
	public float breakpower;
	public float maxrotmomentum;
	public float horse_power;
	public float rear_axle_coeff;
	public float reverse_trans_coeff;
	public int gearsCount;
	public float gear_trans_coeff1;
	public float gear_trans_coeff2;
	public float gear_trans_coeff3;
	public float gear_trans_coeff4;
	public float gear_trans_coeff5;
	public float gear_trans_coeff6;
	public float gear_trans_coeff7;
	public float gear_trans_coeff8;
	public float gear_trans_coeff9;
	public float gear_trans_coeff10;
	public float gear_trans_coeff11;
	public float gear_trans_coeff12;
	public float gear_trans_coeff13;
	
	public List<Dot_Truck> truck_Infos;

	public void VisualizeWheel(Dot_Truck wheelPair)
	{
		Quaternion rot;
		Vector3 pos;
		wheelPair.leftWheel.GetWorldPose ( out pos, out rot);
		wheelPair.leftWheelMesh.transform.position = pos;
		wheelPair.leftWheelMesh.transform.rotation = rot;
		wheelPair.rightWheel.GetWorldPose ( out pos, out rot);
		wheelPair.rightWheelMesh.transform.position = pos;
		wheelPair.rightWheelMesh.transform.rotation = rot;
	}
	
	public void Update()
	{
		
		int currentGearNum = 0;
		float currentGearCoeff = 0;
		float motorTorque = 0;
		float steering = 0;
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			motorTorque = maxrotmomentum + horse_power;
			//motorTorque = maxrotmomentum + horse_power * Input.GetAxis("Vertical");
		}
		else
		{
			motorTorque = 0;
		}
		
		if (Input.GetKey(KeyCode.LeftControl))
		{
			steering = (maxSteeringAngle+10) * Input.GetAxis("Horizontal");
		}
		else
		{
			steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		}
		
		float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));
		if (brakeTorque > 0.001) {
			brakeTorque = maxrotmomentum + horse_power;
			motorTorque = 0;
		} else {
			brakeTorque = 0;
		}

		foreach (Dot_Truck truck_Info in truck_Infos)
		{
			if (truck_Info.steering == true) {
				truck_Info.leftWheel.steerAngle = truck_Info.rightWheel.steerAngle = ((truck_Info.reverseTurn)?-1:1)*steering;
			}

			if (truck_Info.motor == true)
			{
				truck_Info.leftWheel.motorTorque = motorTorque;
				truck_Info.rightWheel.motorTorque = motorTorque;
			}

			truck_Info.leftWheel.brakeTorque = brakeTorque;
			truck_Info.rightWheel.brakeTorque = brakeTorque;

			VisualizeWheel(truck_Info);
		}

	}

/*List<GameObject> Rooms = new List<GameObject>();*/


	public void OnTriggerEnter (Collider collider)
	{
		
		//Rooms = GameObject.Find("ap.b3d").GetComponent<B3DScript>().Rooms;
		//Debug.Log(Rooms[0].name);
		if (collider.gameObject.GetComponent<LoadTrigger>())
		{
			CameraControl camera =  GameObject.Find("Main Camera").GetComponent<CameraControl>();
			string name = collider.gameObject.GetComponent<LoadTrigger>().roomName;
			camera.ChangeCurrentRoom(name);
		}
		else if (collider.gameObject.GetComponent<LODCustom>())
		{
			//Debug.Log("collider: "+collider,collider);
			collider.gameObject.GetComponent<LODCustom>().SwitchState(0);
		}

	}
	public void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.GetComponent<LODCustom>())
		{
			collider.gameObject.GetComponent<LODCustom>().SwitchState(1);
		}	
	}



}
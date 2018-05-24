using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyController : MonoBehaviour {
	public GameObject target;
	public float maxForce = 100;
	public float currentForce;
	public Vector3 direction;
	public Vector3 torqueForce = new Vector3();
	public float approachDistance = 500;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Arrive();
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * currentForce);
		rb.AddTorque(torqueForce);
	}

	void Seek ()
	{
		// get the vector from our current position to the target position
		Vector3 targetVector = target.transform.position - transform.position;
		// normalize to isolate direction
		targetVector.Normalize();

		// get the angle between the vector to target and the current forward vector
		float angle = Vector3.Angle(transform.forward, targetVector);

		// get the axis orthogonal to both vectors to determine plane of rotation
		Vector3 plane = Vector3.Cross(transform.forward, targetVector);

		// the rotational force should be in the plane of rotation and proportional to the angle between the vectors
		torqueForce = plane * angle;

		// scale the speed based on the size of the angle
		currentForce = maxForce / (angle / 180);
	}

	void Arrive()
	{
		// get the vector from our current position to the target position
		Vector3 targetVector = target.transform.position - transform.position;
		// get the distance to the target
		float distance = targetVector.magnitude;
		// normalize to isolate direction
		targetVector.Normalize();

		// get the angle between the target direction and the current forward direction
		float angle = Vector3.Angle(transform.forward, targetVector);

		// get the axis orthogonal to both vectors to determine plane of rotation
		Vector3 plane = Vector3.Cross(transform.forward, targetVector);

		// the rotational force should be in the plane of rotation and proportional to the angle between the vectors
		torqueForce = plane * angle;

		// the force should decrease if approaching the target
		if (distance < approachDistance)
		{
			currentForce = maxForce * (distance / approachDistance);
		}
		else
		{
			currentForce = maxForce;
		}

		// the force should decrease the greater the angle is
		currentForce = currentForce * (180 / (180 - angle));
	}

	void OnDrawGizmosSelected()
	{
		if (torqueForce != null)
		{
			Gizmos.color = Color.yellow;
			// draw the current forward vector
			Gizmos.DrawLine(transform.position, transform.position + (transform.forward*100));
			// draw the torque vector at the end of the forward vector
			Gizmos.DrawLine(transform.position + (transform.forward * 100), transform.position + (transform.forward * 100) + torqueForce);
		}
	}
}

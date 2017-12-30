using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

	private Rigidbody rb;
	public Vector2 startWait;
	private float targetManeuver;
	public float dodge;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	public float smoothing;
	public float tilt;
	private float currentSpeed;
	public Boundary boundary;
	private Transform palyerTransform;

	// Use this for initialization
	void Start () {
		rb = GetComponent <Rigidbody> ();
		StartCoroutine (Evade());
		currentSpeed = rb.velocity.z;
		palyerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	IEnumerator Evade() {
		yield return new WaitForSeconds (Random.Range(startWait.x, startWait.y));
		while (true) {
			if (palyerTransform != null) {
				targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (-palyerTransform.position.x);
			}
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}

	void FixedUpdate() {
		float newManeuver = Mathf.MoveTowards (rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
		Debug.Log (newManeuver);
		rb.velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);
		rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
	}

}

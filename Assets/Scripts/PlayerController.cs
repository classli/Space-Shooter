using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	public float speed;
	public Boundary boundary;
	public float title;
	public float fireRate;
	public GameObject shot;
	public Transform []shotSpawns;
	public TouchScreen touch;
	private float nextFire;
	private AudioSource audioS;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioS = GetComponent<AudioSource>();
	}

	void Update (){
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			foreach (var shotSpawn in shotSpawns) {
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			}
			audioS.Play ();
		}
	}

	void FixedUpdate(){
//		Vector2 direction = touch.GetDirection ();
//		if (!touch.shouldStopMove (rb.position) && (Mathf.Pow (direction.x, 2) + Mathf.Pow (direction.y, 2)) > 0.1f) {
//			Vector3 movement = new Vector3 (direction.x, 0.0f, direction.y);
//			float sp=touch.GetVelocity ();
//			rb.velocity = movement *sp;
//		} else {
//			rb.velocity = new Vector3(0.0f,0.0f,0.0f);
//		}
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -title);
	}
}

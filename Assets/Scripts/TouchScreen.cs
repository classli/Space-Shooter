using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchScreen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	private Vector2 origin;
	private Vector2 direction;
	public float smoothing;
	private Vector2 smoothDirection;
	private bool touched;
	private int pointerID;
	Vector2 directionRaw;
	Vector2 palyerStart;
	private Camera camera;
	private Vector2 lastPoint;
	private float velocity;
	private Vector2 playerDiff;
	private Transform palyerTransform;

	void Start()
	{
		camera = Camera.main; 
	}


	void Awake() {
		init ();
	}

	void init(){
		direction = Vector2.zero;
		directionRaw = Vector2.zero;
		touched = false;
		palyerStart = Vector2.zero; 
		velocity = 0;
	}

	public void OnPointerDown (PointerEventData data){
		if (!touched) {
			Vector3 p = new Vector3();
			p = camera.ScreenToWorldPoint (new Vector3 (data.position.x, data.position.y, camera.nearClipPlane));
			origin = new Vector2 (p.x, p.z);
			touched = true;
			pointerID = data.pointerId;
			palyerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
			palyerStart.x = palyerTransform.position.x;
			palyerStart.y = palyerTransform.position.z;
			playerDiff = origin - palyerStart;
		}
	}

	public void OnDrag (PointerEventData data){

		if (data.pointerId == pointerID) {
			Vector3 p = new Vector3();
			p = camera.ScreenToWorldPoint (new Vector3 (data.position.x, data.position.y, camera.nearClipPlane));
			Vector2 currentPosition = new Vector2 (p.x, p.z);
			directionRaw = currentPosition - origin;
			direction = directionRaw.normalized;
			lastPoint = currentPosition;
			Vector2 di = currentPosition - playerDiff;
			StartCoroutine(move (di));
		}
	}

	IEnumerator move(Vector2 df) {
		yield return new WaitForSeconds (0.06f);
		palyerTransform.position = new Vector3 (df.x, 0.0f, df.y);
	}

	public void OnPointerUp (PointerEventData data) {
		if (data.pointerId == pointerID) {
			init ();
		}
	}

	public Vector2 GetDirection(){
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}

	public float GetVelocity(){
		velocity = Mathf.Sqrt(Mathf.Pow(directionRaw.x,2f)+Mathf.Pow(directionRaw.y,2f));  
		return velocity;
	}

	public bool shouldStopMove(Vector3 nowPosition) {
		if ((Mathf.Pow ((nowPosition.x - palyerStart.x), 2) + Mathf.Pow ((nowPosition.z - palyerStart.y), 2)) > (Mathf.Pow ((directionRaw.x), 2) + Mathf.Pow ((directionRaw.y), 2))) {
			init ();
			origin = lastPoint;
			Transform palyerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
			palyerStart.x = palyerTransform.position.x;
			palyerStart.y = palyerTransform.position.z;
			return true;
		}
		return false;
	}
}

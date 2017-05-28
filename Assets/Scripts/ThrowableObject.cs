using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {

	public float maxStretch = 2.0f;

	private GameObject slingShot;
	private Rigidbody2D m_rigidBody;
	private SpringJoint2D m_springJoint;
	private CameraController m_Camera;
	private Ray rayToMouse;
	private Vector2 prevVelocity;
	private bool isClicked;
	private bool launching;
	private float sqrMaxStretch;

	void Awake () 
	{
		slingShot = GameObject.FindWithTag ("SlingShot");
		m_rigidBody = gameObject.GetComponent <Rigidbody2D> ();
		m_springJoint = gameObject.GetComponent <SpringJoint2D> ();
		GetCameraController ();
		isClicked = false;
		launching = false;
	}

	void Start()
	{
		rayToMouse = new Ray (slingShot.transform.position, Vector3.zero);
		sqrMaxStretch = Mathf.Pow (maxStretch, 2);
	}

	void Update () 
	{
		if(isClicked)
		{
			Dragging ();
		}

		if(launching)
		{
			if(prevVelocity.sqrMagnitude > m_rigidBody.velocity.sqrMagnitude)
			{
				m_springJoint.enabled = false;
				m_rigidBody.velocity = prevVelocity;
				launching = false;
			}
			prevVelocity = m_rigidBody.velocity;
		}
	}

	void OnMouseDown()
	{
		m_rigidBody.isKinematic = true;
		isClicked = true;
		m_Camera.canMove = false;
	}

	void OnMouseUp()
	{
		m_rigidBody.isKinematic = false;
		isClicked = false;
		launching = true;
		m_Camera.canMove = true;
	}

	private void GetCameraController()
	{
		GameObject cameraRig = GameObject.FindWithTag ("CameraRig");
		if(cameraRig==null)
		{
			Debug.LogError ("Unnable to find cameraRig");
		}
		m_Camera= cameraRig.GetComponent<CameraController>();
		if(m_Camera== null)
		{
			Debug.LogError ("Unnable to find CameraController");
		}
	}

	private void Dragging ()
	{
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 slingToMouse = mouseWorldPoint - slingShot.transform.position;

		if(slingToMouse.sqrMagnitude > sqrMaxStretch)
		{
			rayToMouse.direction = slingToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}

		mouseWorldPoint.z = 0;
		m_rigidBody.position = mouseWorldPoint;
	}
}

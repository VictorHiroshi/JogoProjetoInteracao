﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {

	public float maxStretch = 2.0f;
	public float timeFollowedByCamera = 2.5f;

	private GameObject slingShot;
	private Rigidbody2D m_rigidBody;
	private SpringJoint2D m_springJoint;
	private CameraController m_Camera;
	private Ray rayToMouse;
	private Vector2 prevVelocity;
	private float sqrMaxStretch;
	private bool isClicked;
	private bool launching;
	private bool cameraFollows;

	void Awake () 
	{
		slingShot = GameObject.FindWithTag ("SlingShot");
		m_rigidBody = gameObject.GetComponent <Rigidbody2D> ();
		m_springJoint = gameObject.GetComponent <SpringJoint2D> ();
		GetCameraController ();
		isClicked = false;
		launching = false;
		cameraFollows = false;
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
				cameraFollows = true;
			}
			prevVelocity = m_rigidBody.velocity;
		}

		if(cameraFollows)
		{
			StartCoroutine (SetCameraToFollow ());
			cameraFollows = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject trashCan = other.gameObject;

		if(trashCan.tag == "Reciclavel")
		{
			FallInRecyclableCan ();
		}
		else if (trashCan.tag == "Organicos")
		{
			FallInOrganicCan ();
		}
		else if (trashCan.tag == "LogisticaReversa")
		{
			FallInReverseLogisticCan ();
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

	private void FallInRecyclableCan ()
	{
		Debug.Log ("Lixeira reciclavel");
	}

	private void FallInOrganicCan ()
	{
		Debug.Log ("Lixeira de organicos");
	}

	private void FallInReverseLogisticCan ()
	{
		Debug.Log ("Lixeira de logistica reversa");
	}

	private IEnumerator SetCameraToFollow()
	{
		float remainingTime = timeFollowedByCamera;

		while (remainingTime > 0)
		{
			m_Camera.MoveToTarget (transform, false);
			remainingTime -= Time.deltaTime;
			yield return null;
		}

		m_Camera.MoveToTarget (slingShot.transform, true);
	}
}

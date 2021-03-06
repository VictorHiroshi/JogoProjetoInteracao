﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {

	public float maxStretch = 2.0f;
	public float timeToNextShot = 0.8f;
	public int points = 5;
	public string wrongTrashCanMessage;
	public string rightTrashCanMessage;

	private GameObject slingShot;
	private Rigidbody2D m_rigidBody;
	private SpringJoint2D m_springJoint;
	private Ray rayToMouse;
	private Vector2 prevVelocity;
	private float sqrMaxStretch;
	private bool isClicked;
	private bool launching;
	private bool followedByCamera;
	private bool fall;
	private bool showingMessage;
	private bool ignoreClick;

	void Awake () 
	{
		slingShot = GameObject.FindWithTag ("SlingShot");
		m_rigidBody = gameObject.GetComponent <Rigidbody2D> ();
		m_springJoint = gameObject.GetComponent <SpringJoint2D> ();
		isClicked = false;
		launching = false;
		followedByCamera = false;
		fall = false;
		showingMessage = false;
	}

	void Start()
	{
		rayToMouse = new Ray (slingShot.transform.position, Vector3.zero);
		sqrMaxStretch = Mathf.Pow (maxStretch, 2);
		ignoreClick = false;
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
				followedByCamera = true;
			}
			prevVelocity = m_rigidBody.velocity;
		}

		if(followedByCamera)
		{
			StartCoroutine (SetCameraToFollow ());
			followedByCamera = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(fall)
		{
			return;
		}

		fall = true;
		GameObject something = other.gameObject;

		ignoreClick = false;

		if(something.tag == tag)
		{
			FallInRightCan ();
		}
		else if(something.tag == "Ground")
		{
			FallOnTheGround ();
		}
		else
		{
			FallInWrongCan ();
		}
	}

	void OnMouseDown()
	{
		if(ignoreClick)
		{
			return;
		}

		m_rigidBody.isKinematic = true;
		isClicked = true;
		GameManager.instance.m_Camera.canMove = false;
	}

	void OnMouseUp()
	{
		if(ignoreClick)
		{
			return;
		}

		m_rigidBody.isKinematic = false;
		isClicked = false;
		launching = true;
		ignoreClick = true;
		GameManager.instance.m_Camera.canMove = true;
	}

	public void ReadMessageOnPanel()
	{
		showingMessage = false;
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

	private void FallInRightCan ()
	{
		GameManager.instance.AddPoints (points);
		if(rightTrashCanMessage != string.Empty)
		{
			showingMessage = true;
			StartCoroutine (ShowPostShotMessage (rightTrashCanMessage, PrepareNextShot ()));
		}
		StartCoroutine (PrepareNextShot ());

	}

	private void FallInWrongCan ()
	{
		showingMessage = true;
		GameManager.instance.AddPoints (GameManager.instance.pointsLostWrongCan);

		string message;

		if(wrongTrashCanMessage == string.Empty)
		{
			message = Phrases.wrongTrashCanMessage [Random.Range (0, Phrases.wrongTrashCanMessage.Length)];
		}
		else
		{
			message = wrongTrashCanMessage;
		}

		StartCoroutine (ShowPostShotMessage (message, PrepareNextShot ()));
	}

	private void FallOnTheGround()
	{
		showingMessage = true;
		GameManager.instance.AddPoints (GameManager.instance.pointsLostOnTheGround);
		string message = Phrases.trashOnTheGroundMessage [Random.Range (0, Phrases.trashOnTheGroundMessage.Length)];
		StartCoroutine (ShowPostShotMessage (message, PrepareReshot ()));
	}

	private IEnumerator SetCameraToFollow()
	{
		while (!fall)
		{
			GameManager.instance.m_Camera.MoveToTarget (transform, false);
			yield return null;
		}
	}

	private IEnumerator PrepareNextShot()
	{
		yield return new WaitForSeconds (timeToNextShot);
		GameManager.instance.SetToInstantiateNextTrash ();
		Destroy (gameObject);
	}

	private IEnumerator PrepareReshot()
	{
		yield return new WaitForSeconds (timeToNextShot);
		GameManager.instance.SetToInstantiateSameTrash ();
		Destroy (gameObject);
	}

	private IEnumerator ShowPostShotMessage(string message, IEnumerator routine)
	{
		GameManager.instance.ShowWrongCanMessage (message, "Ok", this);

		while(showingMessage)
		{
			yield return null;
		}

		StartCoroutine (routine);
	}
}

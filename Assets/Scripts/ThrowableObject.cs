using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {

	public float maxStretch = 2.0f;
	public float timeToNextShot = 0.8f;
	public int points = 5;

	private GameManager gameManager;
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

	void Awake () 
	{
		slingShot = GameObject.FindWithTag ("SlingShot");
		m_rigidBody = gameObject.GetComponent <Rigidbody2D> ();
		m_springJoint = gameObject.GetComponent <SpringJoint2D> ();
		isClicked = false;
		launching = false;
		followedByCamera = false;
		fall = false;
	}

	void Start()
	{
		rayToMouse = new Ray (slingShot.transform.position, Vector3.zero);
		sqrMaxStretch = Mathf.Pow (maxStretch, 2);
		gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent <GameManager> ();
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
		m_rigidBody.isKinematic = true;
		isClicked = true;
		GameManager.instance.m_Camera.canMove = false;
	}

	void OnMouseUp()
	{
		m_rigidBody.isKinematic = false;
		isClicked = false;
		launching = true;
		GameManager.instance.m_Camera.canMove = true;
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
		StartCoroutine (PrepareNextShot ());
		gameManager.AddPoints (points);
	}

	private void FallInWrongCan ()
	{
		StartCoroutine (PrepareNextShot ());
		Debug.Log ("WROOOOONG!");
	}

	private void FallOnTheGround()
	{
		StartCoroutine (PrepareReshot ());
		Debug.Log ("Don't throw it on the ground, fella! Try Again...");
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
		gameManager.SetToInstantiateNextTrash ();
		Destroy (gameObject);
	}

	private IEnumerator PrepareReshot()
	{
		yield return new WaitForSeconds (timeToNextShot);
		gameManager.SetToInstantiateSameTrash ();
		Destroy (gameObject);
	}
}

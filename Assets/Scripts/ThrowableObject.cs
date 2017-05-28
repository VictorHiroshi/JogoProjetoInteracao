using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {

	private Rigidbody2D m_rigidBody;
	private CameraController m_Camera;
	private bool isClicked;

	void Awake () {
		m_rigidBody = gameObject.GetComponent <Rigidbody2D> ();

		GetCameraController ();

		isClicked = false;
	}

	void Update () {
		if(isClicked)
		{
			
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
}

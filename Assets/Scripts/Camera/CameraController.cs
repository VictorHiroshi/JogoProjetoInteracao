using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;

	public void IncrementBoundary(float value)
	{
		xMin -= value*2;
		xMax += value*2;
		yMin -= value;
		yMax += value;
	}
}

public class CameraController : MonoBehaviour {

	public float dampTime = 0.2f;
	public float cameraVelocity = 0.08f;
	public float zoomFactor = 0.5f;
	public float maxOrtographicSize = 5.5f;
	public float minOrtographicSize = 3.0f;
	public Boundary cameraBoundaries;

	private Camera m_Camera;
	private Vector3 cameraOrigin;
	private Vector3 clickPoint;
	Vector3 moveVelocity;


	void Awake () {
		m_Camera = GetComponentInChildren<Camera> ();
	}

	void Update () 
	{
		MoveCamera (0);
	}

	// Move the camera to look at the specified target, maintaining the actual ratio.
	public void MoveToTarget (Transform target)
	{
		transform.position = Vector3.SmoothDamp (transform.position, target.position, ref moveVelocity, dampTime);
/*		transform.position = target.position;*/
	}

	public void ZoomIn()
	{
		if (m_Camera.orthographicSize > minOrtographicSize) 
		{
			m_Camera.orthographicSize -= zoomFactor;
			cameraBoundaries.IncrementBoundary (zoomFactor);
		}
	}

	public void ZoomOut()
	{
		if (m_Camera.orthographicSize < maxOrtographicSize) 
		{
			m_Camera.orthographicSize += zoomFactor;
			cameraBoundaries.IncrementBoundary (-zoomFactor);
		}
	}

	// Checks for movements of the mouse while pressing the given button.
	private void MoveCamera(int button)
	{
		if(Input.GetMouseButtonDown (button))
		{
			clickPoint = Input.mousePosition;
			cameraOrigin = transform.position;
		}

		else if(Input.GetMouseButton (button))
		{
			transform.position = cameraOrigin + (new Vector3((clickPoint.x - Input.mousePosition.x), 
																(clickPoint.y - Input.mousePosition.y), 
																(0f))*cameraVelocity);
			CheckBoundaries ();
		}
	}

	// Guarantees that the camera will remain within the boardgame space.
	private void CheckBoundaries ()
	{
		Vector3 correctedPosition = transform.position;

		if(transform.position.x < cameraBoundaries.xMin){
			correctedPosition.x = cameraBoundaries.xMin;
		}
		if(transform.position.x > cameraBoundaries.xMax){
			correctedPosition.x = cameraBoundaries.xMax;
		}
		if(transform.position.y < cameraBoundaries.yMin){
			correctedPosition.y = cameraBoundaries.yMin;
		}
		if(transform.position.y > cameraBoundaries.yMax){
			correctedPosition.y = cameraBoundaries.yMax;
		}

		transform.position = correctedPosition;
	}
}

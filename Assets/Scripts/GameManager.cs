﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int secondsToRestart = 3;
	public GameObject[] trashObjects;
	public int trashListSize = 10;
	public Transform spawnPoint;
	public GameObject slingShot;

	public static GameManager instance;

	[HideInInspector] public CameraController m_Camera;

	private int TrashIndex;
	private bool toInstantiateTrash;
	private int points;
	private GameObject trashInstance;
	private GameObject[] trashList;
	private HUDController hud;

	void Awake () {

		if(instance==null)
		{
			instance = this;
		}
		else
		{
			Destroy (this);
		}

		hud = gameObject.GetComponent <HUDController> ();
		GetCameraController ();
	}

	void Start()
	{
		if(spawnPoint == null)
		{
			Debug.LogError ("No spawn point informed!");
		}
		if(trashObjects.Length==0)
		{
			Debug.LogError ("No trash in the array!");
		}
		if(slingShot == null)
		{
			Debug.LogError ("No slingshot informed!");
		}

		Setup ();
	}

	void Update ()
	{
		if(TrashIndex == trashListSize)
		{
			TrashIndex = 0;
			toInstantiateTrash = false;
			GameOver ();
		}
		else if(toInstantiateTrash)
		{
			InstantiateTrash ();
			toInstantiateTrash = false;
		}
	}

	public void RestartGame()
	{
		StartCoroutine (SetupRestart ());
	}
		
	public void SetToInstantiateNextTrash()
	{
		TrashIndex ++;
		toInstantiateTrash = true;
	}

	public void SetToInstantiateSameTrash()
	{
		toInstantiateTrash = true;
	}

	public void AddPoints(int extraPoints)
	{
		points += extraPoints;
		hud.UpdatePunctuationText (points);
	}

	public void ShowWrongCanMessage(string message, string buttonText, ThrowableObject instance)
	{
		m_Camera.canMove = false;
		hud.ShowPanelMessage (message);
		StartCoroutine (hud.ShowPanelButton (buttonText, instance));
	}

	private void InstantiateTrash ()
	{
		GameObject toInstantiate = trashList [TrashIndex];
		trashInstance = Instantiate (toInstantiate, spawnPoint.position, spawnPoint.rotation);
		SpringJoint2D springJoint = trashInstance.GetComponent <SpringJoint2D> ();
		springJoint.connectedBody = slingShot.GetComponent <Rigidbody2D> ();

		m_Camera.MoveToTarget (trashInstance.transform, false);
	}

	private void GameOver()
	{
		m_Camera.canMove = false;
		hud.ShowPanelMessage ("Game Over!");
		StartCoroutine (hud.ShowPanelButton ("Recomeçar"));
	}

	private void GenerateRandomicTrashList ()
	{
		trashList = new GameObject[trashListSize];
		for(int i=0; i< trashListSize; i++)
		{
			trashList[i] = trashObjects [Random.Range (0, trashObjects.Length)];
		}
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

	private void Setup()
	{
		m_Camera.canMove = true;
		TrashIndex = 0;
		toInstantiateTrash = true;
		points = 0;
		hud.HidePanelMessage ();
		hud.UpdatePunctuationText (points);
		GenerateRandomicTrashList ();
	}

	private IEnumerator SetupRestart()
	{
		for(int i =0; i<secondsToRestart; i++)
		{
			hud.ShowPanelMessage ("Restarting in " + (secondsToRestart-i));
			yield return new WaitForSeconds (1f);
		}
		Setup ();
	}

}

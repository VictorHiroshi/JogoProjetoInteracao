using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject[] trashObjects;
	public int trashListSize = 10;
	public Transform spawnPoint;
	public GameObject slingShot;

	private int TrashIndex;
	private bool toInstantiateTrash;
	private int points;
	private GameObject[] trashList;
	private HUDController hud;

	void Awake () {
		TrashIndex = 0;
		toInstantiateTrash = true;
		points = 0;
		hud = gameObject.GetComponent <HUDController> ();
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

		trashList = new GameObject[trashListSize];
		GenerateRandomicTrashList ();

		hud.ChangePunctuationText (points);
	}

	void Update ()
	{
		if(TrashIndex == trashListSize)
		{
			GameOver ();
		}
		else if(toInstantiateTrash)
		{
			InstantiateTrash ();
			toInstantiateTrash = false;
		}
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
		hud.ChangePunctuationText (points);
	}

	private void InstantiateTrash ()
	{
		GameObject toInstantiate = trashList [TrashIndex];
		GameObject instance = Instantiate (toInstantiate, spawnPoint.position, spawnPoint.rotation);
		SpringJoint2D springJoint = instance.GetComponent <SpringJoint2D> ();
		springJoint.connectedBody = slingShot.GetComponent <Rigidbody2D> ();
	}

	private void GameOver()
	{
		Debug.Log ("Game over!");
	}

	private void GenerateRandomicTrashList ()
	{
		for(int i=0; i< trashListSize; i++)
		{
			trashList[i] = trashObjects [Random.Range (0, trashObjects.Length)];
		}
	}
}

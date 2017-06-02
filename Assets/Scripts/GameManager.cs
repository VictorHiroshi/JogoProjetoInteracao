using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject[] trashObjects;
	public Transform spawnPoint;
	public GameObject slingShot;

	private int TrashIndex;
	private bool toInstantiateTrash;

	void Awake () {
		TrashIndex = 0;
		toInstantiateTrash = true;
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
	}

	void Update ()
	{
		if(TrashIndex == trashObjects.Length)
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

	private void InstantiateTrash ()
	{
		GameObject toInstantiate = trashObjects [TrashIndex];
		GameObject instance = Instantiate (toInstantiate, spawnPoint.position, spawnPoint.rotation);
		SpringJoint2D springJoint = instance.GetComponent <SpringJoint2D> ();
		springJoint.connectedBody = slingShot.GetComponent <Rigidbody2D> ();
	}

	private void GameOver()
	{
		Debug.Log ("Game over!");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public Text punctuationText;

	void Start()
	{
		punctuationText.text = string.Empty;
	}

	public void ChangePunctuationText(int newPuctuation)
	{
		punctuationText.text = "Points: " + newPuctuation;
	}
}

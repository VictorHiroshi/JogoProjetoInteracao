using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public Text punctuationText;
	public Image messagePanelImage;
	public Text messagePanelText;

	void Start()
	{
		punctuationText.text = string.Empty;
		messagePanelText.text = string.Empty;
		messagePanelImage.enabled = false;
	}

	public void ChangePunctuationText(int newPuctuation)
	{
		punctuationText.text = "Points: " + newPuctuation;
	}

	public void ShowGameOverMessage(string message)
	{
		messagePanelText.text = message;
		messagePanelImage.enabled = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public Text punctuationText;
	public Image messagePanelImage;
	public Text messagePanelText;
	public Button restartButton;
	public float delayToShowRestartButton = 1f;

	private WaitForSeconds restartDelay;

	void Awake()
	{
		punctuationText.text = string.Empty;
		HidePanelMessage ();
		restartDelay = new WaitForSeconds (delayToShowRestartButton);
	}

	public void UpdatePunctuationText(int newPuctuation)
	{
		punctuationText.text = "Points: " + newPuctuation;
	}

	public void HidePanelMessage()
	{
		messagePanelText.text = string.Empty;
		restartButton.gameObject.SetActive (false);
		messagePanelImage.enabled = false;
	}

	public void ShowPanelMessage(string message)
	{
		messagePanelText.text = message;
		messagePanelImage.enabled = true;
	}

	public IEnumerator ShowRestartButton()
	{
		yield return restartDelay;
		restartButton.gameObject.SetActive (true);
		restartButton.onClick.AddListener (RestartClick);
	}

	private void RestartClick()
	{
		restartButton.gameObject.SetActive (false);
		GameManager.instance.RestartGame ();
	}
}

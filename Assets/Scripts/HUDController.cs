using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public Text punctuationText;
	public Image messagePanelImage;
	public Text messagePanelText;
	public Button panelButton;
	public float delayToShowButton = 1f;

	private WaitForSeconds showButtonDelay;
	private ThrowableObject trashShowingMessage;

	void Awake()
	{
		punctuationText.text = string.Empty;
		HidePanelMessage ();
		showButtonDelay = new WaitForSeconds (delayToShowButton);
	}

	public void UpdatePunctuationText(int newPuctuation)
	{
		punctuationText.text = "Points: " + newPuctuation;
	}

	public void HidePanelMessage()
	{
		messagePanelText.text = string.Empty;
		panelButton.gameObject.SetActive (false);
		messagePanelImage.enabled = false;
	}

	public void ShowPanelMessage(string message)
	{
		messagePanelText.text = message;
		messagePanelImage.enabled = true;
	}

	public IEnumerator ShowPanelRestartButton()
	{
		yield return showButtonDelay;
		panelButton.GetComponentInChildren <Text> ().text = "Recomeçar";
		panelButton.gameObject.SetActive (true);
		panelButton.onClick.AddListener (RestartClick);
	}

	public IEnumerator ShowPanelButton(string buttonText, ThrowableObject instance)
	{
		yield return showButtonDelay;
		trashShowingMessage = instance;
		panelButton.GetComponentInChildren <Text> ().text = buttonText;
		panelButton.gameObject.SetActive (true);
		panelButton.onClick.AddListener (ShowingThrowableMessage);
	}

	private void RestartClick()
	{
		panelButton.gameObject.SetActive (false);
		GameManager.instance.RestartGame ();
		panelButton.onClick.RemoveAllListeners ();
	}

	private void ShowingThrowableMessage()
	{
		HidePanelMessage ();
		panelButton.gameObject.SetActive (false);
		trashShowingMessage.ReadMessageOnPanel ();
		panelButton.onClick.RemoveAllListeners ();
	}
}

using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Networking;

public class HyperText : MonoBehaviour {

	//Object Types (UI/Object)
	private enum HyperTextMode {
		UI,
		Email,
		GameObject
	};

	//Methods of opening the URL depend on if the object is UI or a 3D Object.
	[SerializeField]
	private HyperTextMode Mode;

	//Link URL w/ default URL provided.
	[Tooltip("Paste the link here")]
	[SerializeField]
	private string hyperlinkString = "http://www.google.com";


	private void Start() {
		if(Mode == HyperTextMode.UI) {
			//Warns the user if there is no EventTrigger present and then proceeds to add the EventTrigger component for the user.
			if(GetComponent<EventTrigger>() == null) {
				Debug.LogWarning("An Event Trigger Is Required For 'HyperText' Script To Run!\nAn Event Trigger Has Been Added For You.");
				this.gameObject.AddComponent<EventTrigger>();
			}
			EventTrigger trigger = GetComponent<EventTrigger>( );
			EventTrigger.Entry entry = new EventTrigger.Entry( );
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener( (data) => { OnPointerClickDelegate((PointerEventData) data); } );
			trigger.triggers.Add(entry);
		}
		else if (Mode == HyperTextMode.Email)
		{
			//Warns the user if there is no EventTrigger present and then proceeds to add the EventTrigger component for the user.
			if (GetComponent<EventTrigger>() == null)
			{
				Debug.LogWarning("An Event Trigger Is Required For 'HyperText' Script To Run!\nAn Event Trigger Has Been Added For You.");
				this.gameObject.AddComponent<EventTrigger>();
			}
			EventTrigger trigger = GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener((data) => { OnPointerClickDelegate((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}
		else {
			//Add a mesh collider if the object doesn't have any colliders.
			//Preferably you would want to choose your collider to your specific purposes instead of
			//having the program generate one for you.
			if(GetComponent<Collider>() == null) {
				this.gameObject.AddComponent<MeshCollider>();
			}
		}
	}

	private void OnMouseDown() {
		if(Mode == HyperTextMode.GameObject) {
			//If the mode is set to GameObject, then execute the hyperlink.
			Hyperlink(hyperlinkString);
		}
	}

	//Delegate used for UI Click handling events.
	private void OnPointerClickDelegate(PointerEventData data) {
		if (Mode == HyperTextMode.UI)
		{
			Hyperlink(hyperlinkString);
		}
		else if (Mode == HyperTextMode.Email)
		{
			SendEmail(hyperlinkString);
		}
	}

	//Hyperlink method takes a string URL parameter and opens it.
	private void Hyperlink(string link) {
		Application.OpenURL(link);
	}
	private void SendEmail(string _email)
	{
		string email = _email.ToString();
		string subject = MyEscapeURL("Village pine Feedback ");
		string body = MyEscapeURL("Please\r\nprovide your feedback here...");
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
	private string MyEscapeURL(string url)
	{
		return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
	}
}

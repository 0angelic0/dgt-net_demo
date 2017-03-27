using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DGTMainController : MonoBehaviour
{
	public Text m_chat;
	public InputField m_inputText;

	void Update ()
	{
		DGTRemote.GetInstance ().ProcessEvents (); 
	}
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (ConnectToServer ());
	}
	
	public IEnumerator ConnectToServer ()
	{
		DGTPacket.Config pc = new DGTPacket.Config ("localhost", 3456);
		DGTRemote.resetGameState ();
		DGTRemote gamestate = DGTRemote.GetInstance ();
		gamestate.Connect (pc.host, pc.port);
		gamestate.ProcessEvents ();
		yield return new WaitForSeconds (0.1f);
		for (int i = 0; i < 10; i++) {
			if (gamestate.Connected ()) {
				break;
			}
			if (gamestate.ConnectFailed ()) {
				break;
			}
			
			gamestate.ProcessEvents ();
			yield return new WaitForSeconds (i * 0.1f);
		}
		
		if (gamestate.Connected ()) {			
			Debug.Log ("Login Finish");
			// send login
			gamestate.RequestLogin ();
			gamestate.mainController = this;
			
		} else {
			yield return new WaitForSeconds (5f);
			Debug.Log ("Cannot connect");
		}
//		StartCoroutine(PingTest());
		yield break;
	}
	
	public IEnumerator PingTest ()
	{
		int i = 0;
		while (true) {
			DGTRemote.GetInstance ().TryPing(i);
			i++;
			yield return new WaitForSeconds(3);
		}
	}

	public void sendChat()
	{
		if(DGTRemote.Instance.Connected())
		{
			DGTRemote.Instance.RequestSendChat(m_inputText.text);
		}
	}
}

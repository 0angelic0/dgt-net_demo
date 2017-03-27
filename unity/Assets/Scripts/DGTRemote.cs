using UnityEngine;
using System.Collections;

public class DGTRemote : MonoBehaviour 
{
	public DGTMainController mainController;

	private enum State
	{
		DISCONNECTED = 0,
		DISCONNECTING,
		CONNECTED,
		CONNECTING,
	};
	
	
	private State _State;
	private DGTPacket _Packet;
	
	////////////////////////////////////////////////////////////////////////////////
	// Singleton Design Pattern.
	////////////////////////////////////////////////////////////////////////////////
	
	private static GameObject gameObjectState;
	private static DGTRemote g_instance;
	public static DGTRemote GetInstance() { 
		if(g_instance ==null){
			gameObjectState = new GameObject("DGTRemote");
			g_instance = gameObjectState.AddComponent<DGTRemote>();
			DontDestroyOnLoad(gameObjectState);
		}
		return g_instance;
	}
	public static void resetGameState(){
		
		Destroy(gameObjectState);
		g_instance = null;
	}
	public static DGTRemote Instance { get { return GetInstance(); } }
	
	////////////////////////////////////////////////////////////////////////////////
	public void Connect(string host, int port)
	{
		if (_State != State.DISCONNECTED) return;
		
		_State = State.CONNECTING;
		_Packet.Connect(host, port);
	}
	
	public void Disconnect()
	{
//		Debug.Log (" Disconnect : _State "+ _State);
		if (_State != State.CONNECTED) return;
		_State = State.DISCONNECTED;
		_Packet.Disconnect();
	}
	
	public void OnConnected()
	{
//		Debug.Log (" Connected : _State "+ _State);
		_State = State.CONNECTED;
	}
	
	public void OnDisconnected()
	{
		if(_State != State.DISCONNECTED)
		{
			
		}	
		_State = State.DISCONNECTED;	
	}
	
	public void OnFailed()
	{
		if(_State != State.DISCONNECTED)
		{

		}
		_State = State.DISCONNECTED;
	}
	
	public bool Connected()
	{
		return _Packet.Connected && _State == State.CONNECTED;
	}
	
	public bool ConnectFailed()
	{
		return _Packet.Failed;
	}
	
	public void ProcessEvents()
	{
		
		_Packet.ProcessEvents();
	}
	
	void Awake()
	{
		_Packet = new DGTPacket(this);
		_State = State.DISCONNECTED;		
		//test();
	}
	////////////////////////////////////////////////////////////////////////////////
	public void RequestLogin()
	{
		_Packet.RequestLogin();
	}

	public void RequestSendChat(string msg)
	{
		_Packet.RequestSendChat(msg);
	}

	public void TryPing(int pingTime)
	{
		_Packet.RequestPing(pingTime);
	}

	public void recvQuestion()
	{
		_Packet.RequestAnswer();
	}

	public void recvChat(string msg)
	{
		mainController.m_chat.text += msg+"\n";
	}
}

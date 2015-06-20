using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour {

	public string PlayerName;
	public static MultiplayerManager instance;
	public List<MPPlayer> PlayerList = new List<MPPlayer>();
	public List<MPZombie> ZombieList = new List<MPZombie>();

	// Use this for initialization

	// bouton name player a metre a voire ...
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		// verification si les zombie ou les joueur sont dans la zone et les afficher

	
	}

	void client_AddZombieToList(int id)
	{
		MPZombie temps = new MPZombie ();
		temps.id = id;
	}

	void Client_AddPlayerToList(string PlayerName , NetworkPlayer PlayerNetwork)
	{
		MPPlayer temps = new MPPlayer ();
		temps.PlayerName = PlayerName;
		temps.PlayerNetwork = PlayerNetwork;
		PlayerList.Add (temps);
	}

	void RemovePlayer(NetworkPlayer PlayerNetwork)
	{
		MPPlayer temps = null;
		foreach (MPPlayer pl in PlayerList) 
		{
			if(pl.PlayerNetwork == PlayerNetwork)
			{
				temps = pl;
			}
		}
		if (temps != null) 
		{
			PlayerList.Remove(temps);
		}
	}
}
public class MPZombie
{
	public int id;
}

public class MPPlayer
{
	public string PlayerName = "";
	public NetworkPlayer PlayerNetwork;

}
using UnityEngine;
using System.Collections;

public class insertName : MonoBehaviour {

	public GameObject loadingMenue;
	public GUI Name ;
	
	public void Loadimage()
	{
		loadingMenue.SetActive (false);
		MultiplayerManager.instance.name = Name.ToString(); 
	}
}

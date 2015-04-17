using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	
	public GameObject loadingMenue;
	
	public void Loadimage()
	{
		loadingMenue.SetActive (true);
	}

}
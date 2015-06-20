using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputIP : MonoBehaviour {

	public static string IP_Host;

	// Use this for initialization
	void Start () {
		IP_Host = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setIpHost(string inputField){
		IP_Host = inputField;
	}
}

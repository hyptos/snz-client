//css_import Mesh.cs, ZEvent.cs
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class connexionClient : MonoBehaviour {

	internal Boolean socketReady = false;

	TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	String Host = "188.166.115.159";
	Int32 Port = 8080;
	
	void Start () {   
		setupSocket();
		readRessource();
		//writeSocket("Tuturuuu");
		//closeSocket ();
		
	}
	void Update () {
	}
	
	public void setupSocket() { 
		try {
			mySocket = new TcpClient(Host, Port);
			theStream = mySocket.GetStream(); 
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;         
		}
		catch (Exception e) {
			Debug.Log("Socket error: " + e);
		}
	}

	public void readSocket(){
		
		readRessource();
		/*byte[] TabIdentifier = new byte[4];
		theStream.Read(TabIdentifier,0,4);
		int Identifier = BitConverter.ToInt32(TabIdentifier, 0);
		
		switch (Identifier)
		{
		case 11:
			//Read Ressourse
			readRessource();
			break;
		case 12:
			//Event
			ZEvent monEvent = new ZEvent();
			monEvent.fromBinary(theStream);
			break;
		default:
			byte[] TabTaille = new byte[4];
			theStream.Read(TabTaille,0,4);
			int tailleTrame = BitConverter.ToInt32(TabTaille, 0);
			
			Debug.Log ("type inconnue, je jette tout : " + tailleTrame + "Byte");
			byte[] dechet = new byte[tailleTrame];
			theStream.Read(dechet,0,tailleTrame);
			break;
		}*/
	}
	
	public void readRessource() { 
		byte[] isInt2 = new byte[4];
		theStream.Read(isInt2,0,4);
		//int test2 = BitConverter.ToInt32(isInt2, 0);
		
		byte[] QUuid = new byte[38];
		theStream.Read(QUuid,0,38);
		//pour l'instant on ne fait rien avec le QUuid.
		
		byte[] isInt = new byte[8];
		theStream.Read(isInt,0,8);
		ulong taille = (ulong)BitConverter.ToInt64(isInt, 0);
		
		byte[] bytes = new byte[taille];
		theStream.Read(bytes,0,(int)taille);
		string type = System.Text.Encoding.ASCII.GetString(bytes);
		
		if (type == "Mesh") {
			ZMesh monMesh = new ZMesh();
			monMesh.readMesh(theStream);
		} else {
			Debug.Log ("type inconnue, je jette tout : " + taille + "octets");
			byte[] dechet = new byte[taille];
			theStream.Read(dechet,0,(int)taille);
		}
	}

	public void writeSocket(string theLine) {
		
		byte[] isInt = new byte[4];
		isInt = BitConverter.GetBytes((UInt32)theLine.Length);
		byte[] asciiBytes = System.Text.Encoding.ASCII.GetBytes(theLine);
		byte[] msg = new byte[isInt.Length+asciiBytes.Length];
		for (int i=0;i<isInt.Length;i++){
			msg[i] = isInt[i];
		}
		for (int i=4;i<asciiBytes.Length+4;i++){
			msg[i] = asciiBytes[i-4];
		}
		theStream.Write (msg, 0, msg.Length);
	}

	public string readString() {
		byte[] isInt = new byte[4];
		theStream.Read(isInt,0,4);
		int test = BitConverter.ToInt32(isInt, 0);
		
		byte[] bytes = new byte[test];
		theStream.Read(bytes,0,test);
		string str = System.Text.Encoding.ASCII.GetString(bytes);

		return str;
	}
	
	public void envoieMove(ulong id, int t, int s, Vector3 pos, Vector3 dir){
		ZEvent eventMove = new ZEvent(id, t, s, pos.x, pos.y, pos.z, dir.x, dir.y, dir.z);
		byte[] msg = eventMove.toBinary();
		Debug.Log ("position Envoyer");
		theStream.Write (msg, 0, msg.Length);
	}

	public void closeSocket() {
		if (!socketReady)
			return;
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}
}
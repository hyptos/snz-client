//css_import Mesh.cs, ZEvent.cs, InputIp.cs
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace UnityStandardAssets.Characters
{
	public class connexionClient : MonoBehaviour {
		internal Boolean socketReady = false;
		Dictionary<ulong, int> Characters = new Dictionary<ulong, int>();


		TcpClient mySocket;
		NetworkStream theStream;
		String Host = InputIP.IP_Host;
		Int32 Port = 3000;
		ulong IDjoueur;
		
		void Start () {
			setupSocket();
			ZEvent eventMove = new ZEvent((ulong)0, 2, 495.7633f, 0.0f, 495.411f, 0.0f, 0.0f, 0.0f);
			byte[] evt = eventMove.toBinary();
			formatAndSend ("c", evt);
			readSocket();
			/*ThreadStart listen = new ThreadStart(readSocket);
			Thread thread = new Thread(listen);
			thread.Start();*/
			//closeSocket();
			
		}

		void Update () {
			if (theStream.DataAvailable) {
				readSocket ();
			}
		}
		
		public void setupSocket() { 
			try {
				Debug.Log("Host : " + Host + " Port : " + Port);
				mySocket = new TcpClient(Host, Port);
				theStream = mySocket.GetStream(); 
				socketReady = true;         
			}
			catch (Exception e) {
				Debug.Log("Socket error: " + e);
			}
		}

		public void readSocket(){

			//readRessource();
			byte[] TabTaille = new byte[4];
			theStream.Read(TabTaille,0,4);
			int taille = BitConverter.ToInt32(TabTaille, 0);
			
			byte[] TabIdentifier = new byte[1];
			theStream.Read(TabIdentifier,0,1);
			char[] Identifier = System.Text.Encoding.ASCII.GetChars(TabIdentifier);
			Debug.Log (Identifier.Length);

			switch (Identifier[0])
			{
			case 'r':
				//Read Ressourse
				readRessource();
				break;
			case 'w':
				//Read Ressourse
				string str = readString(taille-1);
				Debug.Log("reçu "+str);
				break;
			case 'u':
				//Event
				ZEvent monEvent = new ZEvent();
				monEvent.fromBinary(theStream);
				receiveMove(monEvent);
				break;
			case 'c':
				//connexion
				ZEvent maConn = new ZEvent();
				maConn.fromBinary(theStream);
				//receiveMove(maConn);
				IDjoueur = maConn.getEvent();
				//Debug.Log("connexion !! "+maConn.getEvent()+" "+maConn.getType()+" "+maConn.getPosition().x+" "+maConn.getPosition().y+" "+maConn.getPosition().z);
				//Debug.Log("connexion !! "+maConn.getEvent()+" "+maConn.getType()+" "+maConn.getDirection().x+" "+maConn.getDirection().y+" "+maConn.getDirection().z);

				break;
			default:
				taille = taille-1;
				Debug.Log ("type inconnue, je jette tout : " + taille + "Byte");
				byte[] dechet = new byte[taille];
				theStream.Read(dechet,0,taille);
				break;
			}
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
			byte[] asciiBytes = System.Text.Encoding.ASCII.GetBytes(theLine);
			formatAndSend ("w", asciiBytes);
		}
		
		public string readString(int taille) {
			byte[] bytes = new byte[taille];
			theStream.Read(bytes,0,taille);
			string str = System.Text.Encoding.ASCII.GetString(bytes);
			return str;
		}
		
		public void envoieMove(Vector3 pos, Vector3 dir){
			ZEvent eventMove = new ZEvent(IDjoueur, 2, pos.x, pos.y, pos.z, dir.x, dir.y, dir.z);
			byte[] evt = eventMove.toBinary();
			formatAndSend ("u", evt);
		}

		public void formatAndSend(string ident, byte[] data){
			
			byte[] isInt = new byte[4];
			isInt = BitConverter.GetBytes((UInt32)(data.Length+1));
			
			byte[] identB = System.Text.Encoding.ASCII.GetBytes(ident);
			
			byte[] msg = new byte[isInt.Length+data.Length+1];
			
			for (int i=0;i<isInt.Length;i++){
				msg[i] = isInt[i];
			}
			msg[isInt.Length] = identB[0];
			for (int i=5;i<data.Length+5;i++){
				msg[i] = data[i-5];
			}
			if (theStream.CanWrite){
				theStream.Write (msg, 0, msg.Length);
			}
			else{
				Debug.Log("Sorry.  You cannot write to this NetworkStream.");  
			}
		}

		public void receiveMove(ZEvent monEvent){
			int ID = 0;
			if(monEvent.getEvent() == IDjoueur)
				return;
			if (Characters.TryGetValue (monEvent.getEvent(), out ID)) {
				GameObject o = GameObject.Find("z@walk "+ID);
				AICharacterControl cc = (AICharacterControl)o.GetComponent (typeof(AICharacterControl));
				transform.position = monEvent.getPosition();
				//Debug.Log ("z@walk "+ID+" "+monEvent.getPosition().x+" "+monEvent.getPosition().y+" "+monEvent.getPosition().z+" ");
				//o.transform.forward = monEvent.transform.forward;
				cc.SetTarget(transform);
			} else {
				//on fait pop le zombie ou le characters
				Characters.Add(monEvent.getEvent(),(int)monEvent.getEvent());
			}
			
		}

		public void closeSocket() {
			if (!socketReady)
				return;
			mySocket.Close();
			socketReady = false;
		}
	}
}
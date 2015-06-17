//css_import Mesh.cs, ZEvent.cs
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;

namespace UnityStandardAssets.Characters
{
	public class connexionClient : MonoBehaviour {

		internal Boolean socketReady = false;
		Dictionary<ulong, int> Characters = new Dictionary<ulong, int>();

		TcpClient mySocket;
		NetworkStream theStream;
		StreamWriter theWriter;
		StreamReader theReader;
		String Host = "192.168.1.74";
		Int32 Port = 3000;
		
		void Start () {   
			setupSocket();
			writeSocket("Tuturuuu");
			writeSocket("Tuturuuu");
			//readSocket();
			//closeSocket();
			
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
				Debug.Log ("yop");
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

			byte[] ident = System.Text.Encoding.ASCII.GetBytes("w");

			byte[] msg = new byte[isInt.Length+asciiBytes.Length+1];

			for (int i=0;i<isInt.Length;i++){
				msg[i] = isInt[i];
			}
			msg[isInt.Length] = ident[0];
			for (int i=5;i<asciiBytes.Length+5;i++){
				msg[i] = asciiBytes[i-5];
			}

			if (theStream.CanWrite){
				theStream.Write (msg, 0, msg.Length);
			}
			else{
				Debug.Log("Sorry.  You cannot write to this NetworkStream.");  
			}
			//theStream.EndWrite
		}
		
		public string readString(int taille) {
			byte[] bytes = new byte[taille];
			theStream.Read(bytes,0,taille);
			string str = System.Text.Encoding.ASCII.GetString(bytes);

			return str;
		}
		
		public void envoieMove(ulong id, int t, Vector3 pos, Vector3 dir){
			/*ZEvent eventMove = new ZEvent(id, t, pos.x, pos.y, pos.z, dir.x, dir.y, dir.z);
			byte[] evt = eventMove.toBinary();

			byte[] isInt = new byte[4];
			isInt = BitConverter.GetBytes((UInt32)evt.Length);

			byte[] ident = System.Text.Encoding.ASCII.GetBytes("u");
			
			byte[] msg = new byte[isInt.Length+evt.Length+1];
			
			for (int i=0;i<isInt.Length;i++){
				msg[i] = isInt[i];
			}
			msg[isInt.Length] = ident[0];
			for (int i=5;i<evt.Length+5;i++){
				msg[i] = evt[i-5];
			}
			theStream.Write (msg, 0, msg.Length);*/
			writeSocket("Totorooo");
		}
		
		public void receiveMove(ZEvent monEvent){
			int ID = 0;
			if (Characters.TryGetValue (monEvent.getEvent(), out ID)) {
				Debug.Log ("z@walk "+ID);
				/*Transform trf = new Transform();
				trf.position = monEvent.getPosition();
				trf.transform.TransformDirection(monEvent.getDirection());
				GameObject o = GameObject.Find("z@walk "+ID);
				AICharacterControl cc = (AICharacterControl)o.GetComponent (typeof(AICharacterControl));
				cc.SetTarget(transform.position);*/

			} else {
				//on fait pop le zombie ou le characters
				Characters.Add(monEvent.getEvent(),(int)monEvent.getEvent());
			}
			
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
}
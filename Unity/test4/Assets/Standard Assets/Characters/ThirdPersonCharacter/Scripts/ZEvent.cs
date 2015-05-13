﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class ZEvent : MonoBehaviour {
	
	///Constructeur
	public ZEvent(){
		m_id = 0;
		m_type = 0;
		m_position = new Vector3 (0, 0, 0);
		m_direction = new Vector3 (0, 0, 0);
	}

	///Constructeur
	public ZEvent(ulong id, int t, float x, float y, float z, float dx, float dy, float dz){
		m_id = id;
		m_type = t;
		m_position = new Vector3 (x, y, z);
		m_direction = new Vector3 (dx, dy, dz);
	}
	
	///Retourne l'id de l'entité
	ulong getEvent(){
		return this.m_id;
	}
	
	///Retourne le type de l'entité
	int getType(){
		return this.m_type;
	}

	Vector3 getPosition (){
		return this.m_position;
	}
	Vector3 getDirection (){
		return this.m_direction;
	}
		
	ulong m_id;    // Id de l'entité
	
	int m_type;        // Type de l'entité

	Vector3 m_position;
	Vector3 m_direction;

	public byte[] toBinary(){

		byte[] msg = new byte[41];
		
		//On prend la taille 
		byte[] TailleBin1 = BitConverter.GetBytes(37);
		TailleBin1.CopyTo (msg, 0);

		//On on ajoute le 'u' pour event 
		byte[] indiceEvent = BitConverter.GetBytes('u');
		indiceEvent.CopyTo (msg, 4);

		//On prend l'ID 
		byte[] IDBin = BitConverter.GetBytes(m_id);
		//IDBin [1] = IDBin [0];
		IDBin.CopyTo (msg, 5);
		Debug.Log (sizeof(int));
		//Debug.Log (" "+IDBin[0]+" "+IDBin[1]+" "+IDBin[2]+" "+IDBin[3]+" "+IDBin[4]+" "+IDBin[5]+" "+IDBin[6]+" "+IDBin[7]);
		
		//On prend le type
		byte[] typeBin = BitConverter.GetBytes(m_type);
		typeBin.CopyTo (msg, 13);
		//Debug.Log (typeBin.Length);
		//Debug.Log (" "+typeBin[0]+" "+typeBin[1]+" "+typeBin[2]+" "+typeBin[3]);

		//On prend la posX
		byte[] posXBin = BitConverter.GetBytes(m_direction.x);
		posXBin.CopyTo (msg, 17);
		//Debug.Log (posXBin.Length);
		//Debug.Log (" "+posXBin[0]+" "+posXBin[1]+" "+posXBin[2]+" "+posXBin[3]);

		//On prend la posY
		byte[] posYBin = BitConverter.GetBytes(m_direction.z);
		posYBin.CopyTo (msg, 21);
		//Debug.Log (posYBin.Length);
		//Debug.Log (" "+posYBin[0]+" "+posYBin[1]+" "+posYBin[2]+" "+posYBin[3]);
		
		//On prend la posZ
		byte[] posZBin = BitConverter.GetBytes(m_direction.y);
		posZBin.CopyTo (msg, 25);
		//Debug.Log (posZBin.Length);
		//Debug.Log (" "+posZBin[0]+" "+posZBin[1]+" "+posZBin[2]+" "+posZBin[3]);
		
		//On prend la dirX
		byte[] dirXBin = BitConverter.GetBytes(m_position.x);
		dirXBin.CopyTo (msg, 29);
		//Debug.Log (dirXBin.Length);
		//Debug.Log (" "+dirXBin[0]+" "+dirXBin[1]+" "+dirXBin[2]+" "+dirXBin[3]);

		//On prend la dirY
		byte[] dirYBin = BitConverter.GetBytes(m_position.z);
		dirYBin.CopyTo (msg, 33);
		//Debug.Log (dirYBin.Length);
		//Debug.Log (" "+dirYBin[0]+" "+dirYBin[1]+" "+dirYBin[2]+" "+dirYBin[3]);
		
		//On prend la dirZ
		byte[] dirZBin = BitConverter.GetBytes(m_position.y);
		dirZBin.CopyTo (msg, 37);
		//Debug.Log (dirZBin.Length);
		//Debug.Log (" "+dirZBin[0]+" "+dirZBin[1]+" "+dirZBin[2]+" "+dirZBin[3]);

		return msg;
	}

	public ZEvent fromBinary(NetworkStream stream){
		//On prend l'ID 
		byte[] IDBin = new byte[8];
		stream.Read(IDBin,0,8);
		m_id = BitConverter.ToUInt64(IDBin,0); 
		
		//On prend le type
		byte[] typeBin = new byte[4];
		stream.Read(typeBin,0,4);
		m_type = BitConverter.ToInt32(typeBin,0); 
	
		//On prend la posX
		byte[] posXBin = new byte[8];
		stream.Read(posXBin,0,8);
		float posx = BitConverter.ToSingle(posXBin,0); 

		//On prend la posY
		byte[] posYBin = new byte[8];
		stream.Read(posYBin,0,8);
		float posy = BitConverter.ToSingle(posYBin,0); 
		
		//On prend la posZ
		byte[] posZBin = new byte[8];
		stream.Read(posZBin,0,8);
		float posz = BitConverter.ToSingle(posZBin,0); 

		m_position = new Vector3(posx, posy, posz);
		
		//On prend la dirX
		byte[] dirXBin = new byte[8];
		stream.Read(dirXBin,0,8);
		float dirx = BitConverter.ToSingle(dirXBin,0); 
		
		//On prend la dirY
		byte[] dirYBin = new byte[8];
		stream.Read(dirYBin,0,8);
		float diry = BitConverter.ToSingle(dirYBin,0); 
		
		//On prend la dirZ
		byte[] dirZBin = new byte[8];
		stream.Read(dirZBin,0,8);
		float dirz = BitConverter.ToSingle(dirZBin,0); 
		
		m_direction = new Vector3 (dirx, diry, dirz);

		return this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


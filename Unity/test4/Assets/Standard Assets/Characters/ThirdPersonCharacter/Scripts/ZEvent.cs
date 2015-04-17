using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class ZEvent : MonoBehaviour {
	
	///Constructeur
	public ZEvent(){
		m_id = 0;
		m_state = 0;
		m_type = 0;
		m_position = new Vector3 (0, 0, 0);
		m_direction = new Vector3 (0, 0, 0);
	}

	///Constructeur
	public ZEvent(ulong id, int t, int s, float x, float y, float z, float dx, float dy, float dz){
		m_id = id;
		m_state = s;
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
	
	///Retourne l'état de l'entité
	int getState(){
		return this.m_state;
	}

	Vector3 getPosition (){
		return this.m_position;
	}
	Vector3 getDirection (){
		return this.m_direction;
	}
		
	ulong m_id;    // Id de l'entité
	
	int m_type;        // Type de l'entité
	int m_state;      // Etat de l'entité

	Vector3 m_position;
	Vector3 m_direction;

	public byte[] toBinary(){
		byte[] msg = new byte[68];
		
		//On prend la taille 
		byte[] TailleBin = BitConverter.GetBytes(64);
		TailleBin.CopyTo (msg, 0);

		//On prend l'ID 
		byte[] IDBin = BitConverter.GetBytes(m_id);
		IDBin.CopyTo (msg, 4);
		
		//On prend le type
		byte[] typeBin = BitConverter.GetBytes(m_type);
		typeBin.CopyTo (msg, 12);
		
		//On prend le state
		byte[] stateBin = BitConverter.GetBytes(m_state);
		stateBin.CopyTo (msg, 16);
		
		//On prend la posX
		byte[] posXBin = BitConverter.GetBytes(m_position.x);
		posXBin.CopyTo (msg, 20);
		
		//On prend la posY
		byte[] posYBin = BitConverter.GetBytes(m_position.y);
		posYBin.CopyTo (msg, 28);
		
		//On prend la posZ
		byte[] posZBin = BitConverter.GetBytes(m_position.z);
		posZBin.CopyTo (msg, 36);
		
		//On prend la dirX
		byte[] dirXBin = BitConverter.GetBytes(m_direction.x);
		dirXBin.CopyTo (msg, 44);
		
		//On prend la dirY
		byte[] dirYBin = BitConverter.GetBytes(m_direction.y);
		dirYBin.CopyTo (msg, 52);
		
		//On prend la dirZ
		byte[] dirZBin = BitConverter.GetBytes(m_direction.z);
		dirZBin.CopyTo (msg, 60);

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
		
		//On prend le state
		byte[] stateBin = new byte[4];
		stream.Read(stateBin,0,4);
		m_state = BitConverter.ToInt32(stateBin,0); 
		
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


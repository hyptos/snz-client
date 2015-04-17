using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class Mesh : MonoBehaviour {

	NetworkStream theStream;

	public bool readMesh(NetworkStream netstr) {  
		this.theStream = netstr;
		byte[] isInt = new byte[4];
		theStream.Read (isInt, 0, 4);
		int taille = (int)BitConverter.ToInt32 (isInt, 0); 
		Debug.Log ("trois " + taille);
		
		byte[] QUuid = new byte[38];
		theStream.Read (QUuid, 0, 38);
		//pour l'instant on ne fait rien avec le QUuid.
		
		byte[] isInt2 = new byte[8];
		theStream.Read (isInt2, 0, 8);
		int tailleNom = (int)BitConverter.ToInt64 (isInt2, 0); 
		Debug.Log ("taille nom " + tailleNom);
		
		byte[] bytes = new byte[tailleNom];
		theStream.Read (bytes, 0, tailleNom);
		string nom = System.Text.Encoding.ASCII.GetString (bytes);
		nom = nom.Substring(14);
		Debug.Log (nom); 
		
		//Nom du mesh
		System.IO.File.WriteAllText (@".\Assets\Resources\"+nom, "o" + nom + "\n\n");
		
		//Nombre de sommet de l'obj
		int nbSommet = read4bytesToInt ("nbSommet");
		
		for (int i = 0; i <nbSommet; i++) {
			//Debug.Log (i+1 + "eme point");
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "v ");
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("x") + " ");	
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("y") + " ");		
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("z") + " ");		
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "\n");
		}		
		System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "\n");
		
		//Nombre de Textures de l'obj
		int nbTextures = read4bytesToInt ("nbTextures");
		
		for (int i = 0; i <nbTextures; i++) {
			//Debug.Log (i+1 + "eme point");
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "vt ");
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("x") + " ");	
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("y") + " ");	
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("z") + " ");			
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "\n");
		}		
		System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "\n");
		
		//Nombre de normales de l'obj
		int nbnormales = read4bytesToInt ("nbnormales");
		
		for (int i = 0; i <nbnormales; i++) {
			//Debug.Log (i+1 + "eme point");
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "vn ");
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("x") + " ");	
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("y") + " ");		
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, read4bytesToFloat ("z") + " ");		
			System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "\n");
		}		
		System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "\n");
		
		//Nombre de Triangles de l'obj
		int nbTriangles = read4bytesToInt ("nbTriangles");
		
		for(int i=0;i<nbTriangles; i++){
			bool hasNormals = read1byt1 ("passage" + i + " m_hasNormals");
			bool hasTexcoords = read1byt3 ("passage" + i + " m_hasTexcoords");
			
			int[] tab = read40bytesToTabInt ("passage" + i + " PremierTab");
			//Debug.Log (tab [0] + " " + tab [1] + " " + tab [2]);
			
			int[] tab2 = read40bytesToTabInt ("passage" + i + " DeuxiemeTab");
			//Debug.Log (tab2 [0] + " " + tab2 [1] + " " + tab2 [2]);
			
			int[] tab3 = read40bytesToTabInt ("passage" + i + " TroisiemeTab");
			//Debug.Log (tab3 [0] + " " + tab3 [1] + " " + tab3 [2]); 
			
			if(hasNormals && hasTexcoords){
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "f "+tab[0]+"/"+tab2[0]+"/"+tab3[0]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[1]+"/"+tab2[1]+"/"+tab3[1]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[2]+"/"+tab2[2]+"/"+tab3[2]+" \n");
			}if(!hasNormals && hasTexcoords){
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "f "+tab[0]+"/"+tab3[0]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[1]+"/"+tab3[1]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[2]+"/"+tab3[2]+" \n");
			}if(hasNormals && !hasTexcoords){
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "f "+tab[0]+"//"+tab2[0]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[1]+"//"+tab2[1]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[2]+"//"+tab2[2]+" \n");
			}if(!hasNormals && !hasTexcoords){
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "f "+tab[0]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[1]+" ");
				System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, tab[2]+" \n");
			}
		}
		System.IO.File.AppendAllText (@".\Assets\Resources\"+nom, "\n");
		
		byte[] tabG = new byte[4];
		theStream.Read (tabG, 0, 4);
		int tailleG = (int)BitConverter.ToInt32 (tabG, 0); 
		Debug.Log ("trois " + tailleG);
		
		Debug.Log (nom.Replace (".obj", ""));
		Debug.Log (nom);
		nom = nom.Replace (".obj", "");
		GameObject o = Resources.Load (nom) as GameObject;
		Instantiate (o, Vector3.zero, Quaternion.identity);

		return true;
	}
	
	public bool read1byt1(string s){
		byte[] binSomm = new byte[1];
		theStream.Read(binSomm,0,1);
		bool nbSommets = BitConverter.ToBoolean(binSomm,0); 
		//Debug.Log (s + " = " + nbSommets);
		//bool nbSommets2 = BitConverter.ToBoolean(binSomm,1); 
		//Debug.Log (s + " = " + nbSommets2);
		return nbSommets;
	}
	
	public bool read1byt3(string s){
		byte[] binSomm = new byte[3];
		theStream.Read(binSomm,0,3);
		bool nbSommets = BitConverter.ToBoolean(binSomm,0); 
		//Debug.Log (s + " = " + nbSommets);
		//bool nbSommets2 = BitConverter.ToBoolean(binSomm,1); 
		//Debug.Log (s + " = " + nbSommets2);
		return nbSommets;
	}
	
	public int read4bytesToInt(string s){
		byte[] binSomm = new byte[4];
		theStream.Read(binSomm,0,4);
		int nbSommets = BitConverter.ToInt32(binSomm, 0); 
		//Debug.Log (s + " = " + nbSommets);
		return nbSommets;
	}
	
	public float read4bytesToFloat(string s){
		byte[] binSomm = new byte[4];
		theStream.Read(binSomm,0,4);
		float val =  BitConverter.ToSingle(binSomm, 0); 
		//Debug.Log (s + " = " + val);
		return val;
	}
	
	public int[] read40bytesToTabInt(string s){
		//Debug.Log (s);
		byte[] binSomm = new byte[12];
		theStream.Read(binSomm,0,12);
		int[] tab = new int[3];
		tab[0] =  1+BitConverter.ToInt32(binSomm, 0);		
		tab[1] =  1+BitConverter.ToInt32(binSomm, 4); 		
		tab[2] =  1+BitConverter.ToInt32(binSomm, 8); 
		return tab;
	}

}

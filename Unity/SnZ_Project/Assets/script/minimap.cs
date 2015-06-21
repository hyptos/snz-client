using UnityEngine;
using System.Collections;
using System.Collections.Generic ; 

public class minimap : MonoBehaviour {
	Transform me;
	GameObject[] zombies;
	//Transform[] zombies;
	
	Texture2D meArrow;
	Texture2D zombiesArrow;
	Camera map;
	int MARGE_GENERALE = 10;
	string debug;
	void start() 
	{
	}
	
	void OnGUI()
	{
		//debug = GUI.TextArea(new Rect(100, 100, 100, 100),debug);
		me = GameObject.Find("Alexis").transform;
		int i = 0;
		zombies = tableauZombies(9); 
		
		
		foreach (Camera c in Camera.allCameras) 
		{
			if(c.name=="CameraMap")
			{
				map = c;
				//debug=zombies[0].ToString();
			}
		}
		meArrow = (Texture2D) Resources.Load ("flecheJoueur");
		zombiesArrow = (Texture2D) Resources.Load ("flecheZombies");
		if (map.enabled) {
			Vector3 objPos = map.WorldToViewportPoint(me.transform.position);
			float meAngle = me.transform.eulerAngles.y;
			Matrix4x4 guiRotationMatrix = GUI.matrix;
			Vector2 pivotMe;
			pivotMe.x = Screen.width*(map.rect.x + (objPos.x * map.rect.width));
			pivotMe.y = Screen.height * (1-(map.rect.y + (objPos.y * map.rect.height)));
			GUIUtility.RotateAroundPivot(meAngle,pivotMe);
			float hauteur_map = (float) 0.25 * Screen.height; 
			GUI.DrawTexture(new Rect((float)(Screen.width * (map.rect.x + (objPos.x*map.rect.width))-12.5),(float) (Screen.height * (1-(map.rect.y + (objPos.y * map.rect.height)))-12.5), 25, 25),meArrow);
			GUI.matrix = guiRotationMatrix;
			
			i = 0;
			for(i = 0;i<zombies.Length;i++)
			{
				float zombieAngle = zombies[i].transform.transform.eulerAngles.y;
				Vector3 zombiesPos = map.WorldToViewportPoint(zombies[i].transform.transform.position);
				Matrix4x4 guiRMatrix = GUI.matrix;
				Vector2 pivotZombie;
				pivotZombie.x = Screen.width*(map.rect.x + (zombiesPos.x * map.rect.width));
				pivotZombie.y = Screen.height * (1-(map.rect.y + (zombiesPos.y * map.rect.height)));
				GUIUtility.RotateAroundPivot(zombieAngle,pivotZombie);
				GUI.DrawTexture(new Rect((float)(Screen.width * (map.rect.x + (zombiesPos.x*map.rect.width))-12.5),(float) (Screen.height * (1-(map.rect.y + (zombiesPos.y * map.rect.height)))-12.5), 25, 25),zombiesArrow);
				GUI.matrix = guiRMatrix;
			}
		}
	}
	
	void Update(){
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); 
	}
	
	Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}
	
	GameObject[] tableauZombies(int layer)
	{
		List<GameObject> zombies = new List<GameObject>();
		GameObject[] obj = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int l = 0, i = 0;
		for (i = 0; i < obj.Length; i++) 
		{
			if(obj[i].layer == 9 && (zombies.Contains(obj[i])) == false)
			{
				//debug = GUI.TextField (new Rect (20*l, 20*l, 20, 20),"'"+obj [i].layer.ToString ()+"'");
				zombies.Add(obj[i]);
				l++;
				//print("layer = "+obj[i].layer.ToString());
			}
		}
		return zombies.ToArray();
	}
}

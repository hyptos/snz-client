using UnityEngine;
using System.Collections;

public class minimap : MonoBehaviour {
	/*Transform me;
	Texture2D meArrow;
	Camera map;
	GameObject target ; 
	int MARGE_GENERALE = 10;*/

	void start() 
	{
	}

	void OnGUI()
	{
		/*map = GameObject.Find ("CameraMap"); 
		meArrow = MakeTex (50, 50, Color.blue); 
		if (map.enabled) {
			Vector3 objPos = map.WorldToViewportPoint(me.transform.position);
			float meAngle = me.transform.eulerAngles.y;
			Matrix4x4 guiRotationMatrix = GUI.matrix;
			Vector2 pivotMe;
			pivotMe.x = Screen.width*(map.rect.x + (objPos.x * map.rect.width));
			pivotMe.y = Screen.height * (1-(map.rect.y + (objPos.y * map.rect.height)));
			GUIUtility.RotateAroundPivot(meAngle,pivotMe);
			float hauteur_map = (float) 0.25 * Screen.height; 
			GUI.DrawTexture(new Rect(Screen.width - hauteur_map - MARGE_GENERALE, MARGE_GENERALE, hauteur_map, hauteur_map),meArrow);
			GUI.matrix = guiRotationMatrix;
		}*/
	}

	void Update()
	{ 
		//transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z); 
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
}

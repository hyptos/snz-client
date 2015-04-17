using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClicktoLoadAsync : MonoBehaviour {

	public Slider loadingbar;
	public GameObject loadingImage;

	public AsyncOperation async;

	public void ClickAsync( int level )
	{
		loadingImage.SetActive (true);
		StartCoroutine (LoadLevelWithBar(level));
	}

	IEnumerator LoadLevelWithBar(int level)
	{
		async = Application.LoadLevelAsync (level);
		while (!async.isDone) {
			loadingbar.value=async.progress;
			yield return null;
		}
	}

}

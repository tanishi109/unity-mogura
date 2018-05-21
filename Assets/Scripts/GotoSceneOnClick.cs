using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoSceneOnClick : MonoBehaviour {

	public void GotoScene(int level)
	{
		SceneManager.LoadScene(level);
	}
}

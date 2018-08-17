using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SmoxMaster : MonoBehaviour {

	public void LoadNext(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}

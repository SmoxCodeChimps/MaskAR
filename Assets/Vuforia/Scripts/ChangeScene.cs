using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeScene = 5f;
    [SerializeField]
    private string sceneToBeLoaded;

    private float timeElapsed;
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > delayBeforeScene)
        {
            SceneManager.LoadScene(sceneToBeLoaded);
        }
    }
}

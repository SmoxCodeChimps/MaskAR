using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    String Projects = "02MainMenu";
 
	// Use this for initialization
	void Start () {
      
        getGroup();
    }

    public void getGroup(){

        string url = "localhost:3000/users/groop";
        Dictionary<string, string> headers = new Dictionary<string, string>();
        Debug.Log("cooks: " + Users.cookie);
        headers.Add("Cookie","_session_id="+ Users.cookie);
        WWW www = new WWW(url,null,headers);
        StartCoroutine(WaitForRequest(www,JoinGroup,EditGroup));
    }
    IEnumerator WaitForRequest(WWW www,Action join, Action edit )
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("JOIN? " + www.text);
            if (www.text != "null") edit();
            else join();

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }
    void JoinGroup()
    {
        GameObject button = GameObject.Find("Canvas/Background/Logo/ButtonProjectEdit");
        Button butt = button.GetComponent<Button>();
        butt.GetComponentInChildren<Text>().text = "Join Group";

        Projects = "09JoinGroup";

    }
    void EditGroup()
    {
        GameObject button = GameObject.Find("Canvas/Background/Logo/ButtonProjectEdit");
        Button butt = button.GetComponent<Button>();
        butt.GetComponentInChildren<Text>().text = "Edit Group";

        Projects = "04ProjectEdit";
    }
    public void LoadNext()
    {

        SceneManager.LoadScene(Projects);
    }
    
    // Update is called once per frame
    void Update () {
		
	}
}

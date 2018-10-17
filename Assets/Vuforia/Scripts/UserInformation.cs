using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInformation : MonoBehaviour {
    Usuario user;
    Dictionary<string, string> headers;
    // Use this for initialization
    void Start () {
        string url = "localhost:3000/users/getUser";
        headers = new Dictionary<string, string>();
        Debug.Log("cooks: " + Users.cookie);
        headers.Add("Cookie", "_session_id=" + Users.cookie);
        WWW www = new WWW(url, null, headers);
        StartCoroutine(WaitForRequest(www));

      
    }
    IEnumerator GetCompas(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("COMPAS");
            Debug.Log(www.text);
            Usuario[] users = JsonHelper.FromJson<Usuario>(fixJson(www.text));
            PutCompas(users);


        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }
    public void PutCompas(Usuario[] usuarios)
    {
        GameObject compasGO = GameObject.Find("Canvas/compas");
        Text compasText = compasGO.GetComponent<Text>();
        string compadres = "";

        for (int i =0; i< usuarios.Length; i++)
        {
            if (usuarios[i].id != user.id)
                compadres += usuarios[i].username + " \n";
        }
        compasText.text = compadres;

    }

    public void LeaveGroop()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        Debug.Log("cooks: " + Users.cookie);
        headers.Add("Cookie", "_session_id=" + Users.cookie);
        string url = "localhost:3000/groups/" + user.group_id + "/leaveGroup";
        WWW www = new WWW(url, null, headers);
        StartCoroutine(Leaves(www));
    }
    IEnumerator Leaves(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);


        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null){
            

            Debug.Log(www.text);
            user= JsonUtility.FromJson<Usuario>(www.text);
            string urlcompas = "localhost:3000/groups/" + user.group_id + " /compas";

            GameObject usuarioGO = GameObject.Find("Canvas/UserHeader");
            Text username = usuarioGO.GetComponent<Text>();
            username.text = "Usuario: " + user.username;

            WWW wwwCompas = new WWW(urlcompas, null, headers);
            StartCoroutine(GetCompas(wwwCompas));
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }

    public void UpdateUser()
    {
        GameObject userGO = GameObject.Find("Canvas/usuario");
        InputField username = userGO.GetComponent<InputField>();


        GameObject passwordGO = GameObject.Find("Canvas/contra");
        InputField password = passwordGO.GetComponent<InputField>();

        GameObject confirmGO = GameObject.Find("Canvas/confirma");
        InputField confirmation = confirmGO.GetComponent<InputField>();

        Debug.Log("username: " + username.text + "password: " + password.text + "confi: " + confirmation.text);

        if(username.text != "" || password.text != "")
        {
            string url = "localhost:3000/users/appEdit";
            StartCoroutine(UpUser(url, username.text, password.text));
        }
        else
        {

        }
        
    }

    IEnumerator UpUser(string url, string username, string password)
    {


        var data = new List<IMultipartFormSection> {
            new MultipartFormDataSection("id", user.id.ToString()),
            new MultipartFormDataSection("username", username),
            new MultipartFormDataSection("password", password),

    };


        UnityWebRequest www = UnityWebRequest.Post(url, data);
        yield return www.SendWebRequest();

        if (www.error == null)
        {
            SceneManager.LoadScene("02MainMenu");
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }

    public void Delet()
    {
        string url = "localhost:3000/logout";
        headers = new Dictionary<string, string>();
        Debug.Log("cooks: " + Users.cookie);
        headers.Add("Cookie", "_session_id=" + Users.cookie);
        WWW www = new WWW(url, null, headers);
        StartCoroutine(This(www));
    }
    IEnumerator This(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            SceneManager.LoadScene("00Splash");


        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }


    // Update is called once per frame
    void Update () {
		
	}
    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}

[Serializable]
public class Usuario
{
    public int id;
    public string username;
    public string password;
    public int group_id;
    public string created_at;
    public string updated_at;
}
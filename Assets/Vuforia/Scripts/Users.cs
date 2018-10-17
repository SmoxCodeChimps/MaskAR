using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Users : MonoBehaviour
{
    public static string cookie;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SignUp()
    {
        GameObject userGO = GameObject.Find("Canvas/username");
        InputField username = userGO.GetComponent<InputField>();

        GameObject emailGO = GameObject.Find("Canvas/email");
        InputField email = emailGO.GetComponent<InputField>();

        GameObject passwordGO = GameObject.Find("Canvas/password");
        InputField password = passwordGO.GetComponent<InputField>();

        GameObject confirmGO = GameObject.Find("Canvas/dup_password");
        InputField confirmation = confirmGO.GetComponent<InputField>();

        Debug.Log("username: " + username.text + "email: " + email.text + "password: " + password.text + "confi: " + confirmation.text);

        string url = "localhost:3000/users/appCreate";
        StartCoroutine(SignUpPost(url, username.text, email.text, password.text));
    }

    IEnumerator SignUpPost(string url, string username, string email, string password)
    {


        var data = new List<IMultipartFormSection> {

            new MultipartFormDataSection("username", username),
            new MultipartFormDataSection("email", email),
            new MultipartFormDataSection("password", password),
       
    };


        UnityWebRequest www = UnityWebRequest.Post(url, data);
        yield return www.SendWebRequest();

        if (www.error == null)
        {
            getcookie(www.GetResponseHeader("Set-Cookie"));
            SceneManager.LoadScene("02MainMenu");
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }

    public void Login()
    {

        GameObject userGO = GameObject.Find("Canvas/username");
        InputField username = userGO.GetComponent<InputField>();

        GameObject passwordGO = GameObject.Find("Canvas/password");
        InputField password = passwordGO.GetComponent<InputField>();

        
        string url = "localhost:3000/login";
        if(username.text != "" || password.text != "")
        {
            StartCoroutine(LoginPost(url, username.text, password.text));
        }
            
    }

    IEnumerator LoginPost(string url, string username, string password)
    {
        var data = new List<IMultipartFormSection> {

            new MultipartFormDataSection("username", username),
            new MultipartFormDataSection("password", password),
    };


        UnityWebRequest www = UnityWebRequest.Post(url, data);
        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("WWW OK!: " + www.GetResponseHeaders());

          
           if ( www.GetResponseHeaders().ContainsKey("Set-Cookie"))
            {
                getcookie(www.GetResponseHeader("Set-Cookie"));
                SceneManager.LoadScene("02MainMenu");
            }
           
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }


    }

    public void getcookie(string ses)
    {
        Debug.Log(ses);
        string[] temp = ses.Split(';');
        cookie = temp[0].Remove(0, 12);

        Debug.Log(cookie);
    }
}

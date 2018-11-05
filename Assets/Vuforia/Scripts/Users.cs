using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Users : MonoBehaviour
{
    Usuario user;
    public static string cookie;
    public static int userID;
    public static string baseurl = "http://18.216.168.174:3003/";
    Dictionary<string, string> headers;
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

      //  string url = "http://18.216.168.174:3003/users/appCreate";
        string url = baseurl+"users/appCreate";

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

        
        //string url = "http://18.216.168.174:3003/login";
        string url = baseurl+"login";
        if (username.text != "" || password.text != "")
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
                
                
            }
           
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }


    }
    public void getUserID()
    {
        string url = baseurl+"users/getUser";
        headers = new Dictionary<string, string>();
        Debug.Log("cooks: " + Users.cookie);
        headers.Add("Cookie", "_session_id=" + cookie);
        WWW www = new WWW(url, null, headers);
        StartCoroutine(wwwuserID(www));
    }
    IEnumerator wwwuserID(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {


            Debug.Log(www.text);
            user = JsonUtility.FromJson<Usuario>(www.text);
            userID = user.id;
            
            Debug.Log("USER ID"+user.id);
            SceneManager.LoadScene("02MainMenu");


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
        getUserID();

    }
}

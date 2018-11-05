using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinBehaviour : MonoBehaviour {
    public Button butt;
	// Use this for initialization
	void Start () {
      //  string url = "http://18.216.168.174:3003/groups/appIndex";
        string url = Users.baseurl + "groups/appIndex";

        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    public void CreateButtons(Group[] groups)
    {
        GameObject Image = GameObject.Find("Canvas/Panel/Image");
        Button[] buttons= new Button[groups.Length];
        for (int i = 0; i < groups.Length; i++)
        {
            buttons[i] = Instantiate(butt);
            buttons[i].GetComponentInChildren<Text>().text = groups[i].name;
            int temp = groups[i].id;
            buttons[i].onClick.AddListener(delegate { TaskOnClick(temp); });
            buttons[i].transform.SetParent(Image.transform, false);
        }
    }
    public void TaskOnClick(int name){
        Debug.Log(name);
        JoinGroop(name);
    }

    public void JoinGroop(int id)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        Debug.Log("cooks: " + Users.cookie);
        headers.Add("Cookie", "_session_id=" + Users.cookie);
      //  string url = "http://18.216.168.174:3003/groups/" + id + "/join";
        string url = Users.baseurl + "groups/" + id + "/join";

        WWW www = new WWW(url,null,headers);
        StartCoroutine(Joins(www));
    }
    IEnumerator Joins(WWW www)
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
        if (www.error == null)
        {
            Debug.Log(www.text);
            Group[] groups = JsonHelper.FromJson<Group>(fixJson(www.text));
            CreateButtons(groups);
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
public class Group
{
    public int id;
    public string name;
    public string tracker_name;
    public string tracker_id;
    public string created_at;
    public string updated_at;
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
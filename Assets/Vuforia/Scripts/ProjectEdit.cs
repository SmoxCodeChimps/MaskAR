using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProjectEdit : MonoBehaviour {
    public string FileName = "C:\\popu.png";
    public string Groupid;
    // Use this for initialization
    void Start () {
        //string url = "http://18.216.168.174:3003/user/groop";
        string url = Users.baseurl + "users/groop";
        Dictionary<string, string> headers = new Dictionary<string, string>();
        Debug.Log("cooks: " + Users.cookie);
        headers.Add("Cookie", "_session_id=" + Users.cookie);
        WWW www = new WWW(url, null, headers);
        StartCoroutine(GetGroupId(www));

    }
    IEnumerator GetGroupId(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Groupid = www.text;
            GetGroup(www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }
    public void GetGroup(string id)
    {
        //string url = "http://18.216.168.174:3003/groups/"+id;
        string url = Users.baseurl + "groups/" + id;

        WWW www = new WWW(url);
        StartCoroutine(GetGroup(www));
    }
    IEnumerator GetGroup(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            Group group =JsonUtility.FromJson<Group>(www.text);
            LoadGroupToScene(group);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }
    public void LoadGroupToScene(Group g)
    {
        GameObject header = GameObject.Find("Canvas/Text");
        header.GetComponent<Text>().text = g.name;
        DoImages(g.tracker_name,"Canvas/Tracker/Loader","/tracker");
        DoImages(g.tracker_name, "Canvas/Portrait/Loader", "/portrait");


    }

    public void DoImages(string name,string dir,string img)
    {

        //string url = "http://18.216.168.174:3003/groups/" + name + img;
        string url = Users.baseurl + "groups/" + name + img;
        WWW www = new WWW(url);
        StartCoroutine(coTracker(www,dir));
    }
    
    IEnumerator coTracker(WWW www,string dir)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            StartCoroutine(ImageTracker(www.text,dir));

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }

    IEnumerator ImageTracker(string text,string dir)
    {
        // Start a download of the given URL
        using (WWW www = new WWW(text))
        {
            // Wait for download to complete
            yield return www;
            // assign texture
            changeTracker(www.texture,dir);
        }
    }
    public void changeTracker(Texture2D tex,string dir)    {
        GameObject portrait = GameObject.Find(dir);
        RawImage image = portrait.GetComponent<RawImage>();
        Debug.Log("portrait color: " + image.color);
        image.texture = tex;

        Debug.Log("From button: " + image.texture);

    }
    public static Texture2D setImage(int maxSize)
    {
        Texture2D texture = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {
            Debug.Log("Image path: " + path);
            PlayerPrefs.SetString("ARContentImagepath", path);
            PlayerPrefs.SetInt("FirstTimer", 1);
            if (path != null)
            {
                // Create Texture from selected image
                texture = NativeGallery.LoadImageAtPath(PlayerPrefs.GetString("ARContentImagepath"), maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

            }
        }, "Selecciona una imagen PNG", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
        return texture;
    }
    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            tex.name = filePath;
        }
        return tex;
    }


    public void ChangeTrackerImage() {
        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = setImage(1000); //FOR MObILE DEVICES
        changeTracker(tex, "Canvas/Tracker/Loader");
      

    }
    public void ChangePortraitImage()
    {
        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = setImage(1000); //FOR MObILE DEVICES
        changeTracker(tex, "Canvas/Portrait/Loader");
    }
    public void updateGroup(){
       
        //string url = "http://18.216.168.174:3003/groups/appEdit";
        string url = Users.baseurl + "groups/appEdit";

        GameObject portrait = GameObject.Find("Canvas/Portrait/Loader");
        RawImage imagePortrait = portrait.GetComponent<RawImage>();
        GameObject tracker = GameObject.Find("Canvas/Tracker/Loader");
        RawImage imageTracker = tracker.GetComponent<RawImage>();
        Texture2D marco = (Texture2D)imagePortrait.texture;
        Texture2D target = (Texture2D)imageTracker.texture;
        StartCoroutine(UpdateCoroutine(url,  Groupid,  target,  marco));
    }
    IEnumerator UpdateCoroutine(string url, string Groupid, Texture2D target, Texture2D marco){
       

        var data = new List<IMultipartFormSection> {

            new MultipartFormDataSection("id", Groupid),
            new MultipartFormFileSection("tracker", target.EncodeToJPG(), "tracker.jpg", "image/jpg"),
            new MultipartFormFileSection("portrait", marco.EncodeToPNG(), "portrait.png", "image/png")

    };


        UnityWebRequest www = UnityWebRequest.Post(url, data);
        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("WWW OK!: " + www.responseCode);
           



        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }

 

    // Update is called once per frame
    void Update () {
		
	}
}

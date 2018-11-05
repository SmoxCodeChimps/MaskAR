using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Groups : MonoBehaviour {
    public string FileName = "C:\\popu.png";
    Texture2D image;
    static byte[] frame;
    static byte[] tracker;
  
    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

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

                tracker = NativeGallery.getBytesAtPath(PlayerPrefs.GetString("ARContentImagepath"), maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

            }
        }, "Selecciona una imagen", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
        return texture;
    }

    public static Texture2D setFrame(int maxSize)
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
                frame = NativeGallery.getBytesAtPath(PlayerPrefs.GetString("ARContentImagepath"), maxSize);


                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

            }
        }, "Selecciona una imagen", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
        return texture;
    }
    public void ChangeTrackerImage()
    {
        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = setImage(9000); //FOR MObILE DEVICES
        changeTracker(tex, "Canvas/Tracker/Loader");


    }
    public void ChangePortraitImage()
    {
        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = setFrame(9000); //FOR MObILE DEVICES
        GameObject header = GameObject.Find("Canvas/Text");
        header.GetComponent<Text>().text = frame.ToString();

        changeTracker(tex, "Canvas/Portrait/Loader");
    }
    public void changeTracker(Texture2D tex, string dir)
    {
        string temp;
        GameObject header = GameObject.Find("Canvas/Text");
        if (tex == null) temp = "tex is null desu";
        else temp = tex.height.ToString();
        header.GetComponent<Text>().text = temp;

        GameObject portrait = GameObject.Find(dir);
        RawImage image = portrait.GetComponent<RawImage>();
        Debug.Log("portrait color: " + image.color);
        image.texture = tex;

        Debug.Log("From button: " + image.texture);

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
    public static byte[] hailmary(string filePath){
        byte[] fileData=null;
        if (File.Exists(filePath))fileData = File.ReadAllBytes(filePath);
        return fileData;


        }
    public void CreateGroup(){
        Debug.Log("EnteredGrup");
        //string url = "http://18.216.168.174:3003/groups/appCreate";
        string url = Users.baseurl + "groups/appCreate";
        /*
        GameObject portrait = GameObject.Find("Canvas/Portrait/Loader");
        RawImage imagePortrait = portrait.GetComponent<RawImage>();
        
        GameObject tracker = GameObject.Find("Canvas/Tracker/Loader");
        RawImage imageTracker = tracker.GetComponent<RawImage>();
        

        Texture2D marco = (Texture2D)imagePortrait.texture;
        Texture2D target = (Texture2D)imageTracker.texture;

        
      */

        GameObject nombreGO = GameObject.Find("Canvas/groupname");
        InputField grup = nombreGO.GetComponent<InputField>();

        string grupo = grup.text;
        StartCoroutine(CreateCoroutine(url, grupo,tracker,frame));
       
    }
    IEnumerator CreateCoroutine(string url, string grupo,byte[] target, byte[] marco)
    {
        GameObject header = GameObject.Find("Canvas/Text");
        header.GetComponent<Text>().text = "Inicio corutina";
       
        header.GetComponent<Text>().text = tracker.ToString();
        
        var data = new List<IMultipartFormSection> {

            new MultipartFormDataSection("name", grupo),

            new MultipartFormDataSection("user_id",Users.userID.ToString()),
            new MultipartFormDataSection("tracker_name", grupo),
            new MultipartFormFileSection("tracker", target, "tracker.jpg", "image/jpg"),
            new MultipartFormFileSection("portrait", marco, "portrait.png", "image/png")
            

    };
        header.GetComponent<Text>().text = "Datos listos";


        UnityWebRequest www = UnityWebRequest.Post(url, data);
        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("WWW OK!: " + www.responseCode);
            header.GetComponent<Text>().text = "Grupo subido";

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
            header.GetComponent<Text>().text = "Grupo no creado";
        }

    }
   


}

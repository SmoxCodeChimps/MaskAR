using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Groups : MonoBehaviour {
    public string FileName = "C:\\popu.png";
    Texture2D image;



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
        Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        //Texture2D tex = setImage(1000); //FOR MObILE DEVICES
        changeTracker(tex, "Canvas/Tracker/Loader");


    }
    public void ChangePortraitImage()
    {
        Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        //Texture2D tex = setImage(1000); //FOR MObILE DEVICES
        changeTracker(tex, "Canvas/Portrait/Loader");
    }
    public void changeTracker(Texture2D tex, string dir)
    {
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

        string url = "localhost:3000/groups/appCreate";
        GameObject portrait = GameObject.Find("Canvas/Portrait/Loader");
        RawImage imagePortrait = portrait.GetComponent<RawImage>();

        GameObject tracker = GameObject.Find("Canvas/Tracker/Loader");
        RawImage imageTracker = portrait.GetComponent<RawImage>();

        Texture2D marco = (Texture2D)imagePortrait.texture;
        Texture2D target = (Texture2D)imageTracker.texture;

        GameObject nombreGO = GameObject.Find("Canvas/groupname");
        InputField grup = nombreGO.GetComponent<InputField>();

        string grupo = grup.text;

        StartCoroutine(CreateCoroutine(url, grupo, target, marco));
    }
    IEnumerator CreateCoroutine(string url, string grupo, Texture2D target, Texture2D marco)
    {


        var data = new List<IMultipartFormSection> {

            new MultipartFormDataSection("name", grupo),
            new MultipartFormDataSection("tracker_name", grupo),
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








}

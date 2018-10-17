using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class ChangeTex : MonoBehaviour {
    public string FileName = "C:\\popu.png";
    public GameObject portrait;
    public Texture2D byebye;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static Texture2D LoadPNG(string filePath){

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath)){
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            tex.name = filePath;
        }
        return tex;
    }

    public static Texture2D setImage(int maxSize){
        Texture2D texture = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>{
            Debug.Log("Image path: " + path);
            PlayerPrefs.SetString("ARContentImagepath", path);
            PlayerPrefs.SetInt("FirstTimer", 1);
            if (path != null)
            {
                // Create Texture from selected image
                 texture = NativeGallery.LoadImageAtPath(PlayerPrefs.GetString("ARContentImagepath"), maxSize);
                if (texture == null){
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
               
            }
        }, "Selecciona una imagen PNG", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
        return texture;
    }
    public static Texture2D setTheImage(int maxSize){
        Texture2D texture = null;
        /*NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {
            Debug.Log("Image path: " + path);
            PlayerPrefs.SetString("ARContentImagepath", path);
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
        */
        if (PlayerPrefs.GetString("ARContentImagepath") != null)
        {
            texture = NativeGallery.LoadImageAtPath(PlayerPrefs.GetString("ARContentImagepath"), maxSize);
        }
        return texture;
    }


    public void Change(){
        GameObject portrait = GameObject.Find("Canvas/Portrato");
       
        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = setTheImage(1000); //FOR MObILE DEVICES
        /*
        Renderer rend = portrait.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Transparent/VertexLit with Z");
        Vector3 angles = rend.transform.localEulerAngles;
        angles.y = 180;
        rend.transform.localEulerAngles = angles;
        rend.material.mainTexture = tex;
        
        Debug.Log(portrait.name);
        */
        RawImage image = portrait.GetComponent<RawImage>();
        //image.color = new Color(1F, 1F, 1F, 1); //Changes visibility
        Material mat = image.material;
        Debug.Log("portrait color: " +image.color);
        image.texture = tex;
        //image.color = new Color(1F, 1F, 1F, 0F);
        //mat.color = new Color(1F, 1F, 1F, 1F);
        Debug.Log("Material: " + mat.color);
        mat.mainTexture = tex;
        Debug.Log("From button: "+ image.texture);
       
        //mat.mainTexture = tex;

    }


    public void newChange(Texture2D tex){
        GameObject portrait = GameObject.Find("Canvas/Portrato");
        RawImage image = portrait.GetComponent<RawImage>();
        //image.color = new Color(1F, 1F, 1F, 1); //Changes visibility
        Material mat = image.material;
        Debug.Log("portrait color: " + image.color);
        image.texture = tex;
        //image.color = new Color(1F, 1F, 1F, 0F);
        //mat.color = new Color(1F, 1F, 1F, 1F);
        Debug.Log("Material: " + mat.color);
        mat.mainTexture = tex;
        Debug.Log("From button: " + image.texture);

        //mat.mainTexture = tex;


    }

    public void ImgLoad(){
        GameObject portrait = GameObject.Find("Canvas/CustomImage/Loader");

        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = setImage(1000); //FOR MObILE DEVICES
        /*
        Renderer rend = portrait.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Transparent/VertexLit with Z");
        Vector3 angles = rend.transform.localEulerAngles;
        angles.y = 180;
        rend.transform.localEulerAngles = angles;
        rend.material.mainTexture = tex;
        
        Debug.Log(portrait.name);
        */
        RawImage image = portrait.GetComponent<RawImage>();
        //image.color = new Color(1F, 1F, 1F, 1); //Changes visibility
        Material mat = image.material;
        Debug.Log("portrait color: " + image.color);
        image.texture = tex;
        //image.color = new Color(1F, 1F, 1F, 0F);
        //mat.color = new Color(1F, 1F, 1F, 1F);
        Debug.Log("Material: " + mat.color);
        mat.mainTexture = tex;
        Debug.Log("From button: " + image.texture);

        //mat.mainTexture = tex;

    }
    public void DoYouEvenNullBrah()
    {
        GameObject portrait = GameObject.Find("Canvas/Portrato");

        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = byebye; //FOR MObILE DEVICES
        /*
        Renderer rend = portrait.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Transparent/VertexLit with Z");
        Vector3 angles = rend.transform.localEulerAngles;
        angles.y = 180;
        rend.transform.localEulerAngles = angles;
        rend.material.mainTexture = tex;
        
        Debug.Log(portrait.name);
        */
        RawImage image = portrait.GetComponent<RawImage>();
        //image.color = new Color(1F, 1F, 1F, 1); //Changes visibility
        Material mat = image.material;
        Debug.Log("portrait color: " + image.color);
        image.texture = tex;
        //image.color = new Color(1F, 1F, 1F, 0F);
        //mat.color = new Color(1F, 1F, 1F, 1F);
        Debug.Log("Material: " + mat.color);
        mat.mainTexture = tex;
        Debug.Log("From button: " + image.texture);

        //mat.mainTexture = tex;

    }
    public void Preview()
    {
        GameObject portrait = GameObject.Find("Canvas/CustomImage");
        if (PlayerPrefs.GetInt("FirstTimer") != 1)
        {
            Texture2D tex = byebye;
            RawImage image = portrait.GetComponent<RawImage>();
            Material mat = image.material;
            Debug.Log("portrait color: " + image.color);
            image.texture = tex;
            Debug.Log("Material: " + mat.color);
            mat.mainTexture = tex;
            Debug.Log("From button: " + image.texture);
        }
        else if (PlayerPrefs.GetInt("FirstTimer") == 1)
        {
            Texture2D tex = setImage(1000); //FOR MObILE DEVICES
            RawImage image = portrait.GetComponent<RawImage>();
            Material mat = image.material;
            Debug.Log("portrait color: " + image.color);
            image.texture = tex;
            Debug.Log("Material: " + mat.color);
            mat.mainTexture = tex;
            Debug.Log("From button: " + image.texture);
        }
    }

    public void showImage(string name){
      
        string url = "localhost:3000/groups/"+name +"/portrait";
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            StartCoroutine(getimage(www.text));
            
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
       
    }

    IEnumerator getimage(string text)
    {
        // Start a download of the given URL
        using (WWW www = new WWW(text))
        {
            // Wait for download to complete
            yield return www;
            // assign texture
            newChange(www.texture);
        }
    }
}

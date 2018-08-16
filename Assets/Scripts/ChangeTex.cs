using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTex : MonoBehaviour {
    public string FileName = "C:\\popu.png";
    public GameObject portrait;
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

    public static Texture2D getImage(int maxSize){
        Texture2D texture = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>{
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                 texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null){
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
               
            }
        }, "Selecciona una imagen PNG", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
        return texture;
    }
    public void Change(){
        GameObject portrait = GameObject.Find("Canvas/Portrato");
       
        //Texture2D tex = LoadPNG(FileName); // FOR TEST IN PC
        Texture2D tex = getImage(1000); //FOR MObILE DEVICES

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
}

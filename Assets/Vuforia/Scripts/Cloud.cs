using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class Cloud : MonoBehaviour, ICloudRecoEventHandler{

    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    private GameObject portrait;
    private RawImage image;


    // Use this for initialization
    void Start () {
        portrait = GameObject.Find("Canvas/Portrato");
        image = portrait.GetComponent<RawImage>();
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (mCloudRecoBehaviour){
            mCloudRecoBehaviour.RegisterEventHandler(this);
        }
    }
    public void OnInitialized(){
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError){
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError){
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }
    // Update is called once per frame
    public void OnStateChanged(bool scanning){
        mIsScanning = scanning;
        if (scanning){
            // clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.TargetFinder.ClearTrackables(false);

        }
    }
    void OnGUI(){
       
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Escaneando" : "No escaneando");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Nombre: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Vuelve a Escanear")){
                // Restart TargetFinder
                mCloudRecoBehaviour.CloudRecoEnabled = true;
                image.color = new Color(1F, 1F, 1F, 0);
            }
            

        }
        
    }
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult){
        // do something with the target metadata
        mTargetMetadata = targetSearchResult.TargetName;

        ChangeTex tex = GameObject.Find("Button").AddComponent<ChangeTex>();
        tex.showImage(mTargetMetadata);
        // stop the target finder (i.e. stop scanning the cloud)
        if (image.texture.Equals(null)){
            image.color = new Color(1F, 1F, 1F, 0.5F);
        }
        else{
            image.color = new Color(1F, 1F, 1F, 1F);
        }
        mCloudRecoBehaviour.CloudRecoEnabled = false;
        
    }
    void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotNoUi : MonoBehaviour
{

    public void OnClickScreenCaptureButton()
    {
        StartCoroutine(CaptureScreen());
    }
    public IEnumerator CaptureScreen()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;
        GameObject.Find("MyCanvas").GetComponent<Canvas>().enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Take screenshot
        ScreenCapture.CaptureScreenshot("AR4U_Screenshot.png");

        // Show UI after we're done
        GameObject.Find("MyCanvas").GetComponent<Canvas>().enabled = true;
    }
}
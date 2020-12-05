using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangePreviewImage : MonoBehaviour
{
    static int initupdate = 0;
    private GameObject image;
    private GameObject previewPanel;

    //When a MastCam sol is clicked, it will turn the preview window on (on bottom
    //right)
    public void updatePreview(){
        //Make sure PreviewImage is tagged with MCPI (MastCam Preview Image)
        image = GameObject.FindWithTag("MCPI");

        //Get the raw image texture from the button's parent. This script is
        //attached to the button of the sol in the observations maenu, and the
        //parent is the Sol object, with a RawImage associated.
        var tex = gameObject.transform.parent.gameObject.GetComponent<RawImage>().texture;

        //Update text and preveiw image
        GameObject.FindWithTag("MCText").GetComponent<TMP_Text>().text = "Product: " + gameObject.transform.parent.gameObject.name;
        image.GetComponent<RawImage>().texture = tex;
    }

}

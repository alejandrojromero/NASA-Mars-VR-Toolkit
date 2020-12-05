using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class SequenceImageLoader : MonoBehaviour
{
    private string ImageURL;
    public string productID;
    //Access the dictionary of csv data
    public Main csv;
    public List<Dictionary<string,object>> data;

    public float progress;

    // Start is called before the first frame update
    void Start()
    {
        csv = Camera.main.GetComponent<Main>(); //Access the main script where the dictionary was created
        data = csv.dict; //Get the dictionary
        productID = gameObject.name; //Get the product ID based off of the current sol mesh's name
        ImageURL = "https://an.rsl.wustl.edu/msl/mslbrowser/imDisp.aspx?m=1&it=B1&ii=" + productID + "&is=IsiBrowse&st=0"; //Change URL based on productID
        StartCoroutine (LoadImage(ImageURL)); //Fetch the corresponding image from the server at the determined URL
    }

    public IEnumerator LoadImage(string uri){
        // Request and wait for the desired page.
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri);
        var asyncOperation = webRequest.SendWebRequest();

        //Check progress of request
        while(!webRequest.isDone){
            progress = asyncOperation.progress;
            yield return null;
        }

        progress = 1f;

        //Create texture from web image and assign it to plane
        var texture = DownloadHandlerTexture.GetContent(webRequest);
        gameObject.GetComponent<RawImage>().texture = texture;
    }

    //Resets the sol name from its number, ex. "1098" to "Sol", which is
    //necessary for other scripts to generalize to objects with the name "Sol"
    public void overwriteName(){
        gameObject.name = "Sol";
    }

    //Reload the corresponding sol thumbnail image. Called when going back through menus
    //the user has already been to
    public void reloadImage(){
        StartCoroutine(LoadImage("https://an.rsl.wustl.edu/msl/mslbrowser/imDisp.aspx?m=1&it=B1&ii=" + gameObject.name + "&is=IsiBrowse&st=0"));
    }
}

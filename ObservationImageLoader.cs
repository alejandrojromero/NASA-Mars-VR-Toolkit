using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ObservationImageLoader : MonoBehaviour
{
    //Variables for storing imageURL and product ID
    private string ImageURL;
    public string productID;

    //Access the dictionary of csv data
    public Main csv; 
    public List<Dictionary<string,object>> data;

    public float progress; //Needed to keep track of progress from web request

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

        //Create texture from web image and assign it to RawImage on the label thumbnail
        var texture = DownloadHandlerTexture.GetContent(webRequest);
        gameObject.GetComponent<RawImage>().texture = texture;
    }

    //Reset the name of the header to "Sol". It will be reset to a data value later on. Since this object is recylced, we need
    //to make sure it is reset back to its original name to avoid naming errors
    public void overwriteName(){
        gameObject.name = "Sol";
    }

    //Gets the image with the new product ID when necessary
    public void reloadImage(){
        StartCoroutine(LoadImage("https://an.rsl.wustl.edu/msl/mslbrowser/imDisp.aspx?m=1&it=B1&ii=" + gameObject.name + "&is=IsiBrowse&st=0"));
    }
}

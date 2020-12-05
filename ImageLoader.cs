using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ImageLoader : MonoBehaviour
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
        productID = getProductID(gameObject.name); //Get the product ID based off of the current sol mesh's name
        ImageURL = "https://an.rsl.wustl.edu/msl/mslbrowser/imDisp.aspx?m=1&it=B1&ii="
          + productID + "&is=IsiBrowse&st=0"; //Change URL based on productID
        StartCoroutine (LoadImage(ImageURL)); //Fetch the corresponding image from the server at the determined URL

        //Camera.main.GetComponent<GenerateSolList>().showProdID(productID);
    }

    IEnumerator LoadImage(string uri){
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


    private string getProductID(string name){

        //Look through dictionary for row with correct instrument and sol
        //If found, return corresponding product ID to feed into URL
        for(var i=0; i < data.Count; i++) {
            if(data[i]["sol"].ToString() == name && data[i]["target"].ToString() == "MARS"){
                //return the product ID
                return (data[i]["productID"]).ToString();
            }
        }
        //Should never get here
        return (data[5]["productID"]).ToString();
    }
}

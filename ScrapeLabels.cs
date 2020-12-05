using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class ScrapeLabels : MonoBehaviour
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
        data = csv.dict; //Get the dictionary with all MastCam data. Thsi was created in the Main.cs script

        //For eb
        for(var i=0; i < data.Count; i++){
            //Method for getting product labels. The methods are as below
            //For product labels: https://an.rsl.wustl.edu/msl/mslbrowser/downloadFile.aspx?it=LF&ii=<productId>
            //For data products: https://an.rsl.wustl.edu/msl/mslbrowser/downloadFile.aspx?it=PR&ii=<productId>
            StartCoroutine(LoadText("https://an.rsl.wustl.edu/msl/mslbrowser/downloadFile.aspx?it=LF&ii="
            + (data[i]["productId"]).ToString(), i));
        }
    }

    IEnumerator LoadText(string uri, int i){
        // Request and wait for the desired page based on the webpage specified
        //above when calling LoadText()
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        //Queries internet for data associated with the url
        var asyncOperation = webRequest.SendWebRequest();

        //Check progress of request
        while(!webRequest.isDone){
            progress = asyncOperation.progress;
        }
        //Set up data path where we want to save PDS Labels. In this case, they
        //save to the app dta path in a folder called "Labels" and follow the
        //naming convention [PRODUCT ID].txt
        string path = Application.dataPath + "/Labels/" + (data[i]["productId"]).ToString() + ".txt";

        //Write the PDS label at the web address to the text file we created above
        File.WriteAllText(path, webRequest.downloadHandler.text);
        yield return null;

        //Update progress to 1 as the operation is completed
        progress = 1f;
    }
}

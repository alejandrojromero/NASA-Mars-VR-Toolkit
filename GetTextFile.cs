 using UnityEngine;
 using System.Collections;
 using UnityEngine.UI;
 using UnityEngine.Events;
 using UnityEngine.Networking;
 using System;
 using TMPro;


 /*
THIS SCRIPT IS A TEST FOR FUNCTIONALITY THAT IS USED IN ImageLoader.cs and
ScrapeLabels.cs. USE THEM INSTEAD. 
 */
 public class GetTextFile : MonoBehaviour {

     private string WebsiteWithTextUrl = "https://an.rsl.wustl.edu/msl/mslbrowser/an3.aspx";
     public Text webUrlText;
     // Use this for initialization
     void Start () {
         StartCoroutine (GetRequest(WebsiteWithTextUrl));
     }
     IEnumerator GetRequest(string uri){
         using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " +  webRequest.downloadHandler.text);

                foreach(Transform child in GameObject.Find("Labels").transform){
                    child.GetComponent<TextMeshPro>().text = webRequest.downloadHandler.text;
                }
            }
        }
     }
 }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//***********************************//
// THIS SCRIPT ATTACHED TO VR CAMERA //

public class GenerateSolList : MonoBehaviour
{
    //START AND END SOLS. IN THIS CASE, WE DEAL WITH SOLS 1098-1100 INCLUSIVE.
    //CHANGE THESE VALUES TO MIRROR YOUR DESIRED RANGE OF SOLS.
    private int start = 1098;
    private int end = 1100;
    private GameObject template;
    private GameObject clone;
    int count = 0;

    // Start is called before the first frame update
    void Awake()
    {
        //Template is a blueprint for a button that we will duplicate as many
        //times as needed, depending on how many datapoints there are. We will
        //generate one button per sol, which will be a clone of the template
        template = GameObject.Find("ListItemTemplate");

        //For the range of sols, create a button for each one.
        for(int i = start; i < end + 1; i++){
            //If on first sol in the range:
            if(i == start){
                //Display "Sol: XXXX" in the GUI, where XXXX is the current sol number (ex 1098)
                template.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = "Sol " + start.ToString();
                template.transform.Find("Sol").name = start.ToString(); //set the Sol name to the Sol number (ex. 1098)
            }
            //For all other sols after the first of the range, instantiate a clone of the Template
            //and alter its
            else{
                //Create clone of template
                GameObject clone = Instantiate(template);
                //Set the clone's parent to the template's same parent so that
                //it is in same place in Unity hierarchy
                clone.transform.SetParent(template.transform.parent, false);
                //Set the cloned button to be below the previous one by an offset of 0.46f along y axis
                clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y - (count * 0.46f), clone.transform.position.z);
                //Display "Sol: XXXX" in the GUI, where XXXX is the current sol number (ex 1098)
                clone.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = "Sol " + i.ToString();
                clone.transform.Find(start.ToString()).name = i.ToString(); //set the Sol name to the Sol number (ex. 1098)
            }
            count++;   //Track number of sols in GUI
        }
        //Maniplations are done, so make menu inactive in the GUI
        GameObject.Find("3.6 MastCam Scroll").SetActive(false);
    }

    //Start is called after Awake() on load
    void Start()
     {
         //Call LateStart after a short delay to ensure previous operations
         //are complete
         StartCoroutine(LateStart(0.5f));
     }

     //Coroutine for showing the productID in the GUI next to the image preview
     IEnumerator LateStart(float waitTime)
     {
         yield return new WaitForSeconds(waitTime);
         // Show product ID in list as text
         GameObject[] list = GameObject.FindGameObjectsWithTag("MCListObj");
         foreach (GameObject go in list) {
            go.transform.Find("prodID").GetComponent<TMP_Text>().text = "Product ID: " + go.transform.GetComponentInChildren<ImageLoader>().productID;
        }
     }

}

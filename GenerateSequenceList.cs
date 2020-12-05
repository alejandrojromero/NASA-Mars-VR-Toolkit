using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


//***********************************//
// SCRIPT ATTACHED TO VR CAMERA //
// SCRIPT CALLED WHEN SOL LIST ITEM IS CLICKED //

public class GenerateSequenceList : MonoBehaviour
{
    //Create variables
    private GameObject template;
    private GameObject clone;
    private GameObject sequenceUI;
    int count = 0;
    int j = 0;
    int g = 0;
    public Main csv;
    public List<Dictionary<string,object>> data;


    //Create a dictionary of lists. These lists will each be one sequence with the
    //respective sequence number as the key. See Dictionary data structure documentation
    //to understand key/value relationship in dictionaries, as this script relies
    //heavily on it. https://learn.unity.com/tutorial/lists-and-dictionaries
    public Dictionary<string,List<string>> sequenceList;

    public string sol = "SOL"; //Sol string used for parsing

    // Awake is called before the first frame update
    public void Awake(){
        sequenceUI = GameObject.Find("3.61 MastCam Sequences"); //Save menu in variable so we can access it
        GameObject.Find("3.61 MastCam Sequences").SetActive(false); //Turn off the menu in the GUI
    }

    //Method called when clicking on sol and viewing corresponding sequence of
    //MastCam images
    public void ChangeSequence(string sol)
    {
        template = GameObject.Find("SequenceTemplate");
        csv = Camera.main.GetComponent<Main>(); //Access the main script where the dictionary was created
        data = csv.dict; //Get the dictionary from the MastCam csv
        //Dictonary that keeps track if we have seen sequence number yet
        //by storing sequence numbers dynamically
        Dictionary<int,string> sequenceNumbers = new Dictionary<int,string>();

        //Dictionary that contains all sequence numbers and their corresponding sequences
        //The hierarchy is: sequenceList contains all sequences, with the key being the sequenceNumber
        //and the value being the sequence of ProductIDs. The sequenceNumbers dictionary is only used
        //to keep track of which sequence numbers we have found in the CSV file, so we can determine
        //where to add the corresponding productIDs.
        sequenceList = new Dictionary<string,List<string>>();

        for(var i = 0; i < data.Count; i++){
            //Only consider items from the sol the user clicked on. That is, only look at
            //data corresponding to a sol with the sol number we are looking at.
            if(data[i]["sol"].ToString() == sol){
                //If the dictionary does not have the current sequence #, then it hasn't
                //come across it in the lookup table CSV yet.
                //Add this sequence number to the dictionary.
                string sequenceNum = data[i]["sequence"].ToString();
                if(sequenceNumbers.ContainsValue(sequenceNum) == false){
                    sequenceNumbers.Add(j, sequenceNum); //Add sequence number to sequenceNumbers dictionary
                    j++; //Increment j
                    //Create a sequence that will hold all of the ProductIDs for a given sequnceNumber.
                    //For example, the sequenceNumber 1000 can hold a lot of ProductIDs within its list, since they
                    //all share the same sequence number
                    List<string> sequence = new List<string>();
                    sequence.Add(data[i]["productID"].ToString()); //Add productID to the sequence
                    //Add the list to the dictionary (sequenceList)
                    sequenceList.Add(sequenceNum, sequence);

                }
                //If we have already seen the sequence number, then that means its list already
                //exists in the super list of sequence numbers (sequenceList). Therefore, we can add
                else if(sequenceNumbers.ContainsValue(sequenceNum)){
                    //If we have already created the list in the dictionary, just add to that list
                    sequenceList[sequenceNum].Add(data[i]["productID"].ToString());
                }
            }
        }

        //Create a temporary list that we will use to generate buttons in the GUI
        List<string> temp = new List<string>();
        //For each unique sequence numbers that we found in the CSV, we want to generate
        //a button that tells the user how many images are in the corresponding sequence
        for(int i = 0; i < sequenceNumbers.Count; i++){
            //On each iteration, temp stores all of the product IDs stored in GenerateSequenceList
            //for a given sequence number. Remember that the elemts of sequenceNumbers[] are in the format
            //XXXX, and are keys to the actual sequences in SequenceList, which are the corresponding lsist of productIDs
            temp = sequenceList[sequenceNumbers[i]];

            //If this is the first sequence button we are creating, use the initial template and change its text
            if(i == 0){
                //Change label to "Sequence:" + the sequence number
                template.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = "Sequence " + sequenceNumbers[0].ToString();
                //Label which sol the sequence corresponds to
                template.transform.Find("prodID").GetComponent<TMP_Text>().text = "Sol " + sol;
                //Show how many images there are in the sequence. This is simply a matter of displaying the size
                //of the list of productIDs in the corresponding sequence.
                template.transform.Find("numImages").GetComponent<TMP_Text>().text = temp.Count + " images in sequence > ";

            }
            //If on any sequence other than the first, make a clone of the template
            else{
                GameObject clone = Instantiate(template); //make clone
                clone.transform.SetParent(template.transform.parent, false); //set same parent as template in hierarchy
                //Set the cloned button to be below the previous one by an offset of 0.46f along y axis
                clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y - (count * 0.46f), clone.transform.position.z);
                //Change label to "Sequence:" + the sequence number
                clone.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = "Sequence " + sequenceNumbers[i].ToString();
                  //Label which sol the sequence corresponds to
                clone.transform.Find("prodID").GetComponent<TMP_Text>().text = "Sol " + sol;
                //Show how many images there are in the sequence. This is simply a matter of displaying the size
                //of the list of productIDs in the corresponding sequence.
                clone.transform.Find("numImages").GetComponent<TMP_Text>().text = temp.Count + " images in sequence > ";
            }
        }

        //Set each newly cloned gameObject's "Sol" gameobject to be named by the
        //actual productID. This is necessary to generalize to other scripts so
        //that they may recognize this gameobject and manipulate variables accordingly.
        GameObject[] sequenceItems = new GameObject[temp.Count]; //Create array that's same size as temp
        sequenceItems = GameObject.FindGameObjectsWithTag("MCSequence"); //Insert all gameobjects with mastcam sequence tag
        foreach (GameObject si in sequenceItems)
        {
            temp = sequenceList[sequenceNumbers[g]];
            sequenceItems[g].transform.Find("Sol").name = temp[0].ToString();
            g++;
        }
    }

    //When going back in menus, we don't want to keep all of these lists filled,
    //since we may have different sols with different sequences. Clear the lists and variables,
    //and destroy all of the cloned objects we have created above.
    public void clearLists(){
        sequenceUI.SetActive(true);
        count = 0;
        j = 0;
        g = 0;
        //Set the name of all gameobjects with the mactcamsequence tag to "Sol" to generalize
        GameObject.FindWithTag("MCSequenceNum").GetComponent<SequenceImageLoader>().overwriteName();
        sequenceList.Clear(); //clear SequenceList so it may be reused and not added to, which would cause errors
        //Get all objects with a certain name and destroy them
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "SequenceTemplate(Clone)");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        //Turn off the sequence menu
        sequenceUI.SetActive(false);
    }

}

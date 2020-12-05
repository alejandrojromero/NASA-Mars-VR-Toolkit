using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GenerateObservationsList : MonoBehaviour
{
    public Main csv;
    public List<Dictionary<string,object>> data;
    public string sequence;
    public string sol;
    private GameObject template;
    public GenerateSequenceList generator;
    int count = 0;

    //Make a list that will hold all observations we want to show in the menu
    public List<string> observations;

    public void GetObservations(){
        csv = Camera.main.GetComponent<Main>(); //Access the main script where the dictionary was created
        data = csv.dict; //Get the dictionary from the MastCam csv
        template = GameObject.Find("ObservationTemplate");
        //Create variable where we can store the script that created our sequenceList, which already
        //contains all of the sequences and corresponding productIDs.
        generator = Camera.main.GetComponent<GenerateSequenceList>();

        //Get the correct sequence and sol values from the CSV
        for(var i = 0; i < data.Count; i++){
            if(data[i]["productID"].ToString() == gameObject.name){
               sequence = data[i]["sequence"].ToString();
               sol = data[i]["sol"].ToString();
               break;
            }
        }

        //Get the proper observation sequence
        observations = generator.sequenceList[sequence];

        //For each observation, generate a label in the GUI, as explained in GenerateSolList.cs and GenerateSequenceList.cs
         for(var i = 0; i < observations.Count; i++){
             if(i == 0){
                template.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = "ProductID: " + observations[i].ToString();
                template.transform.Find("prodID").GetComponent<TMP_Text>().text = "Sol " + sol;
             }
             else{
                GameObject clone = Instantiate(template);
                clone.transform.SetParent(template.transform.parent, false);
                clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y - (count * 0.46f), clone.transform.position.z);
                clone.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = "ProductID: " + observations[i].ToString();
                clone.transform.Find("prodID").GetComponent<TMP_Text>().text = "Sol " + sol;
            }

        }

        //Name the obervation's "Sol" gameobject to the productID to generalize for
        //other scripts to access
        GameObject[] observationItems = new GameObject[observations.Count];
        observationItems = GameObject.FindGameObjectsWithTag("MCObservation");
        int g = 0;

        foreach (GameObject oi in observationItems)
        {
            observationItems[g].transform.Find("Sol").name = observations[g].ToString();
            g++;
        }

    }

    //Method resets labels and collects garbage when leaving the observations menu so there
    //is no data to throw off future executions
    public void ResetLabels(){
        //Get all objects with a certain name and destroy them
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "ObservationTemplate(Clone)");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }

        //Rename all observation items to original name to prevent errors in subsequent executions
        GameObject[] observationItems = new GameObject[observations.Count];
        observationItems = GameObject.FindGameObjectsWithTag("MCObservationNum");
        int g = 0;
        foreach (GameObject oi in observationItems)
        {
            oi.name = "Sol";
            g++;
        }
    }
}

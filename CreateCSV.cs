using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CreateCSV : MonoBehaviour {

    private List<string[]> rowData = new List<string[]>();

    // Use this for initialization
    void Start () {
        //Save the CSV that we create from all the labels we saved in
        //ScrapeLabels.cs. This saves the master lookup table.
        Save();
    }

    void Save(){
        //Get the directory we want to read our labels from.

        //*****IMPORTANT***** CHANGE THIS DEPENDING ON YOUR FILE SYSTEM
        DirectoryInfo dir = new DirectoryInfo(@"/Users/alejandro/Desktop/Labels");

        // Create First row of titles manually
        //This is the file header we will use to get datapoints from the CSV
        //Set size of array to be the number of features in the CSV
        string[] rowDataTemp = new string[5];
        rowDataTemp[0] = "productID";
        rowDataTemp[1] = "sol";
        rowDataTemp[2] = "sequence";
        rowDataTemp[3] = "target";
        rowDataTemp[4] = "filter";
        rowData.Add(rowDataTemp);

        //For every file, add its contents to the CSV via AddToCSV()
        foreach(var fi in dir.GetFiles()){
            StartCoroutine(AddToCSV(fi, rowDataTemp));
        }

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"MastCamTable.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"MastCamTable.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"MastCamTable.csv";
        #else
        return Application.dataPath +"/"+"MastCamTable.csv";
        #endif
    }


    //Coroutine run so that on each iteration of the loop, previous file must be closed
    //to open new file. This prevents the 'System.IO: too many open files' error
    IEnumerator AddToCSV(FileInfo fi, string[] rowDataTemp){
        rowDataTemp = new string[5];
        rowDataTemp[0] = fi.Name.Substring(0, fi.Name.Length - 4); // productID
        rowDataTemp[1] = GetSol(fi); // sol num
        rowDataTemp[2] = FindKeyword("SEQUENCE_ID", fi); // sequence name
        rowDataTemp[3] = FindKeyword("TARGET_NAME", fi); // target name
        rowDataTemp[4] = FindKeyword("FILTER_NAME", fi); // filter name
        rowData.Add(rowDataTemp);
        yield return null;
    }

    //Find the desired keyword in the text file and return its value for that file
    private string FindKeyword(string keyword, FileInfo file){
        StreamReader sr = file.OpenText();
        var s = "";
            //Go through the text file, line by line, and while the line is not NULL,
            //check if it is one of the values we want
            while ((s = sr.ReadLine()) != null)
            {
                if(s.Length >= 10 && s.Substring(0, 11).Equals(keyword)){
                    if(keyword == "SEQUENCE_ID"){
                        return (s.Substring(s.Length - 10, 9));
                    }
                    if(keyword == "TARGET_NAME"){
                        for(int i = 0; i < s.Length; i++){
                            //If we find quotes, print out the target name. We print relative to end of the string line
                            //because the line ends in a quotation mark
                            if(s[i] == '"'){
                                return (s.Substring(i + 1, (s.Length - 2) - i));
                            }
                        }
                    }
                }

                    if(s.Length >= 10 && s.Substring(1, 11).Equals(keyword) && keyword == "FILTER_NAME"){
                         for(int i = 0; i < s.Length; i++){
                            //If we find equal sign, print out the target name. We print relative to end of the string line
                            //because the line ends in a quotation mark
                            if(s[i] == '='){
                                return (s.Substring(i + 2));
                            }
                        }

                    }
            }

        return "NULL";
    }

    //Get the sol number from the filename
    private string GetSol(FileInfo file){
        //Get substring of first four characters of filename
        return file.Name.Substring(0,4);
    }
}

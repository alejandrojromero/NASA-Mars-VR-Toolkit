using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

  //Create dictionary variable where we will hold all the infromation from
  //the CSV file with sol information
  public List<Dictionary<string,object>> dict;

  //Create a dictionary with every entry in csv file on Awake()
	void Awake() {
    /*
    Dictionary is composed of all entries in the MastCamTable CSV (comma
    separated values) file found in Assets > Resources. This file was generated
    using the ScrapeLabels and CreateCSV scripts. We then use it here and call
    CSVReader.Read on it. Save the dictionary as a public variable so we can
    use it across other scripts. This is the MOST IMPORTANT variable for the
    project's MastCam system, since it contains all of its data.
    */
		dict = CSVReader.Read("MastCamTable");
	}

}

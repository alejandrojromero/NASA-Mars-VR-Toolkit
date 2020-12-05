using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
Code courtesy of https://github.com/tiago-peres/blog/blob/master/csvreader/CSVReader.cs
Allows us to parse any CSV file and return a dictionary data structure with its data.
Use it by calling CSVReader.Read("YOUR CSV FILE")
*/
public class CSVReader
{
	//Special characters we want to split the file based on
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

	//Also split based on new lines in the file
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

	//Characters to trim
	static char[] TRIM_CHARS = { '\"' };

	//Read the CSV file and output a dictionary we can then use in other code
	public static List<Dictionary<string, object>> Read(string file)
	{
		//Create a list of dictionaries
		var list = new List<Dictionary<string, object>>();

		//Convert CSV file to TextAsset to let us manipulate it
		TextAsset data = Resources.Load (file) as TextAsset;

		//Split the text based on the split characters defined above
		var lines = Regex.Split (data.text, LINE_SPLIT_RE);

		//Base case. Return if data length is 0
		if(lines.Length <= 1) return list;

		//Get header of the CSV file
		var header = Regex.Split(lines[0], SPLIT_RE);

		//For all lines in the data, parse out comma separated values and add it to
		//a dictionary, with the key being the header index and the value being the
		//datapoint

		for(var i=1; i < lines.Length; i++) {

			var values = Regex.Split(lines[i], SPLIT_RE);
			if(values.Length == 0 ||values[0] == "") continue;

			//Save each line as a dictionary
			var entry = new Dictionary<string, object>();
			for(var j=0; j < header.Length && j < values.Length; j++ ) {
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalvalue = value;
				int n;
				float f;
				if(int.TryParse(value, out n)) {
					finalvalue = n;
				} else if (float.TryParse(value, out f)) {
					finalvalue = f;
				}
				entry[header[j]] = finalvalue;
			}
			//Add the line dictionary to the list
			list.Add (entry);
		}
		//Return the list 
		return list;
	}
}

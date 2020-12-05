using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SHOW SUBSET ACCORDING TO SOL #
public class ShowSubset : MonoBehaviour
{
    public void overrideSubset(){
        //When a button in the sol list is selected, only show the sequences for that specific sol
        //"gameObject.name" corresponds to the name of the sol being passed into "ChangeSequence," which
        //will display the sequences according to the passed sol

        Camera.main.GetComponent<GenerateSequenceList>().ChangeSequence(gameObject.name);
    }
}

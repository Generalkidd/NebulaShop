using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GameObject menu = (GameObject)Instantiate(Resources.Load("MiniMenu"));
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

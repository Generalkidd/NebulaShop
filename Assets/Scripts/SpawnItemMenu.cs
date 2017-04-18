using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GameObject menu = (GameObject)Instantiate(Resources.Load("MiniMenu"));
        menu.GetComponent<MiniSelect>().ObjName = "New Game Object";
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

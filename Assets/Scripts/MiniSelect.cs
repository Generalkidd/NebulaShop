using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSelect : MonoBehaviour
{
    public string Action = "";
    public string ObjName = "";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect()
    {
        Action = this.GetComponent<TextMesh>().text;
        GameObject temp;

        switch(Action)
        {
            case "Move Item":
                temp = GameObject.Find(ObjName);
                temp.GetComponent<ItemModeSelect>().isRotation = false;
                resetText();
                temp.GetComponent<TextMesh>().color = Color.gray;
                break;
            case "Rotate Item":
                temp = GameObject.Find(ObjName);
                temp.GetComponent<ItemModeSelect>().isRotation = true;
                resetText();
                temp.GetComponent<TextMesh>().color = Color.gray;
                break;
            case "Remove Item":
                temp = GameObject.Find(ObjName);
                Destroy(temp);
                GameObject temp2 = GameObject.Find("MiniMenu");
                Destroy(temp2);
                break;
        }
    }

    void resetText()
    {
        GameObject temp;

        temp = GameObject.Find("Move");
        temp.GetComponent<TextMesh>().color = Color.green;
        temp = GameObject.Find("Rotate");
        temp.GetComponent<TextMesh>().color = Color.green;
    }
}

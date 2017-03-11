// This example loads an OBJ file from the WWW, including the MTL file and any textures that might be referenced

using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class Example2_WWW : MonoBehaviour {

	public string objFileName = "http://www.starscenesoftware.com/objtest/Spot.obj";
    public string objTextureName = "";
	public Material standardMaterial;	// Used if the OBJ file has no MTL file
	ObjReader.ObjData objData;
	string loadingText = "";
	bool loading = false;

    public void GetObj()
    {
        StartCoroutine(Load());
    }

	IEnumerator Load () {
		loading = true;
		if (objData != null && objData.gameObjects != null) {
			for (var i = 0; i < objData.gameObjects.Length; i++) {
				Destroy (objData.gameObjects[i]);
			}
		}
		
		objData = ObjReader.use.ConvertFileAsync (objFileName, true, standardMaterial);
		while (!objData.isDone) {
			loadingText = "Loading... " + (objData.progress*100).ToString("f0") + "%";
			if (Input.GetKeyDown (KeyCode.Escape)) {
				objData.Cancel();
				loadingText = "Cancelled download";
				loading = false;
				yield break;
			}
			yield return null;
		}
		loading = false;
		if (objData == null || objData.gameObjects == null) {
			loadingText = "Error loading file";
			yield return null;
			yield break;
		}
		
		loadingText = "Import completed";
        StartCoroutine(GetTexture());
		//FocusOnObjects();
	}
	
	//void OnGUI () {
	//	GUILayout.BeginArea (new Rect(10, 10, 400, 400));
	//	objFileName = GUILayout.TextField (objFileName, GUILayout.Width(400));
	//	GUILayout.Label ("Also try pig.obj, car.obj, and cube.obj");
	//	if (GUILayout.Button ("Import") && !loading) {
	//		StartCoroutine (Load());
	//	}
	//	GUILayout.Label (loadingText);
	//	GUILayout.EndArea();
	//}
	IEnumerator GetTexture()
    {
        WWW www = new WWW(objTextureName);
        yield return www;
        GameObject temp;
        temp = GameObject.Find("New Game Object");
        temp.GetComponent<Renderer>().material.mainTexture = www.texture;
        //temp.GetComponent<Transform>().transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        temp.GetComponent<Transform>().transform.localRotation = new Quaternion(180f, 0f, 0f, 0f);
        temp.GetComponent<Transform>().transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        temp.GetComponent<Transform>().transform.position = new Vector3(-1.042f, 0.2f, 3f);
        ApplyScripts();
    }

    void ApplyScripts()
    {
        GameObject temp;
        temp = GameObject.Find("New Game Object");
        temp.AddComponent<GestureManipulator>();
        temp.AddComponent<Interpolator>();
        temp.AddComponent<RotationManipulator>();
        temp.AddComponent<BoxCollider>();
        temp.AddComponent<ItemModeSelect>();
        temp.AddComponent<SpawnItemMenu>();
        temp = GameObject.Find("MiniMenu");
        //temp.GetComponent<MiniSelect>().ObjName = "New Game Object";
    }

	void FocusOnObjects () {
		var cam = Camera.main;
		var bounds = new Bounds(objData.gameObjects[0].transform.position, Vector3.zero);
		for (var i = 0; i < objData.gameObjects.Length; i++) {
			bounds.Encapsulate (objData.gameObjects[i].GetComponent<Renderer>().bounds);
		}
		
		var maxSize = bounds.size;
		var radius = maxSize.magnitude / 2.0f;
	    var horizontalFOV = 2.0f * Mathf.Atan (Mathf.Tan (cam.fieldOfView * Mathf.Deg2Rad / 2.0f) * cam.aspect) * Mathf.Rad2Deg;
	    var fov = Mathf.Min (cam.fieldOfView, horizontalFOV);
	    var distance = radius / Mathf.Sin (fov * Mathf.Deg2Rad / 2.0f);
	
		cam.transform.position = bounds.center;
		cam.transform.Translate (-Vector3.forward * distance);
		cam.transform.LookAt (bounds.center);
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AppleTexture : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Get());
    }

    IEnumerator Get()
    {
        var geturi = new Uri("http://40.121.206.106:5073/getcats"); //replace your url  
        var responseGet = UnityWebRequest.Get(geturi.ToString());
        yield return responseGet.Send();
        if (responseGet.isError)
        {
            Debug.Log(responseGet.error);
        }
        else
        {

        }

        string url = "http://40.121.206.106/uploads/books_tex_kcoleman.jpg";

        WWW www = new WWW(url);
        yield return www;
        this.GetComponent<Renderer>().material.mainTexture = www.texture;
    }

    void Update()
    {

    }
}

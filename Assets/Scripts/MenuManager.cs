using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Assets.Scripts.ApiClients.DataTypes;
using Assets.Scripts.DataTypes;

public class MenuManager : MonoBehaviour
{
    private const string barcode = "+0002F1";

    public MenuManager()
    {
        SelectedReason = -1;
        AdjustmentReasonIds = new List<int>();
        AdjustmentReasonNames = new List<string>();
    }

    public string Action { get; set; }

    private GameObject GameObject { get; set; }

    public int SelectedReason { get; set; }

    private int Count { get; set; }

    private int Increment { get; set; }

    private List<int> AdjustmentReasonIds { get; set; } 

    private List<string> AdjustmentReasonNames { get; set; }

    private Product product;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Get());
    }

    IEnumerator Get()
    {
        var geturi = new Uri("http://40.87.66.169:5073/getdetails/furniture/58239f264d41086272833fec"); //replace your url  
        var responseGet = UnityWebRequest.Get(geturi.ToString());
        yield return responseGet.Send();
        if (responseGet.isError)
        {
            Debug.Log(responseGet.error);
        }
        else
        {
            string response = responseGet.downloadHandler.text;
            product = JsonConvert.DeserializeObject<Product>(response);
        //    var temp = JsonConvert.DeserializeObject<List<ProductArray>>(response);
        //    var temp2 = temp[0];
        //    var temp3 = temp2.Product;
        //    StringBuilder builder = new StringBuilder();
        //    //foreach (string value in temp3)
        //    //{
        //    //    builder.Append(value);
        //    //}
        //    //product = JsonConvert.DeserializeObject<Product>(builder.ToString());
        }

        //if (AdjustmentReasonIds.Count == 0)
        //{
        //    geturi = new Uri("http://192.168.60.143/hololensapi/reasons/");
        //    responseGet = UnityWebRequest.Get(geturi.ToString());
        //    yield return responseGet.Send();
        //    if (responseGet.isError)
        //    {
        //        Debug.Log(responseGet.error);
        //    }
        //    else
        //    {
        //        string response = responseGet.downloadHandler.text;
        //        var adjustmentReasons = JsonConvert.DeserializeObject<List<AdjustmentReason>>(response);
        //        foreach(var reason in adjustmentReasons)
        //        {
        //            AdjustmentReasonIds.Add(reason.Id);
        //            AdjustmentReasonNames.Add(reason.Name);
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        switch (Action)
        {
            case "Television":
                GameObject = GameObject.Find("MenuTitle");
                GameObject.GetComponent<TextMesh>().text = product.Item;
                GameObject = GameObject.Find("Option1");
                GameObject.GetComponent<TextMesh>().text = product.ItemDesc;
                GameObject = GameObject.Find("Option2");
                GameObject.GetComponent<TextMesh>().text = product.Price;
                GameObject = GameObject.Find("Option3");
                GameObject.GetComponent<TextMesh>().text = product.Seller;
                GameObject = GameObject.Find("Option4");
                GameObject.GetComponent<TextMesh>().text = product.Email;
                GameObject = GameObject.Find("TV_obj");
                GameObject.GetComponent<Transform>().transform.position = new Vector3(-1.042f, 0.178f, 1.623f);
                Action = "";
                break;
            case "Decrease":
                Count--;
                Increment--;
                GameObject = GameObject.Find("Option2");
                GameObject.GetComponent<TextMesh>().text = Count.ToString();
                Action = "";
                break;
            case "Next":
                GameObject = GameObject.Find("Option1");
                GameObject.GetComponent<TextMesh>().text = AdjustmentReasonNames[0];
                GameObject.GetComponent<MenuSelect>().reasonId = 0;
                GameObject.GetComponent<MenuSelect>().isReasons = true;
                GameObject = GameObject.Find("Option2");
                GameObject.GetComponent<TextMesh>().text = AdjustmentReasonNames[1];
                GameObject.GetComponent<MenuSelect>().reasonId = 1;
                GameObject.GetComponent<MenuSelect>().isReasons = true;
                GameObject = GameObject.Find("Option3");
                GameObject.GetComponent<TextMesh>().text = AdjustmentReasonNames[2];
                GameObject.GetComponent<MenuSelect>().reasonId = 2;
                GameObject.GetComponent<MenuSelect>().isReasons = true;
                GameObject = GameObject.Find("Option4");
                GameObject.GetComponent<TextMesh>().text = "Submit";
                Action = "";
                break;
            case "Submit":
                if (SelectedReason != -1)
                {
                    StartCoroutine(submit());
                }
                Action = "";
                break;
        }
    }

    void Increase()
    {
        Action = "Increase";
    }

    void Decrease()
    {
        Action = "Decrease";
    }

    void Next()
    {
        Action = "Next";
    }

    void Submit()
    {
        Action = "Submit";
    }

    IEnumerator submit()
    {
        if(Increment != 0)
        {
            var geturi = new Uri("http://192.168.60.143/hololensapi/adjustments"); //replace your url  
            var adjustment = new Adjustment {Barcode = barcode, Quantity = Increment, ReasonId = AdjustmentReasonIds[SelectedReason] };
            var serializedAdjustment = JsonConvert.SerializeObject(adjustment);
            byte[] adjustmentBytes = Encoding.UTF8.GetBytes(serializedAdjustment);
            var postRequest = new UnityWebRequest(geturi.ToString(), "POST");
            postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(adjustmentBytes);
            postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");
            yield return postRequest.Send();
            if (postRequest.isError)
            {
                Debug.Log(postRequest.error);
            }
            if (postRequest.responseCode == 201)
            {
                Increment = 0;
                this.GetComponent<TextMesh>().text = "SUCCESS!";
                GameObject = GameObject.Find("Option1");
                GameObject.GetComponent<TextMesh>().text = "";
                GameObject = GameObject.Find("Option2");
                GameObject.GetComponent<TextMesh>().text = "";
                GameObject = GameObject.Find("Option3");
                GameObject.GetComponent<TextMesh>().text = "";
                GameObject = GameObject.Find("Option4");
                GameObject.GetComponent<TextMesh>().text = "";
            }
            else
            {
                this.GetComponent<TextMesh>().text = string.Format("The request failed. Status Code: {0}", postRequest.responseCode);
                GameObject = GameObject.Find("Option1");
                GameObject.GetComponent<TextMesh>().text = "";
                GameObject = GameObject.Find("Option2");
                GameObject.GetComponent<TextMesh>().text = "";
                GameObject = GameObject.Find("Option3");
                GameObject.GetComponent<TextMesh>().text = "";
                GameObject = GameObject.Find("Option4");
                GameObject.GetComponent<TextMesh>().text = "";
            }
        }
    }
}

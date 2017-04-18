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

    private Cats categories;

    public bool catSelect = false;

    public bool itemSelect = false;

    public List<ItemsListJson> tmp;

    public GameObject temp;

    private string currentCat = "";

    // Use this for initialization
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
            string response = responseGet.downloadHandler.text;
            categories = JsonConvert.DeserializeObject<Cats>(response);

            GameObject = GameObject.Find("Option1");
            GameObject.GetComponent<TextMesh>().text = categories.Categories[0];
            GameObject = GameObject.Find("Option2");
            GameObject.GetComponent<TextMesh>().text = categories.Categories[1];
            GameObject = GameObject.Find("Option3");
            GameObject.GetComponent<TextMesh>().text = categories.Categories[2];
            GameObject = GameObject.Find("Option4");
            GameObject.GetComponent<TextMesh>().text = categories.Categories[3];
        }

        catSelect = true;

        geturi = new Uri("http://40.121.206.106:5073/getdetails/electronics/58239f264d41086272833fec"); //replace your url  
        responseGet = UnityWebRequest.Get(geturi.ToString());
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

        //GameObject temp = GameObject.Find("ObjectManager");
        //temp.GetComponent<Example2_WWW>().GetObj();

        //string url = "http://www.nebulashop.net/uploads/books_mod_kcoleman.obj";

        //while (!Caching.ready)
        //    yield return null;
        //// Start a download of the given URL
        //WWW www = WWW.LoadFromCacheOrDownload(url, 1);
        //yield return www;

        //// Load and retrieve the AssetBundle
        //AssetBundle bundle = www.assetBundle;

        //// Load the object asynchronously
        //AssetBundleRequest request = bundle.LoadAssetAsync("book_mod_kcoleman", typeof(GameObject));

        //// Wait for completion
        //yield return request;

        //// Get the reference to the loaded object
        //GameObject obj = request.asset as GameObject;

        //// Unload the AssetBundles compressed contents to conserve memory
        //bundle.Unload(false);

        //// Frees the memory from the web stream
        //www.Dispose();

        //obj.GetComponent<Transform>().transform.position = new Vector3(-1.042f, 0.178f, 1.623f);

        //renderer.material.mainTexture = www.texture;

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
            case "electronics":
                clearMenu();
                StartCoroutine(getItems("http://40.121.206.106:5073/getitems/electronics"));
                GameObject = GameObject.Find("MenuTitle");
                GameObject.GetComponent<TextMesh>().text = "Electronics";
                currentCat = Action;
                Action = "";
                break;
            case "books":
                clearMenu();
                StartCoroutine(getItems("http://40.121.206.106:5073/getitems/books"));
                GameObject = GameObject.Find("MenuTitle");
                GameObject.GetComponent<TextMesh>().text = "Books";
                currentCat = Action;
                Action = "";
                break;
            case "furniture":
                clearMenu();
                StartCoroutine(getItems("http://40.121.206.106:5073/getitems/furniture"));
                GameObject = GameObject.Find("MenuTitle");
                GameObject.GetComponent<TextMesh>().text = "Furniture";
                currentCat = Action;
                Action = "";
                break;
            case "music":
                clearMenu();
                StartCoroutine(getItems("http://40.121.206.106:5073/getitems/music"));
                GameObject = GameObject.Find("MenuTitle");
                GameObject.GetComponent<TextMesh>().text = "Music";
                currentCat = Action;
                Action = "";
                break;
            case "Option1":
                Action = "";
                temp = GameObject.Find("ObjectManager");
                temp.GetComponent<Example2_WWW>().GetObj(tmp[0].Model, tmp[0].Texture);
                StartCoroutine(getProductInfo("http://40.121.206.106:5073/getdetails/" + currentCat + "/" + tmp[0].ID));
                break;
            case "Option2":
                Action = "";
                temp = GameObject.Find("ObjectManager");
                temp.GetComponent<Example2_WWW>().GetObj(tmp[1].Model, tmp[1].Texture);
                StartCoroutine(getProductInfo("http://40.121.206.106:5073/getdetails/" + currentCat + "/" + tmp[1].ID));
                break;
            case "Option3":
                Action = "";
                temp = GameObject.Find("ObjectManager");
                temp.GetComponent<Example2_WWW>().GetObj(tmp[2].Model, tmp[2].Texture);
                StartCoroutine(getProductInfo("http://40.121.206.106:5073/getdetails/" + currentCat + "/" + tmp[2].ID));
                break;
            case "Option4":
                Action = "";
                temp = GameObject.Find("ObjectManager");
                temp.GetComponent<Example2_WWW>().GetObj(tmp[3].Model, tmp[3].Texture);
                StartCoroutine(getProductInfo("http://40.121.206.106:5073/getdetails/" + currentCat + "/" + tmp[3].ID));
                break;
        }
    }

    void loadMenu()
    {
        temp = GameObject.Find("ObjectManager");
        for (int i = 0; i < tmp.Capacity; i++)
        {
            temp.GetComponent<Example2_WWW>().GetObj(tmp[i].Model, tmp[i].Texture);
            StartCoroutine(getProductInfo("http://40.121.206.106:5073/getdetails/" + currentCat + "/" + tmp[i].ID));
        }
    }

    void clearMenu()
    {
        GameObject = GameObject.Find("Option1");
        GameObject.GetComponent<TextMesh>().text = "";
        GameObject = GameObject.Find("Option2");
        GameObject.GetComponent<TextMesh>().text = "";
        GameObject = GameObject.Find("Option3");
        GameObject.GetComponent<TextMesh>().text = "";
        GameObject = GameObject.Find("Option4");
        GameObject.GetComponent<TextMesh>().text = "";
    }

    IEnumerator getProductInfo(string url)
    {
        var geturi = new Uri(url); //replace your url  
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
        }
    }

    IEnumerator getItems(string url)
    {
        var geturi = new Uri(url); //replace your url  
        var responseGet = UnityWebRequest.Get(geturi.ToString());
        yield return responseGet.Send();
        if (responseGet.isError)
        {
            Debug.Log(responseGet.error);
        }
        else
        {
            string response = responseGet.downloadHandler.text;
            tmp = JsonConvert.DeserializeObject<List<ItemsListJson>>(response);

            int i = 0;

            try
            {
                GameObject = GameObject.Find("Option1");
                GameObject.GetComponent<TextMesh>().text = tmp[0].Item;
                GameObject = GameObject.Find("Option2");
                GameObject.GetComponent<TextMesh>().text = tmp[1].Item;
                GameObject = GameObject.Find("Option3");
                GameObject.GetComponent<TextMesh>().text = tmp[2].Item;
                GameObject = GameObject.Find("Option4");
                GameObject.GetComponent<TextMesh>().text = tmp[3].Item;
            }
            catch(Exception e)
            {

            }
        }

        itemSelect = true;
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
            var geturi = new Uri(""); //replace your url  
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class JSONLoader : MonoBehaviour
{
    [SerializeField]
    string url = "https://pokeapi.co/api/v2/pokemon/2/";
    public delegate void JSONRefreshed(JSONNode json);
    public JSONRefreshed jsonRefreshed;

    JSONNode jsonNode;

    void Start()
    {
        StartRefreshJSON();
    }

    public void StartRefreshJSON()
    {
        //For external use
        StartCoroutine(RefreshJSON());
    }


    IEnumerator RefreshJSON()
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.error == null)
        {
            jsonNode = JSON.Parse(www.text);
            jsonRefreshed?.Invoke(jsonNode);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }
}

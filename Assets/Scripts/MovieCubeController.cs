using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class MovieCubeController : MonoBehaviour
{
    private int actualFace;
    private GameObject movieCube;
    private Material testMaterial;
    
    private GameObject caraDel;
    private GameObject caraDet;
    private GameObject caraDer;
    private GameObject caraIzq;

    [SerializeField] public Texture texturaDel;
    [SerializeField] public Texture texturaDet;
    [SerializeField] public Texture texturaDer;
    [SerializeField] public Texture texturaIzq;
    // Start is called before the first frame update
    void Start()
    {
        movieCube = GameObject.Find("Moviecube");
        caraDel = GameObject.Find("caraDel");
        caraDet = GameObject.Find("caraDet");
        caraDer = GameObject.Find("caraDer");
        caraIzq = GameObject.Find("caraIzq");
        StartCoroutine(GetImg());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void generateCubeImages()
    {
        caraDel.GetComponent<Renderer>().material.SetTexture("_MainTex",texturaDel);
        caraDet.GetComponent<Renderer>().material.SetTexture("_MainTex",texturaDet);
        caraDer.GetComponent<Renderer>().material.SetTexture("_MainTex",texturaDer);
        caraIzq.GetComponent<Renderer>().material.SetTexture("_MainTex",texturaIzq);
    }

    void RightRotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }

    void LeftRotate()
    {
        transform.Rotate(new Vector3(0, -90, 0));
    }
    
    IEnumerator GetImg()
    {
        UnityWebRequest data = UnityWebRequest.Get("https://www.omdbapi.com/?s=glory&apikey=bec0dc5f");
        yield return data.SendWebRequest();
        if (data.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(data.error);
        }
        else
        {
            Debug.Log(data.downloadHandler.text);
            SearchData mySearch = JsonUtility.FromJson<SearchData>(data.downloadHandler.text);
            Debug.Log(mySearch.Search[0].Poster);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(mySearch.Search[1].Poster);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
                texturaDel = loadedTexture;
                print(mySearch.Search[0].Title);
                generateCubeImages();
                //todo implementar todo esto a mi modo y unir los sprites a las caras de los cubos
            }
        }
    }
    [Serializable]
    public class SearchData
    {
        public List<MovieData> Search;
        public string totalResults;
        public string Response;
    }
    [Serializable]
    public class MovieData
    {
        public string Title;
        public string Year;
        public string imdbID;
        public string Type;
        public string Poster;
    }
}
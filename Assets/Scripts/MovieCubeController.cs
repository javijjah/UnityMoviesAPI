using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MovieCubeController : MonoBehaviour
{
    private int _actualFace;
    private GameObject _movieCube;
    private Material _testMaterial;
    private List<MovieData> _loadedMovies = new();
    private GameObject _caraDel;
    private GameObject _caraDet;
    private GameObject _caraDer;
    private GameObject _caraIzq;

    private Renderer _rendererDel;
    private Renderer _rendererDet;
    private Renderer _rendererDer;
    private Renderer _rendererIzq;


    [SerializeField] public Texture texturaDel;
    [SerializeField] public Texture texturaDet;
    [SerializeField] public Texture texturaDer;
    [SerializeField] public Texture texturaIzq;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    // Start is called before the first frame update
    void Start()
    {
        _movieCube = GameObject.Find("Moviecube");
        _caraDel = GameObject.Find("caraDel");
        _caraDet = GameObject.Find("caraDet");
        _caraDer = GameObject.Find("caraDer");
        _caraIzq = GameObject.Find("caraIzq");
        _rendererDel = _caraDel.GetComponent<Renderer>();
        _rendererDet = _caraDet.GetComponent<Renderer>();
        _rendererDer = _caraDer.GetComponent<Renderer>();
        _rendererIzq = _caraIzq.GetComponent<Renderer>();
        StartCoroutine(GetPage());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateCubeImages()
    {
        _rendererDel.material.SetTexture(MainTex, texturaDel);
        _rendererDet.material.SetTexture(MainTex, texturaDet);
        _rendererDer.material.SetTexture(MainTex, texturaDer);
        _rendererIzq.material.SetTexture(MainTex, texturaIzq);
    }

    void RightRotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }

    void LeftRotate()
    {
        transform.Rotate(new Vector3(0, -90, 0));
    }

    IEnumerator GetPage()
    {
        UnityWebRequest data =
            UnityWebRequest.Get("https://www.omdbapi.com/?apikey=bec0dc5f&type=movie&page=1&y=2023&s=hello");
        yield return data.SendWebRequest();
        if (data.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(data.error);
            print("apierror");
        }
        else
        {
            Debug.Log(data.downloadHandler.text);
            SearchData mySearch = JsonUtility.FromJson<SearchData>(data.downloadHandler.text);
            Debug.Log(mySearch.Search[0].Poster);
            for (int i = 0; i < mySearch.Search.Count; i++)
            {
                _loadedMovies.Add(mySearch.Search[i]);
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(mySearch.Search[i].Poster);
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    _loadedMovies[i].PosterText = DownloadHandlerTexture.GetContent(www);
                }

                print(i);
                print(_loadedMovies[i].Title);
            }

            FillCubeTextures();
        }
    }

    public void FillCubeTextures()
    {
        texturaDel = _loadedMovies[0].PosterText;
        texturaDet = _loadedMovies[1].PosterText;
        texturaDer = _loadedMovies[2].PosterText;
        texturaIzq = _loadedMovies[3].PosterText;
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
        [CanBeNull] public Texture PosterText;
    }
}
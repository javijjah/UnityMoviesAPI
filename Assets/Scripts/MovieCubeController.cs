using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class MovieCubeController : MonoBehaviour
{
    private int _actualFace;
    private GameObject _movieCube;
    private Material _testMaterial;
    private List<MovieData> _loadedMovies = new();
    private Quaternion _targetRotation;
    System.Random rnd = new System.Random();
    private string baselink = "https://www.omdbapi.com/?apikey=bec0dc5f&type=movie&page=1&y=2023&s=";

    private List<string> _listOfTerms = new List<string>()
    {
        "Hello","Lord","Tree","Fish","Hollywood","Love","War","Time","Night","Day","Life","Death","World","Man","Woman",
        "Child","Adventure","Journey","Dream","Mystery","Secret","Star","Game","Heart"
    };
    
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
        StartCoroutine(GetPage(GetRandomMoviePageLink()));
    }

    // Update is called once per frame
    void Update()
    {
        GenerateCubeImages();
    }

    public string GetRandomMoviePageLink()
    {
        return baselink + _listOfTerms[rnd.Next(_listOfTerms.Count)];
    }

    void GenerateCubeImages()
    {
        _rendererDel.material.SetTexture(MainTex, texturaDel);
        _rendererDet.material.SetTexture(MainTex, texturaDet);
        _rendererDer.material.SetTexture(MainTex, texturaDer);
        _rendererIzq.material.SetTexture(MainTex, texturaIzq);
    }

    public void RightRotate()
    {
        Debug.Log("RightRotatingCube");
        var rotation = transform.rotation;
            Quaternion targetRotation = rotation * Quaternion.AngleAxis(90f, Vector3.up);
            StartCoroutine(Rotationcor(targetRotation));
    }

    public void limpiarPelisCargadas()
    {
      _loadedMovies.Clear();
    }
    IEnumerator Rotationcor(Quaternion targetRotation)
    {
        //el bug está en que cada vez que esto se ejecuta está asignando el valor, por lo que no puede rotar porque no tiene una 
        //referencia clara. Hay que conseguir una referencia general que no cambie.
        //var rotation = transform.rotation;
        //Quaternion targetRotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y+180, rotation.z));
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 200);
            yield return null;
        }
    }

    public IEnumerator GetPage(string req)
    {
        Debug.Log("Cadena utilizada:" + req);
        UnityWebRequest data =
            UnityWebRequest.Get(req);
        yield return data.SendWebRequest();
        if (data.result == UnityWebRequest.Result.ConnectionError)
        {
            print("apierror");
        }
        else
        {
            SearchData mySearch = JsonUtility.FromJson<SearchData>(data.downloadHandler.text);
            for (int i = 0; i < mySearch.Search.Count; i++)
            {
                _loadedMovies.Add(mySearch.Search[i]);
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(mySearch.Search[i].Poster);
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                }
                else
                {
                    try
                    {
                        _loadedMovies[i].PosterText = DownloadHandlerTexture.GetContent(www);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Poster of " + _loadedMovies[i].Title + "couldn't be downloaded");
                    }
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
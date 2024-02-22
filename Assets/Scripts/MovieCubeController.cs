using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
/**
 * Gestor principal del cubo central que muestra las películas.
 */
public class MovieCubeController : MonoBehaviour
{
    /**
     * Lista que guarda las películas cargadas que tenemos
     */
    private List<MovieData> _loadedMovies = new();
    /**
     * Nuestro gestor de números aleatorios
     */
    System.Random rnd = new();
    /**
     * Link base para llamar a la API
     */
    private string baselink = "https://www.omdbapi.com/?apikey=bec0dc5f&type=movie&page=1&y=2023&s=";

    /**
     * Lista de términos que luego serán escogidos de forma aleatoria para mostar películas que los incluyan
     */
    private List<string> _listOfTerms = new List<string>()
    {
        "Hello","Lord","Tree","Fish","Hollywood","Love","War","Time","Night","Day","Life","Death","World","Man","Woman",
        "Child","Adventure","Journey","Dream","Mystery","Secret","Star","Game","Heart"
    };
    
    //Estas caras serían los Quads que muestran las películas, y con el rendered podríamos cambiar su textura
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
    /**
     * Obtiene un valor random para buscar en la API de la lista
     */
    public string GetRandomMoviePageLink()
    {
        return baselink + _listOfTerms[rnd.Next(_listOfTerms.Count)];
    }
    /**
     * Regenera las imágenes de los quads a las sostenidas en las texturas
     */
    void GenerateCubeImages()
    {
        _rendererDel.material.SetTexture(MainTex, texturaDel);
        _rendererDet.material.SetTexture(MainTex, texturaDet);
        _rendererDer.material.SetTexture(MainTex, texturaDer);
        _rendererIzq.material.SetTexture(MainTex, texturaIzq);
    }
    /**
     * Función padre de la corrutina que gestiona el giro de nuestro cubo
     */
    public void RightRotate()
    {
        Debug.Log("RightRotatingCube");
        var rotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, (Mathf.Round(rotation.eulerAngles.y) + 90f)%360f, 0);
            StartCoroutine(Rotationcor(targetRotation));
            
    }
    /**
     * Limpia la lista de pelis cargadas para dejar espacio a otras
     */
    public void LimpiarPelisCargadas()
    {
      _loadedMovies.Clear();
    }
    /**
     * Corrutina que ejecuta el giro por frame de nuestro cubo
     */
    IEnumerator Rotationcor(Quaternion targetRotation)
    {
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 200);
            yield return null;
        }
    }
    /**
     * Corrutina la cual recupera una página de la API, introduciéndole el parámetro de búsqueda
     */
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
    /**
     * Función que asigna las texturas las 4 primeras películas cargadas a las texturas del cubo
     */
    public void FillCubeTextures()
    {
        texturaDel = _loadedMovies[0].PosterText;
        texturaDet = _loadedMovies[1].PosterText;
        texturaDer = _loadedMovies[2].PosterText;
        texturaIzq = _loadedMovies[3].PosterText;
    }
    /**
     * Clase que recibe los datos de la búsqueda (El JSON general)
     */
    [Serializable]
    public class SearchData
    {
        public List<MovieData> Search;
        public string totalResults;
        public string Response;
    }
    /**
     * Clase que almacena los datos de las películas recuperadas por la API
     */
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
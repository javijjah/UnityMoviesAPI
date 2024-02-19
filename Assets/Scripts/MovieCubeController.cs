using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        generateCubeImages();
        
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
}
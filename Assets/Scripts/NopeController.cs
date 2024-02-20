using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NopeController : MonoBehaviour
{
    private GameObject _movieCube;
    private MovieCubeController _movieCubeController;
    void Start()
    {
        _movieCube = GameObject.Find("Moviecube");
        _movieCubeController = _movieCube.GetComponent<MovieCubeController>();
    }
    public void Nope()
    {
        Debug.Log("nope");
        _movieCubeController.RightRotate();
    }

}

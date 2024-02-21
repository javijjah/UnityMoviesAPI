using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class NopeController : MonoBehaviour
{
    private GameObject _movieCube;
    private MovieCubeController _movieCubeController;
    void Start()
    {
        _movieCube = GameObject.Find("Moviecube");
        _movieCubeController = _movieCube.GetComponent<MovieCubeController>();
    }
    public void Nope(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("nope");
            _movieCubeController.RightRotate();
        }
    }

}

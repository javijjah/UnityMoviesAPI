using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RerollController : MonoBehaviour
{
    private GameObject _movieCube;
    private MovieCubeController _movieCubeController;
    void Start()
    {
        _movieCube = GameObject.Find("Moviecube");
        _movieCubeController = _movieCube.GetComponent<MovieCubeController>();
    }
    public void Reroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Reroll");
            _movieCubeController.LimpiarPelisCargadas();
            StartCoroutine(_movieCubeController.GetPage(_movieCubeController.GetRandomMoviePageLink()));
        }
    }

    public void listenedNope(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("NopeListened");
        }
    }
}

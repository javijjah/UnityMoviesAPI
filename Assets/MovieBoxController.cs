using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovieBoxController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
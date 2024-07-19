using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    private Transform _transform;
    void Start()
    {
        cam = Camera.main.transform;
        _transform = GetComponent<Transform>();
    }
    void LateUpdate()
    {
        _transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);
    }
}

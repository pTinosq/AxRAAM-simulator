using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followObject;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followObject.transform.position.x, followObject.transform.position.y + 0.1f, followObject.transform.position.z - 2);
    }
}

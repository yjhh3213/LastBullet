using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public float smoothing = 5.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        Vector3 move = new Vector3(dir.x * smoothing * Time.deltaTime, dir.y * smoothing * Time.deltaTime, 0.0f);
        
        this.transform.Translate(move);
    }
}

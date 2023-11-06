using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolinInstate : MonoBehaviour
{
    public float scale;
    public GameObject randomPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (randomPrefab != null)
        {
            Debug.Log("noise:" + Mathf.PerlinNoise(Time.time, Time.time));
            Instantiate(randomPrefab, new Vector3(Mathf.PerlinNoise(Time.time, Time.time) * scale, Mathf.PerlinNoise(Time.time, - Time.time) * scale, Mathf.PerlinNoise(-Time.time, Time.time)) * scale, new Quaternion());
        }
    }
}

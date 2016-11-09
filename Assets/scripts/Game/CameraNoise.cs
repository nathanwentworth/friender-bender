using UnityEngine;
using System.Collections;

public class CameraNoise : MonoBehaviour
{

    public Transform obj;
    private float x1, y1, x2, y2;
    public float noiseStrength = 5;

    void Start()
    {
        x1 = y1 = x2 = y2 = 0f;
    }

    void FixedUpdate()
    {
        if (!obj) return;
        obj.transform.localPosition = new Vector3(Mathf.PerlinNoise(x1 += .01f, y1 += .01f) * Time.deltaTime * noiseStrength, Mathf.PerlinNoise(x2 += .01f, y2 += .01f) * Time.deltaTime * noiseStrength, 0);
    }
}
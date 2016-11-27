using UnityEngine;
using System.Collections;

public class CameraNoise : MonoBehaviour
{

    public Transform obj;
    private float x1, y1, x2, y2;
    [SerializeField]
    private float noiseStrength = 5;
    [SerializeField]
    private float shakeSpeed = 0.01f;

    void Start()
    {
        x1 = y1 = 0f;
        x2 = y2 = 0.5f;
    }

    void FixedUpdate()
    {
        if (!obj) return;
        obj.transform.localPosition = new Vector3((Mathf.PerlinNoise(x1 += shakeSpeed, y1 += shakeSpeed) - 0.5f) * Time.deltaTime * noiseStrength, -(Mathf.PerlinNoise(x2 -= shakeSpeed, y2 -= shakeSpeed) - 0.5f) * Time.deltaTime * noiseStrength, 0);
    }
}
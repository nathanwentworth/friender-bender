using UnityEngine;
using System.Collections;

public class CameraNoise : MonoBehaviour
{

    public Transform obj;
    private float x1, y1, x2, y2;
    [SerializeField]
    private float noiseStrength = 5f;
    [SerializeField]
    private float shakeSpeed = 0.01f;

    private void Start()
    {
        x1 = y1 = 0f;
        x2 = y2 = 0.5f;
    }

    private void FixedUpdate()
    {
        if (!obj) return;
        obj.transform.localPosition = new Vector3((Mathf.PerlinNoise(x1 += shakeSpeed, y1 += shakeSpeed) - 0.5f) * Time.deltaTime * noiseStrength, -(Mathf.PerlinNoise(x2 -= shakeSpeed, y2 -= shakeSpeed) - 0.5f) * Time.deltaTime * noiseStrength, 0);
    }

    public IEnumerator ScreenShake(float speed, float strength) {
        noiseStrength = 5f;
        shakeSpeed = 0.01f;
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime;
            shakeSpeed = Mathf.SmoothStep(speed, 0.01f, t);
            noiseStrength = Mathf.SmoothStep(strength, 5f, t);
            yield return null;
        }
        yield return null;
    }

}
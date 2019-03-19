using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
    private Light lite;
    private Vector3 initialPos;
    private float initialIntensity;
    public float intensityModify = 1;
    public Color alikeColor;
    private Color initialColor;
    private bool stoppedLight;
    public bool moveOnXY = true;
    public bool moveOnZ;

    public float speed = 35;
    public float length = 0.05f;
    

    private float[] randomVals = new float[8];

    private void Start() {
        lite = GetComponent<Light>();
        initialIntensity = lite.intensity;
        initialPos = transform.localPosition;
        initialColor = lite.color;

        GenerateRandom();
    }

    private void Update() {
        if (!stoppedLight) {
            transform.localPosition = initialPos;
            if (moveOnXY)
                transform.localPosition += new Vector3(Mathf.Sin(Time.time * speed * randomVals[0]) * length * randomVals[1],
                    Mathf.Cos(Time.time * speed * randomVals[2]) * length * randomVals[3]);
            if (moveOnZ) {
                transform.localPosition += new Vector3(0,0, Mathf.Sin(Time.time * speed * randomVals[7]) * length * randomVals[6]) * Time.deltaTime * 20f;
            }
            lite.intensity = initialIntensity + (Mathf.Sin(Time.time * speed * randomVals[4]) * intensityModify);
            lite.color = Color.Lerp(initialColor, alikeColor, Mathf.Sin(Time.time * speed * randomVals[5]) / 2f + 0.5f);
            
        }
    }

    private void GenerateRandom() {

        for (int i = 0; i < randomVals.Length; i++) {
            randomVals[i] = Random.Range(0.8f, 1.2f);
        }
    }

    public void StopLight() {
        lite.intensity = 0;
        stoppedLight = true;
    }

    public void StartLight() {
        stoppedLight = false;
    }
}

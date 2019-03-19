using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAutoKill : MonoBehaviour {
    private Light lite;
    public float deathSeconds = 1;
    public bool fadeIntensity = true;
    public bool fadeRadius;
    public AnimationCurve fadeCurve;
    private float initialIntensity;
    private float initialRadius;
    private float counter;
    
	void Start () {
        lite = GetComponent<Light>();
        lite.FadeAndDie(this, deathSeconds, fadeCurve);
	}
}

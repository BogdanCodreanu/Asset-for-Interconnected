using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour {
    public float speed;
    public float length;

    private Vector3 initialPos;

    private void Start() {
        initialPos = transform.position;
    }

    void Update () {
        transform.position = initialPos + new Vector3(Mathf.Sin(Time.time * speed) * length, 0);
	}
}

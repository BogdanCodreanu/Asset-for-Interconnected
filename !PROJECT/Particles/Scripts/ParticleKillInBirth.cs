using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKillInBirth : MonoBehaviour {
    private ParticleSystem ps;
    public float timeAlive = 1f;

    private void Start() {
        ps = GetComponent<ParticleSystem>();

        StartCoroutine(Die());
    }

    private IEnumerator Die() {
        yield return new WaitForSeconds(timeAlive);
        ps.StopEmmittingAndDie(timeAlive);
    }
}

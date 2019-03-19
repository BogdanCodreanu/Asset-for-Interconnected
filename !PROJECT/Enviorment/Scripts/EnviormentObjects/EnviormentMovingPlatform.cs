using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentMovingPlatform : EnviormentObject {
    public Vector3[] movingPositions;
    public float movingSpeed;
    public bool circular;
    public float pauseTime;

    private bool paused;
    private float pauseCounter;
    private bool goingBackwards;

    private int nextIndex;
    private int previousIndex;
    private float initialMovingDistance;

    private Rigidbody2D rb;
    private Vector3 direction;
    private bool working;
    

    public override void Start2() {
        transform.position = movingPositions[0];
        rb = GetComponent<Rigidbody2D>();
        nextIndex = 0;
        ReachedDestination();
    }

    public void FixedUpdate() {
        if (working) {
            transform.rotation = Quaternion.identity;
            if (!paused) {
                rb.velocity = direction * movingSpeed * Time.deltaTime * 50f;
                if ((transform.position - movingPositions[previousIndex]).magnitude >= initialMovingDistance - 0.01f) {
                    ReachedDestination();
                    rb.StopAllForces();
                    rb.position = movingPositions[previousIndex];
                }
            }
            else {
                rb.StopAllForces();
                pauseCounter += Time.deltaTime;
                if (pauseCounter >= pauseTime) {
                    paused = false;
                    rb.position = movingPositions[previousIndex];
                }
            }
        }
    }

    private void ReachedDestination() {
        pauseCounter = 0f;
        paused = true;
        previousIndex = nextIndex;

        if (!circular) {
            if (goingBackwards) {
                nextIndex--;
            }
            else {
                nextIndex++;
            }

            if (nextIndex == movingPositions.Length) {
                goingBackwards = true;
                nextIndex -= 2;
            }
            if (nextIndex == -1) {
                goingBackwards = false;
                nextIndex += 2;
            }
        } else {
            nextIndex++;
            if (nextIndex == movingPositions.Length) {
                nextIndex = 0;
            }
        }

        direction = (movingPositions[nextIndex] - movingPositions[previousIndex]).normalized;

        initialMovingDistance = (movingPositions[nextIndex] - transform.position).magnitude;
    }

    public override void EnabledState(bool value) {
        rb.StopAllForces();
        if (value) {
            nextIndex = 0;
            ReachedDestination();
            goingBackwards = false;
        }
        working = value;
    }

    //private void OnCollisionStay2D(Collision2D collision) {
    //    Rigidbody2D rb = collision.rigidbody;
    //    if (rb) {
    //        rb.AddForce((movingPositions[nextIndex] - transform.position).normalized * rb.mass * movingSpeed);
    //    }
    //}
}

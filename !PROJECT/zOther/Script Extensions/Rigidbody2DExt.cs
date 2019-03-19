using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rigidbody2DExt {
    public static void StopAllForces(this Rigidbody2D rb) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }
}

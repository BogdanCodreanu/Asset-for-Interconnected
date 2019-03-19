using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShooter : MechanicalOperational {
    private float counterCooldown = 0;
    
    public int bullets = 1;
    private int bulletsCounter;
    public float cooldown = 2f;
    public float cannonForce = 50f;
    public float backpushPowerOnMech = 10f;

    public GameObject bulletPrefab;
    public GameObject barrelSmokeEffect;
    public Transform barrel;

    private List<GameObject> bulletSpawns = new List<GameObject>();

    private bool canShoot = true;

    public bool useCutoffEmissionMaterial;
    private Coroutine changingEmission;

    public override void Start2() {
        base.Start2();

        counterCooldown = cooldown;
    }

    public override void Operate() {
        base.Operate();
        if (counterCooldown >= cooldown && canShoot && ownAnchoredJoint.connectedBody && bulletsCounter < bullets) {
            Shoot();
            counterCooldown = 0;
            if (barrelSmokeEffect)
                Instantiate(barrelSmokeEffect, barrel.position, Quaternion.LookRotation((transform.rotation * Vector2.right).normalized, Vector3.forward));
            if (bullets != -1)
                bulletsCounter++;

            if (outputsSignal) {
                outgoingSignal.value = (bulletsCounter < bullets) ? 1 : 0;
                if (useCutoffEmissionMaterial)
                    StartCoroutine(ChangeEmissionValue(((float)bulletsCounter / bullets)));
            }
        }
    }
    private IEnumerator ChangeEmissionValue(float value) {
        float startTime = Time.time;
        float lerper = 0;
        Material emissiveMat = ownRend.material;
        float initialValue = emissiveMat.GetFloat("_Cutoff");
        while (lerper < 1) {
            lerper = (Time.time - startTime) / .2f;
            emissiveMat.SetFloat("_Cutoff", Mathf.Lerp(initialValue, value, lerper));
            yield return new WaitForEndOfFrame();
        }
        emissiveMat.SetFloat("_Cutoff", value);
    }

    private void Shoot() {
        ownAnchoredJoint.connectedBody.AddForceAtPosition(
            (handleTransform.rotation * Vector2.left).normalized * backpushPowerOnMech,
            handleTransform.position, ForceMode2D.Impulse);
        Rigidbody2D spawn = Instantiate(bulletPrefab, barrel.position, Quaternion.identity, null).GetComponent<Rigidbody2D>();
        spawn.AddForce((handleTransform.rotation * Vector2.right).normalized * cannonForce, ForceMode2D.Impulse);
        bulletSpawns.Add(spawn.gameObject);
    }

    public override void OnUpdate() {
        if (counterCooldown < cooldown)
            counterCooldown += Time.deltaTime;
    }

    public override void ChangedEnabledState(bool value) {
        canShoot = value;
        counterCooldown = cooldown;
        if (bullets != -1)
            bulletsCounter = 0;
        else
            bulletsCounter = -2;

        if (outputsSignal) {
            outgoingSignal.value = (bulletsCounter < bullets) ? 1 : 0;
        }

        if (gameObject.activeInHierarchy)
            if (useCutoffEmissionMaterial) {
                StartCoroutine(ChangeEmissionValue(0));
            }
    }

    public override void KillAllSpawns() {
        foreach (GameObject g in bulletSpawns) {
            Destroy(g);
        }
        bulletSpawns.Clear();
    }
}

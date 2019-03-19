//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ParallaxItem : MonoBehaviour {
//    private CameraControl cameraControl;
//    //private float distance;
//    public float customZ = 0;

//    private void Awake() {
//        cameraControl = Camera.main.transform.GetComponent<CameraControl>();
//        //distance = transform.position.z / cameraControl.parallaxMaxDistance;
//        if (transform.position.z > cameraControl.parallaxMaxDistance) {
//            Debug.LogError(gameObject.name + " has more depth than parallax allowed");
//        }
//        Debug.Log("Optimization can be made");
//    }
	
//	void LateUpdate () {
//        //transform.position += cameraControl.deltaPos * distance;
//        if (customZ == 0) {
//            transform.position += cameraControl.deltaPos * transform.position.z / cameraControl.parallaxMaxDistance;
//        } else {
//            transform.position += cameraControl.deltaPos * customZ / cameraControl.parallaxMaxDistance;
//        }
//    }
//}

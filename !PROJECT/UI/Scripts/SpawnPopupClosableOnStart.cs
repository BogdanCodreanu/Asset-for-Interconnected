using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPopupClosableOnStart : MonoBehaviour {
    public float delay = 5f;
    public string title;
    [TextArea]
    public string message;
    public float showDuration = 15;
    void Start () {
        this.ExecuteFunctionWithDelay(delay, delegate { InfoUI.InformationalClosablePopup(title, message, showDuration); });
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalPartSprites : MonoBehaviour {
    public List<SpriteRendererWithPriority> renderers;

    private MechanicalPart mech;

    private void Awake() {
        mech = GetComponent<MechanicalPart>();

        foreach (SpriteRendererWithPriority rend in renderers) {
            rend.rend.sortingOrder = mech.selectionPriority * 1000 + rend.localPriority;
        }
    }
}

public class SpriteRendererWithPriority {
    public SpriteRenderer rend;
    public int localPriority;
}

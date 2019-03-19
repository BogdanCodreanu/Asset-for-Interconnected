using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHover : MonoBehaviour {
    public Transform followedObject;

    private bool following;
    private Color initialColor;
    private int orderInLayer;
    private bool colorChanged;

	public void Init(Transform parent) {
        following = true;
        followedObject = parent;
        transform.position = followedObject.position;
        transform.rotation = followedObject.rotation;
        transform.localScale = followedObject.lossyScale;
        gameObject.layer = 1;
    }
	
	void Update () {
        if (following) {
            if (!followedObject) {
                Destroy(gameObject);
                return;
            }

            transform.position = followedObject.position;
            transform.rotation = followedObject.rotation;
            transform.localScale = followedObject.lossyScale;
        }
	}

    public void StopFollowing() {
        following = false;
    }

    private void OnDestroy() {
        Destroy(gameObject);
    }

    public void ChangeColor(Color newColor, int newOrderInLayer) {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        initialColor = renderer.material.color;
        orderInLayer = renderer.sortingOrder;

        renderer.material.color = newColor;
        renderer.sortingOrder = newOrderInLayer;
        renderer.sortingLayerName = "Selection";
        colorChanged = true;
    }

    public void ResetColor() {
        if (colorChanged) {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.material.color = initialColor;
            renderer.sortingLayerName = "Enchanted Hover";
            renderer.sortingOrder = orderInLayer;
            colorChanged = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationBounds : MonoBehaviour {
    
    private Vector2 topRightCorner;
    private Vector2 bottomLeftCorner;
    public Vector3 boundsScale;
    public GameObject creationBoundsSpritePrefab;
    private SpriteRenderer creationBoundsSpawnSprite;
    public GameObject creationBoundPrefab;

    private GameObject[] boundsSpawn = new GameObject[4];
    private bool needToDelete;

    private void Awake() {
        if (DatabasesManager.IsValidScene())
            topRightCorner = DatabasesManager.GetCreationBoundsCorner();
        bottomLeftCorner = -topRightCorner;
    }

    public void SpawnBounds() {
        DeleteSpawns();
        float xMij, yMij;
        xMij = (topRightCorner.x + bottomLeftCorner.x) * 0.5f;
        yMij = (topRightCorner.y + bottomLeftCorner.y) * 0.5f;
        boundsSpawn[0] = Instantiate(creationBoundPrefab, new Vector3(topRightCorner.x + boundsScale.x * 0.5f, yMij, 0), Quaternion.identity);
        boundsSpawn[1] = Instantiate(creationBoundPrefab, new Vector3(xMij, bottomLeftCorner.y - boundsScale.y * 0.5f, 0), Quaternion.identity);
        boundsSpawn[2] = Instantiate(creationBoundPrefab, new Vector3(bottomLeftCorner.x - boundsScale.x * 0.5f, yMij, 0), Quaternion.identity);
        boundsSpawn[3] = Instantiate(creationBoundPrefab, new Vector3(xMij, topRightCorner.y + boundsScale.y * 0.5f, 0), Quaternion.identity);
        boundsSpawn[0].transform.localScale = boundsScale;
        boundsSpawn[1].transform.localScale = boundsScale;
        boundsSpawn[2].transform.localScale = boundsScale;
        boundsSpawn[3].transform.localScale = boundsScale;
        for (int i = 0; i < boundsSpawn.Length; i++) {
            boundsSpawn[i].hideFlags = HideFlags.HideInHierarchy;
        }

        creationBoundsSpawnSprite = Instantiate(creationBoundsSpritePrefab, new Vector3(xMij, yMij, 0), Quaternion.identity, transform).GetComponent<SpriteRenderer>();
        creationBoundsSpawnSprite.size = new Vector2(topRightCorner.x - bottomLeftCorner.x, topRightCorner.y - bottomLeftCorner.y);

        needToDelete = true;
    }

    public void DeleteSpawns() {
        if (needToDelete) {
            for (int i = 0; i < boundsSpawn.Length; i++) {
                if (boundsSpawn[i])
                    Destroy(boundsSpawn[i]);
            }
            Destroy(creationBoundsSpawnSprite.gameObject);
        }
        needToDelete = false;
    }
}

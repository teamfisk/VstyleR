using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class WarehouseRow : MonoBehaviour {
    public GameObject[] Prefabs;
    public bool ShowInEditor = false;

    BezierCurve curve;

    List<GameObject> furnitures;

    float offset = 0;

    // Use this for initialization
    void Start() {
        curve = GetComponent<BezierCurve>();
        furnitures = new List<GameObject>();

#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += StateChange;
#endif
        spawnPrefabs();
    }

    public void OnEnable() {
        spawnPrefabs();
    }

    void OnDisable() {
        foreach (var item in furnitures) {
            GameObject.DestroyImmediate(item);
        }
        furnitures.Clear();
    }

    void OnDestroy() {
        foreach (var item in furnitures) {
            GameObject.DestroyImmediate(item);
        }
        furnitures.Clear();
    }

    void StateChange() {
        if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying) {
            OnDisable();
        }
    }

    void OnValidate() {
        foreach (var item in furnitures) {
            GameObject.DestroyImmediate(item);
        }
        furnitures.Clear();
        spawnPrefabs();
    }

    void spawnPrefabs() {
        if (furnitures.Count == 0) {
            for (int i = 0; i < Prefabs.Length; i++) {
                var prefab = Prefabs[i];

                GameObject furniture = Instantiate(prefab);
                furniture.transform.SetParent(transform);
                furnitures.Add(furniture);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (Application.isPlaying) {
            offset += Time.deltaTime;
        }

        for (int i = 0; i < furnitures.Count; i++) {
            var furniture = furnitures[i];

            var distance = (float)i / (furnitures.Count - 1) * curve.length + offset;
            distance = Mathf.Repeat(distance, curve.length);
            var pos = curve.GetPointAtDistance(distance);
            furniture.transform.position = pos;
        }
    }
}

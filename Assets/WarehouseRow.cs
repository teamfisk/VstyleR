using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class WarehouseRow : MonoBehaviour {
    public string Category;

    public float Offset { get; set; }

    BezierCurve curve;
    Transform furniture;

    void Awake() {
        curve = transform.Find("Curve").GetComponent<BezierCurve>();
        furniture = transform.Find("Furniture");
    }

    void Start() {
        // Set up text
        transform.Find("Text/Number").GetComponent<TextMesh>().text = (transform.GetSiblingIndex() + 1).ToString();
        transform.Find("Text/Category").GetComponent<TextMesh>().text = Category;
    }

    //void spawnPrefabs() {
    //    if (furnitures.Count == 0) {
    //        for (int i = 0; i < Prefabs.Length; i++) {
    //            var prefab = Prefabs[i];

    //            GameObject furniture = Instantiate(prefab);
    //            furniture.transform.SetParent(transform);
    //            furniture.layer = 8;
    //            furnitures.Add(furniture);
    //        }
    //    }
    //}

    void Update() {
        if (Application.isPlaying) {
            //offset += Time.deltaTime;
        }

        foreach (Transform t in furniture) {
            var i = t.GetSiblingIndex();
            var distance = (float)i / (furniture.childCount) * curve.length + Offset;
            distance = Mathf.Repeat(distance, curve.length);
            var pos = curve.GetUniformPointAtDistance(distance);
            t.position = pos;
        }
    }
}

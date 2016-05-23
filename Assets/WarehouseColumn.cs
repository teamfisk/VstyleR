using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class WarehouseColumn : MonoBehaviour {

    private BezierCurve curve;
    private Transform rows;

    public float Offset { get; set; }

    void Awake() {
        curve = transform.Find("Curve").GetComponent<BezierCurve>();
        rows = transform.Find("Rows");
    }

	void Start () {
	
	}
	
    void Update() {
        if (Application.isPlaying) {
            //offset += Time.deltaTime;
        }

        foreach (Transform row in rows) {
            var i = row.GetSiblingIndex();
            var distance = (float)i / (rows.childCount) * curve.length + Offset;
            distance = Mathf.Repeat(distance, curve.length);
            var pos = curve.GetPointAtDistance(distance);
            row.position = pos;
        }
    }
}

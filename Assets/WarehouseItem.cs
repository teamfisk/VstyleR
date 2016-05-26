﻿using UnityEngine;
using System.Collections;

public class WarehouseItem : FurnitureObject {

    public override void Grab(Transform attachment) {
        var replacement = GameObject.Instantiate(this);
        replacement.transform.parent = transform.parent;
        replacement.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.parent = null;
        foreach (var collider in GetComponentsInChildren<Collider>()) {
            collider.enabled = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;

        var mini = gameObject.AddComponent<WarehouseMiniatureAnim>();
        mini.OriginItem = replacement;
        mini.DestinationAttachment = attachment;
        mini.Duration = 0.33f;
        mini.GoalScale = new Vector3(0.05f, 0.05f, 0.05f);
    }

    public override void Release() {
        var mini = gameObject.GetComponent<WarehouseMiniatureAnim>();
        if (mini) {
            mini.End();
        }

        transform.parent = null;
        foreach (var collider in GetComponentsInChildren<Collider>()) {
            collider.enabled = true;
        }
        GetComponent<Rigidbody>().isKinematic = false;

        gameObject.AddComponent<FurnitureMiniature>();
        Destroy(this);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class FurnitureMiniature : MonoBehaviour {

    public void Grab() {
        transform.parent = null;
        foreach (var collider in GetComponentsInChildren<Collider>()) {
            collider.enabled = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Release() {
        var mini = gameObject.GetComponent<WarehouseMiniatureAnim>();
        if (mini) {
            mini.End();
        }

        transform.parent = null;
        foreach (var collider in GetComponentsInChildren<Collider>()) {
            collider.enabled = true;
        }
        GetComponent<Rigidbody>().isKinematic = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

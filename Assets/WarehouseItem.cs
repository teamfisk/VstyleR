using UnityEngine;
using System.Collections;

public class WarehouseItem : MonoBehaviour {

    public FurnitureMiniature Pick() {
        var clone = GameObject.Instantiate(this);

        foreach (var collider in clone.GetComponentsInChildren<Collider>()) {
            collider.enabled = false;
        }
        clone.GetComponent<Rigidbody>().isKinematic = true;

        // Convert into a miniature
        Destroy(clone.GetComponent<WarehouseItem>());
        var mini = clone.gameObject.AddComponent<FurnitureMiniature>();

        return mini;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RightController : MonoBehaviour {
    public Transform MiniatureAttachmentPoint;

    SteamVR_LaserPointer laserPointer;
    SteamVR_ControllerEvents controllerEvents;
    BoxCollider boxCollider;
    WarehouseColumn warehouse;

    HashSet<GameObject> touchingItems = new HashSet<GameObject>(); 
    GameObject heldItem;

    WarehouseRow whRow = null;
    Vector3? whScrollStartPos = null;
    float whScrollDistance;
    float whBaseOffset = 0.0f;
    float whScrollOffset = 0.0f;
    float whRowBaseOffset = 0.0f;
    float whRowScrollOffset = 0.0f;

    void Awake() {
        laserPointer = GetComponent<SteamVR_LaserPointer>();

        controllerEvents = GetComponent<SteamVR_ControllerEvents>();
        controllerEvents.TriggerClicked += triggerClicked;
        controllerEvents.TriggerUnclicked += triggerUnclicked;
        controllerEvents.TouchpadClicked += touchpadClicked;
        controllerEvents.TouchpadUnclicked += touchpadUnclicked;

        warehouse = GameObject.Find("Warehouse/Shelves/Column").GetComponent<WarehouseColumn>();
    }

    void Start () {
        laserPointer.pointer.layer = 9;
	}

    void Update () {
        // Warehouse scroll
        if (whScrollStartPos != null) {
            Vector3 curPos = transform.position + (transform.forward * whScrollDistance);

            whScrollOffset = (curPos - whScrollStartPos).Value.y;
            whRowScrollOffset = (curPos - whScrollStartPos).Value.x;

            warehouse.Offset = whBaseOffset + (-whScrollOffset);
            whRow.Offset = whRowBaseOffset + whRowScrollOffset;
        }
	}

    void OnTriggerEnter(Collider other) {
        var mini = other.GetComponentInParent<FurnitureMiniature>();
        if (mini == null) {
            return;
        }

        touchingItems.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other) {
        touchingItems.Remove(other.gameObject);
    }

    // Furniture picking
    void triggerClicked(object sender, ControllerClickedEventArgs e) {
        if (heldItem != null) {
            return;
        }

        if (grabByProximity()) {
            return;
        }

        if (grabByLaser()) {
            return;
        }
    }
    void triggerUnclicked(object sender, ControllerClickedEventArgs e) {
        if (heldItem == null) {
            return;
        }

        var furnitureMini = heldItem.GetComponent<FurnitureMiniature>();
        if (furnitureMini) {
            furnitureMini.Release();
        }
        heldItem = null;
    }

    bool grabByProximity() {
        GameObject closest = null;
        foreach (var obj in touchingItems) {
             if (closest == null) {
                closest = obj;
                continue;
            }

            if ((obj.transform.position - boxCollider.bounds.center).magnitude < (closest.transform.position - boxCollider.bounds.center).magnitude) {
                closest = obj;
            }
        }

        if (closest == null) {
            return false;
        }

        var mini = closest.GetComponentInParent<FurnitureMiniature>();
        mini.Grab();
        mini.transform.parent = transform;
        heldItem = mini.gameObject;

        return true;
    }

    bool grabByLaser() {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit result;
        bool hit = Physics.Raycast(ray, out result);

        if (!hit) {
            return false;
        }

        // Warehouse shelf grabbing
        var warehouseItem = result.rigidbody.GetComponent<WarehouseItem>();
        if (warehouseItem) {
            var mini = warehouseItem.Pick();
            mini.Grab();

            var anim = mini.gameObject.AddComponent<WarehouseMiniatureAnim>();
            anim.OriginItem = warehouseItem;
            anim.DestinationAttachment = MiniatureAttachmentPoint;
            anim.GoalScale = new Vector3(0.05f, 0.05f, 0.05f);
            anim.Duration = 0.33f;

            heldItem = mini.gameObject;

            return true;
        }

        // Warehouse miniature grabbing remotely
        var furnitureMini = result.rigidbody.GetComponent<FurnitureMiniature>();
        if (furnitureMini) {
            furnitureMini.Grab();

            var anim = furnitureMini.gameObject.AddComponent<WarehouseMiniatureAnim>();
            //anim.OriginItem = furnitureMini;
            anim.DestinationAttachment = MiniatureAttachmentPoint;
            anim.Duration = 0.33f;

            heldItem = furnitureMini.gameObject;
            return true;
        }

        return false;
    }

    // Furniture scrolling by drag
    void touchpadClicked(object sender, ControllerClickedEventArgs e) {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit result;
        bool hit = Physics.Raycast(ray, out result);

        if (!hit) {
            return;
        }

        var item = result.transform.GetComponentInParent<WarehouseItem>();
        if (!item) {
            return;
        }

        whScrollStartPos = result.point;
        whScrollDistance = result.distance;
        whRow = item.GetComponentInParent<WarehouseRow>();
        whBaseOffset = warehouse.Offset;
        whRowBaseOffset = whRow.Offset;
    }
    void touchpadUnclicked(object sender, ControllerClickedEventArgs e) {
        if (whScrollStartPos != null) {
            whScrollStartPos = null;
        }
    }
}

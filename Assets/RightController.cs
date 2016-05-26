using UnityEngine;
using System.Collections;
using System;

public class RightController : MonoBehaviour {
    public Transform MiniatureAttachmentPoint;

    SteamVR_LaserPointer laserPointer;
    SteamVR_ControllerEvents controllerEvents;
    WarehouseColumn warehouse;

    FurnitureObject heldItem;

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

    RaycastHit raycast() {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(ray, out hit);
        return hit;
    }

    // Furniture picking
    void triggerClicked(object sender, ControllerClickedEventArgs e) {
        if (heldItem != null) {
            return;
        }

        var ray = new Ray(transform.position, transform.forward);
        RaycastHit result;
        bool hit = Physics.Raycast(ray, out result);

        if (!hit) {
            return;
        }

        var item = result.transform.GetComponentInParent<FurnitureObject>();
        if (!item) {
            return;
        }

        item.Grab(MiniatureAttachmentPoint);
        heldItem = item;
    }
    void triggerUnclicked(object sender, ControllerClickedEventArgs e) {
        if (heldItem == null) {
            return;
        }

        heldItem.Release();
        heldItem = null;
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

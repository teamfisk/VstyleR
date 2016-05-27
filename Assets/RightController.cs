using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RightController : MonoBehaviour {
    public Transform MiniatureAttachmentPoint;
    public Material OutlineMaterial;
    public GameObject MainCamera;
    public GameObject WarehouseCamera;

    SteamVR_TrackedObject controller;
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

    Measurement currentMeasurement = null;

    Vector3 lastPos;

    void Awake() {
        controller = GetComponent<SteamVR_TrackedObject>();

        laserPointer = GetComponent<SteamVR_LaserPointer>();

        controllerEvents = GetComponent<SteamVR_ControllerEvents>();
        controllerEvents.TriggerClicked += triggerClicked;
        controllerEvents.TriggerUnclicked += triggerUnclicked;
        controllerEvents.TouchpadClicked += touchpadClicked;
        controllerEvents.TouchpadUnclicked += touchpadUnclicked;
        controllerEvents.ApplicationMenuClicked += applicationMenuClicked;
        controllerEvents.ApplicationMenuUnclicked += applicationMenuUnclicked;

        boxCollider = GetComponent<BoxCollider>();

        warehouse = GameObject.Find("Warehouse/Shelves/Column").GetComponent<WarehouseColumn>();
    }

    void Start () {
        laserPointer.pointer.layer = 9;

        lastPos = transform.position;
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

        // Measurement
        if (currentMeasurement != null) {
            currentMeasurement.EndPosition = transform.localPosition;
        }

        lastPos = transform.position;
	}

    void OnTriggerEnter(Collider other) {
        var mini = other.GetComponentInParent<FurnitureMiniature>();
        if (mini == null) {
            return;
        }

        touchingItems.Add(mini.gameObject);
        //mini.gameObject.AddComponent<Outline>().OutlineMaterial = Resources.Load<Material>("OutLineMaterialMini");
    }

    void OnTriggerExit(Collider other) {
        var mini = other.GetComponentInParent<FurnitureMiniature>();
        if (mini == null) {
            return;
        }
        touchingItems.Remove(mini.gameObject);
        //Destroy(mini.gameObject.GetComponent<Outline>());
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
            /*var device = SteamVR_Controller.Input((int)controller.index);
            var rb = furnitureMini.GetComponent<Rigidbody>();
            rb.velocity = device.velocity;
            rb.angularVelocity = device.angularVelocity;*/
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
            mini.transform.SetParent(MiniatureAttachmentPoint, false);
            mini.transform.localPosition = Vector3.zero;
            mini.transform.localScale = Vector3.one / 10.0f;

            //var anim = mini.gameObject.AddComponent<WarehouseMiniatureAnim>();
            //anim.OriginItem = warehouseItem;
            //anim.DestinationAttachment = MiniatureAttachmentPoint;
            //anim.GoalScale = new Vector3(0.1f, 0.1f, 0.1f);
            //anim.Duration = 0.33f;

            heldItem = mini.gameObject;

            return true;
        }

        // Warehouse miniature grabbing remotely
        var furnitureMini = result.rigidbody.GetComponent<FurnitureMiniature>();
        if (furnitureMini) {
            furnitureMini.Grab();
            if (WarehouseCamera.activeSelf) {
                furnitureMini.transform.SetParent(MiniatureAttachmentPoint, true);
                furnitureMini.transform.localPosition = Vector3.zero;
            } else {
                furnitureMini.transform.SetParent(MiniatureAttachmentPoint, true);
            }

            //var anim = furnitureMini.gameObject.AddComponent<WarehouseMiniatureAnim>();
            //anim.OriginItem = furnitureMini;
            //anim.DestinationAttachment = MiniatureAttachmentPoint;
            //anim.Duration = 0.33f;

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

    void applicationMenuClicked(object sender, ControllerClickedEventArgs e) {
        if (currentMeasurement == null) {
            var prefab = Resources.Load<GameObject>("Measurement");
            currentMeasurement = Instantiate(prefab).GetComponent<Measurement>();
            currentMeasurement.transform.SetParent(GameObject.Find("Scene").transform, false);
            currentMeasurement.StartPosition = transform.localPosition;
            currentMeasurement.EndPosition = transform.localPosition;
        }
    }

    void applicationMenuUnclicked(object sender, ControllerClickedEventArgs e) {
        currentMeasurement = null;
    }
}

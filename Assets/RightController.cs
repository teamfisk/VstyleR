using UnityEngine;
using System.Collections;

public class RightController : MonoBehaviour {
    public GameObject banana;
    public GameObject viveController;

    SteamVR_TrackedController trackedController;
    WarehouseColumn warehouse;

    WarehouseRow whRow = null;
    Vector3? whScrollStartPos = null;
    float whScrollDistance;
    float whBaseOffset = 0.0f;
    float whScrollOffset = 0.0f;
    float whRowBaseOffset = 0.0f;
    float whRowScrollOffset = 0.0f;

    void Awake() {
        trackedController = GetComponent<SteamVR_TrackedController>();
        trackedController.TriggerClicked += triggerClicked;
        trackedController.TriggerUnclicked += triggerUnclicked;

        warehouse = GameObject.Find("Warehouse/Shelves/Column").GetComponent<WarehouseColumn>();
    }

	void Start () {
        GetComponent<ViewFrustrumTrigger>().LeaveView += toggleBanana;
	}

    void toggleBanana() {
        banana.SetActive(!banana.activeSelf);
        viveController.SetActive(!viveController.activeSelf);
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

    void triggerClicked(object sender, ClickedEventArgs e) {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit result;
        bool hit = Physics.Raycast(ray, out result);

        if (!hit) {
            return;
        }

        var item = result.transform.GetComponent<WarehouseItem>();
        if (!item) {
            return;
        }

        whScrollStartPos = result.point;
        whScrollDistance = result.distance;
        whRow = item.GetComponentInParent<WarehouseRow>();
        whBaseOffset = warehouse.Offset;
        whRowBaseOffset = whRow.Offset;
    }

    void triggerUnclicked(object sender, ClickedEventArgs e) {
        if (whScrollStartPos != null) {
            whScrollStartPos = null;
        }
    }
}

using UnityEngine;
using System.Collections;

public class LeftController : MonoBehaviour {

    public GameObject MainCamera;
    public GameObject WarehouseCamera;

    Transform controllerRig;
    SteamVR_ControllerEvents controllerEvents;

    CameraFade cameraFade;

    bool inWarehouse = false;

    void Awake() {
        controllerRig = this.transform.parent;

        controllerEvents = GetComponent<SteamVR_ControllerEvents>();
        controllerEvents.TriggerAxisChanged += triggerAxisChanged;

        cameraFade = MainCamera.GetComponentInChildren<CameraFade>();
    }

	void Start () {
    }
	
    void triggerAxisChanged(object sender, ControllerClickedEventArgs e) {
        float alpha = e.buttonPressure;
        if (alpha > 0.05) {
            cameraFade.SetFade(alpha);
            if (!inWarehouse) {
                WarehouseCamera.SetActive(true);
                controllerRig.SetParent(WarehouseCamera.transform, false);
                inWarehouse = true;
            }
        } else {
            cameraFade.SetFade(0.0f);
            if (inWarehouse) {
                controllerRig.SetParent(MainCamera.transform, false);
                WarehouseCamera.SetActive(false);
                inWarehouse = false;
            }
        }

        foreach (var measurement in GameObject.FindObjectsOfType<Measurement>()) {
            measurement.InWarehouse = inWarehouse;
        }
    }
}

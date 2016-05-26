using UnityEngine;
using System.Collections;

public class LeftController : MonoBehaviour {

    public bool AlwaysCenter = false;

    Transform controllerRig;
    SteamVR_ControllerEvents controllerEvents;

    GameObject mainCamera;
    GameObject warehouseCamera;

    CameraFade cameraFade;
    SteamVR_Camera mainSteamVRCamera;

    bool inWarehouse = false;

    void Awake() {
        controllerRig = this.transform.parent;

        controllerEvents = GetComponent<SteamVR_ControllerEvents>();
        controllerEvents.TriggerAxisChanged += triggerAxisChanged;
    }

	void Start () {
        mainCamera = GameObject.Find("Main Camera (origin)");
        warehouseCamera = GameObject.Find("Warehouse Camera (origin)");

        cameraFade = mainCamera.GetComponentInChildren<CameraFade>();
        mainSteamVRCamera = mainCamera.GetComponentInChildren<SteamVR_Camera>();
    }
	
    void triggerAxisChanged(object sender, ControllerClickedEventArgs e) {
        float alpha = e.buttonPressure;
        if (alpha > 0.05) {
            cameraFade.SetFade(alpha);
            if (!inWarehouse) {
                controllerRig.SetParent(warehouseCamera.transform, false);
                if (AlwaysCenter) {
                    warehouseCamera.transform.localPosition = new Vector3(-mainSteamVRCamera.head.localPosition.x, warehouseCamera.transform.localPosition.y, -mainSteamVRCamera.head.localPosition.z);
                }
                inWarehouse = true;
            }
        } else {
            cameraFade.SetFade(0.0f);
            if (inWarehouse) {
                controllerRig.SetParent(mainCamera.transform, false);
                inWarehouse = false;
            }
        }
    }
}

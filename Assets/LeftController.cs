using UnityEngine;
using System.Collections;

public class LeftController : MonoBehaviour {

    public bool AlwaysCenter = false;


    private Transform controllerRig;
    private SteamVR_TrackedController trackedController;

    private GameObject mainCamera;
    private GameObject warehouseCamera;

    private CameraFade cameraFade;
    private SteamVR_Camera mainSteamVRCamera;

    private bool inWarehouse = false;

	// Use this for initialization
	void Start () {
        controllerRig = this.transform.parent;

        trackedController = GetComponent<SteamVR_TrackedController>();
        trackedController.TriggerClicked += TriggerClicked;
        trackedController.Gripped += Gripped;

        mainCamera = GameObject.Find("Main Camera (origin)");
        warehouseCamera = GameObject.Find("Warehouse Camera (origin)");

        cameraFade = mainCamera.GetComponentInChildren<CameraFade>();
        mainSteamVRCamera = mainCamera.GetComponentInChildren<SteamVR_Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        float alpha = SteamVR_Controller.Input((int)trackedController.controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
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

    void TriggerClicked(object sender, ClickedEventArgs e) {
        //var warehouseCamera = GameObject.FindWithTag("WarehouseCamera").GetComponent<SteamVR_Camera>();
        //warehouseCamera.enabled = true;
        Debug.Log("awd");
    }

    void Gripped(object sender, ClickedEventArgs e) {

    }
}

using UnityEngine;
using System.Collections;

public class LeftController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var trackedController = GetComponent<SteamVR_TrackedController>();
        trackedController.TriggerClicked += TriggerClicked;
        trackedController.Gripped += Gripped;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void TriggerClicked(object sender, ClickedEventArgs e) {
        SteamVR_Fade.Start(Color.black, 2);
        //var warehouseCamera = GameObject.FindWithTag("WarehouseCamera").GetComponent<SteamVR_Camera>();
        //warehouseCamera.enabled = true;
    }

    void Gripped(object sender, ClickedEventArgs e) {
        var warehouseCamera = GameObject.FindWithTag("WarehouseCamera").GetComponent<SteamVR_Camera>();
        warehouseCamera.enabled = false;
    }
}

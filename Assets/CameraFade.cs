using UnityEngine;
using System.Collections;

public class CameraFade : MonoBehaviour {

    private float alpha = 0.0F;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        int deviceIndex = 4;
        alpha = SteamVR_Controller.Input(deviceIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {

    }
}

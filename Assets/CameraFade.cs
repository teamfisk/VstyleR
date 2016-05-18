using UnityEngine;
using System.Collections;
using Valve.VR;

public class CameraFade : MonoBehaviour {

    public RenderTexture BlendTextureLeft;
    public RenderTexture BlendTextureRight;

    private float alpha = 0.0F;

    static Material blitMaterial;

	// Use this for initialization
	void Start () {
        if (!blitMaterial) {
            blitMaterial = new Material(Shader.Find("Custom/TransparencyBitch"));
        }
	}
	
	// Update is called once per frame
	void Update () {
        int deviceIndex = 4;
        alpha = SteamVR_Controller.Input(deviceIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        blitMaterial.SetFloat("_Ratio", alpha);
        if (SteamVR_Render.eye == EVREye.Eye_Left) {
            blitMaterial.SetTexture("_BlendTex", BlendTextureLeft);
        } else {
            blitMaterial.SetTexture("_BlendTex", BlendTextureRight);
        }
        Graphics.Blit(src, dest, blitMaterial);
    }
}

using UnityEngine;
using System.Collections;
using Valve.VR;

[ExecuteInEditMode]
public class PlayAreaScale : MonoBehaviour {
    public float Padding = 0;

    public static bool GetBounds(ref HmdQuad_t pRect)
	{
        var initOpenVR = (!SteamVR.active && !SteamVR.usingNativeSupport);
        if (initOpenVR)
        {
            var error = EVRInitError.None;
            OpenVR.Init(ref error, EVRApplicationType.VRApplication_Other);
        }

        var chaperone = OpenVR.Chaperone;
        bool success = (chaperone != null) && chaperone.GetPlayAreaRect(ref pRect);
        if (!success)
        {
            Debug.LogWarning("Failed to get Calibrated Play Area bounds!  Make sure you have tracking first, and that your space is calibrated.");
        }

        if (initOpenVR)
            OpenVR.Shutdown();

        return success;
    }

	// Use this for initialization
	void OnEnable() {
        StartCoroutine("UpdateBounds");
    }

    // Update is called once per frame
    void Update () {
        
	}

    IEnumerator UpdateBounds()
    {
        var rect = new HmdQuad_t();
        if (!GetBounds(ref rect)) {
            yield return null;
        }
		var corners = new HmdVector3_t[] { rect.vCorners0, rect.vCorners1, rect.vCorners2, rect.vCorners3 };
        float maxX = 0.0f;
        float maxZ = 0.0f;
        foreach (var corner in corners) {
            if (corner.v0 > maxX) {
                maxX = corner.v0;
            }
            if (corner.v2 > maxZ) {
                maxZ = corner.v2;
            }
        }
        maxX += Padding;
        maxZ += Padding;
        maxX *= 2;
        maxZ *= 2;
        transform.localScale = new Vector3(maxX, transform.localScale.y, maxZ);
    }
}

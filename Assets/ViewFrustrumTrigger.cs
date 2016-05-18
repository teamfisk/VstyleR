using UnityEngine;
using System.Collections;

public class ViewFrustrumTrigger : MonoBehaviour {
    public Camera Camera;

    public delegate void ViewFrustrumEventHandler();
    public event ViewFrustrumEventHandler EnterView;
    public event ViewFrustrumEventHandler LeaveView;

    private bool visible = false;

	void Update () {
        Bounds bounds;
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer) {
            bounds = renderer.bounds;
        } else {
            return;
        }

	    if (visible && !IsVisibleFrom(bounds, Camera)) {
            if (LeaveView != null) {
                LeaveView();
            }
            visible = false;
        } else if (!visible && IsVisibleFrom(bounds, Camera)) {
            if (EnterView != null) {
                EnterView();
            }
            visible = true;
        }
    }

    static bool IsVisibleFrom(Bounds bounds, Camera camera) {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}

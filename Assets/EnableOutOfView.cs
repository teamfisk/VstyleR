using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ViewFrustrumTrigger))]
public class EnableOutOfView : MonoBehaviour {
    public Renderer Renderer;

	void Start () {
        GetComponent<ViewFrustrumTrigger>().LeaveView += enable;
	}

    void enable() {
        this.Renderer.enabled = true;
    }
}
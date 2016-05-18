using UnityEngine;
using System.Collections;

public class RightController : MonoBehaviour {
    public GameObject banana;
    public GameObject viveController;

	// Use this for initialization
	void Start () {
        GetComponent<ViewFrustrumTrigger>().LeaveView += toggleBanana;
	}

    void toggleBanana() {
        banana.SetActive(!banana.activeSelf);
        viveController.SetActive(!viveController.activeSelf);
    }

    // Update is called once per frame
    void Update () {
	}
}

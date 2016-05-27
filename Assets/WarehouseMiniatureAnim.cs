using UnityEngine;
using System.Collections;

public class WarehouseMiniatureAnim : MonoBehaviour {

    public WarehouseItem OriginItem;
    public Transform DestinationAttachment;
    public Vector3 GoalScale = Vector3.one;
    public float Duration = 1.0f;

    Vector3 originalPos;
    Vector3 originalScale;
    float alpha = 0.0f;

	void Start () {
        originalPos = transform.position;
        originalScale = transform.localScale;
	}
	
	void Update () {
        alpha += Time.deltaTime / Duration;

        if (alpha > 1) {
            End();
            return;
        }

        var t2 = DestinationAttachment.transform;
        var startPos = OriginItem ? OriginItem.transform.position : originalPos; 
        transform.position = Vector3.Lerp(startPos, t2.position, alpha);
        //transform.rotation = Quaternion.Slerp(transform.rotation, t2.rotation, alpha);
        var startScale = OriginItem.transform.localScale;
        transform.localScale = Vector3.Lerp(startScale, Vector3.Scale(originalScale, GoalScale), alpha);
	}

    public void End() {
        transform.SetParent(DestinationAttachment, false);
        transform.localPosition = Vector3.zero;
        //transform.localScale = originalScale;
        Destroy(this);
    }
}

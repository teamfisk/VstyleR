using System;
using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Measurement : MonoBehaviour {

    public Vector3 StartPosition;
    public Vector3 EndPosition;
    private float HoleSize = 0.1f;
    private Vector3 lineUp = new Vector3(0, 1, 0);
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Transform line1Transform = transform.FindChild("Line1");
        LineRenderer line1Renderer = line1Transform.GetComponent<LineRenderer>();

        Transform line2Transform = transform.FindChild("Line2");
        LineRenderer line2Renderer = line2Transform.GetComponent<LineRenderer>();
        
        Vector3 middlePosition = (StartPosition + (EndPosition - StartPosition) / 2.0f);
        
        float magnitude = (EndPosition - StartPosition).magnitude;
        Vector3 direction = Vector3.Normalize(EndPosition - StartPosition);

        line1Renderer.SetPosition(0, StartPosition);
        line1Renderer.SetPosition(1, middlePosition - (direction * HoleSize));
        
        line2Renderer.SetPosition(0, middlePosition  +(direction * HoleSize));
        line2Renderer.SetPosition(1, EndPosition);

        

        Transform textTransform = transform.FindChild("Text");
	    textTransform.position = middlePosition;

        if (Camera.current != null)
        {
            textTransform.LookAt(Vector3.Normalize(textTransform.position - Camera.current.transform.position) + textTransform.position);
            lineUp = Vector3.Normalize(Vector3.Cross(middlePosition - Camera.current.transform.position, EndPosition - StartPosition));
        }

        TextMesh textMesh = textTransform.GetComponent<TextMesh>();
	    textMesh.text = magnitude.ToString("0.00") + "m";


	    textMesh.characterSize = Mathf.Max(0.001f * Math.Min(1.0f , magnitude), 0.0005f);

        HoleSize = Mathf.Max(0.1f * Math.Min(1.0f, magnitude), 0.05f);
        
        Transform line1BaseTransform = transform.FindChild("Line1Base");
        LineRenderer line1BaseRenderer = line1BaseTransform.GetComponent<LineRenderer>();
        
        line1BaseRenderer.SetPosition(0, StartPosition + lineUp * 0.01f);
        line1BaseRenderer.SetPosition(1, StartPosition - lineUp * 0.01f);


        Transform line2BaseTransform = transform.FindChild("Line2Base");
        LineRenderer line2BaseRenderer = line2BaseTransform.GetComponent<LineRenderer>();

        line2BaseRenderer.SetPosition(0, EndPosition + lineUp * 0.01f);
        line2BaseRenderer.SetPosition(1, EndPosition - lineUp * 0.01f);
    }
}

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PlayAreaWall : MonoBehaviour {
    public enum DirectionEnum
    {
        North,
        South,
        East,
        West
    }

    public DirectionEnum Direction;
    public Transform PlayArea;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Direction == DirectionEnum.North)
        {
            transform.position = new Vector3(0F, transform.localScale.y / 2.0F, PlayArea.localScale.z / 2.0F + transform.localScale.z / 2.0F);
        }
	}
}

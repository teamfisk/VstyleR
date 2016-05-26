using UnityEngine;
using System.Collections;

public class FurnitureObject : MonoBehaviour {
    public virtual void Grab(Transform attachment) { }
    public virtual void Release() { }
}

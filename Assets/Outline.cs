using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Outline : MonoBehaviour {
    public Material OutlineMaterial;

    void Start() {
        OnEnable();
    }

	void OnEnable() {
        foreach (var rend in GetComponentsInChildren<MeshRenderer>()) {
            bool end = false;
            foreach (var mat in rend.sharedMaterials) {
                if (mat == OutlineMaterial) {
                    end = true;
                    break;
                }
            }
            if (end) {
                continue;
            }

            var newMats = rend.materials;
            Array.Resize(ref newMats, newMats.Length + 1);
            newMats[newMats.Length - 1] = OutlineMaterial;
            rend.materials = newMats;
        }
	}
	
    void OnDisable() {
        foreach (var rend in GetComponentsInChildren<MeshRenderer>()) {
            var newMats = new List<Material>();

            foreach (var mat in rend.sharedMaterials) {
                if (mat != OutlineMaterial) {
                    newMats.Add(mat);
                }
            }

            rend.materials = newMats.ToArray();
        }
    }
}

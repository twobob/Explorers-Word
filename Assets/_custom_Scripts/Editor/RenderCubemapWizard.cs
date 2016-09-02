﻿using UnityEngine;
using UnityEditor;
using System.Collections;

// taken from: https://docs.unity3d.com/ScriptReference/Camera.RenderToCubemap.html

public class RenderCubemapWizard : ScriptableWizard {

	public Transform renderFromPosition;
	public Cubemap cubemap;

	void OnWizardUpdate () {
		helpString = "Select transform to render from and cubemap to render into";
		isValid = (renderFromPosition != null) && (cubemap != null);
	}

	void OnWizardCreate () {
		// create temporary camera for rendering
		GameObject go = new GameObject ("CubemapCamera");
		go.AddComponent<Camera>();
		// place it on the object
		go.transform.position = renderFromPosition.position;
		go.transform.rotation = Quaternion.identity;
		// render into cubemap
		go.GetComponent<Camera>().RenderToCubemap( cubemap );

		// destroy temporary camera
		DestroyImmediate( go );
	}

	[MenuItem("-µ-/Render into Cubemap")]
	static void RenderCubemap () {
		ScriptableWizard.DisplayWizard<RenderCubemapWizard>(
			"Render cubemap", "Render!");
	}
}
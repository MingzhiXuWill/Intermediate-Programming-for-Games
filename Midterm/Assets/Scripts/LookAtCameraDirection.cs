using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraDirection : MonoBehaviour
{
	Transform targetToLook;
	void Start()
	{
		targetToLook = Camera.main.transform;
	}

	void Update()
	{
		transform.rotation = Quaternion.LookRotation(-targetToLook.forward, Vector3.up);
	}
}

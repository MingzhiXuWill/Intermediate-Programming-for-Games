using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_SelfDestroy : MonoBehaviour
{
	void Start()
	{
		Destroy(gameObject, GetComponent<ParticleSystem>().main.duration + 2);
	}
}

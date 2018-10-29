using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour {

    ParticleSystem emitter;

	// Use this for initialization
	void Start () {
        emitter = gameObject.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        emitter.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}

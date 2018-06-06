using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class token_spin : MonoBehaviour {

    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.AddTorque(0.0f, -5.0f, 0.0f);
    }

    // Update is called once per frame
    void Update () {
        
	}
}

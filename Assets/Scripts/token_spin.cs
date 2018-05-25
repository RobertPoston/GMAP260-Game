using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class token_spin : MonoBehaviour {

    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.AddTorque(35.0f, 24.0f, 42.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Character")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        
	}
}

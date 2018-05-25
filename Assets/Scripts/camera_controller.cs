using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour {

    public float speed = 50.0f;
    public Transform Player;

    private Vector3 lastPos;

    // Use this for initialization
    void Start () {
        if (Player == null)
            //You'll want to maybe search the tags or something more reasonable here.
            Debug.Log("You forgot to set the player, stupid!");

        lastPos = Player.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Player.position.y > 1.0f)
        {
            //Get the distance the player has moved since last frame.
            Vector3 delta = Player.position - lastPos;
            //Adds this difference to the camera's transform.
            this.transform.position += new Vector3(delta.x, 0.0f, delta.z);
            //Caches the player's current position for next frame.
            lastPos = Player.position;
        }
    }
}

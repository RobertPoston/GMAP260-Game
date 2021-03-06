﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_controller : MonoBehaviour {

    public enum platformType { None, Zdir, Xdir, Glue, Omni, SwitchLeft, SwitchRight };
    public platformType type = platformType.Zdir;
    public platformType subType = platformType.None;
    //public float force = 15000.0f;
    public float maxSpeed = 100.0f;
    public Rigidbody rb;
    public Rigidbody character_rb;
    public character_controller characterPlayer;
    public globals glbls;

    //bool isMoving = false;
    private bool rotate = true;
    private Vector3 oldPosition;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        oldPosition = transform.position;

        glbls = (globals)GameObject.FindObjectOfType(typeof(globals));
	}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Cube" && glbls.platIsMoving)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            glbls.platIsMoving = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Character")
        {
            character_rb.AddForce(0.0f, 0.0001f, 0.0f);
            character_rb.AddForce(0.0f, -0.0001f, 0.0f);
            rotate = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Cube" && glbls.platIsMoving)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            glbls.platIsMoving = false;
            if (transform.childCount > 0)
            {
                if (oldPosition != transform.position)
                {
                    characterPlayer.platformAllowJump = true;
                    characterPlayer.deParent = true;
                }
            }
        }
        if ((collision.gameObject.name == "XdirPlatform" || collision.gameObject.name == "ZdirPlatform" || collision.gameObject.name == "OmniPlatform" || collision.gameObject.name == "StickyPlatform") && glbls.platIsMoving)
        {
            glbls.platIsMoving = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        if (collision.gameObject.name == "Character")
        {
            if (type != platformType.Glue)
            {
                character_rb.constraints = RigidbodyConstraints.FreezeAll;
                character_rb.constraints = RigidbodyConstraints.FreezeRotation;
                if (type == platformType.SwitchLeft && rotate)
                {
                    character_rb.constraints = RigidbodyConstraints.None;
                    rotate = false;
                    if (characterPlayer.current_direction == 3)
                    {
                        character_rb.transform.Rotate(0.0f, -90.0f, 0.0f);
                        characterPlayer.current_direction = 1;
                    }
                    else if (characterPlayer.current_direction == 2)
                    {
                        character_rb.transform.Rotate(0.0f, -90.0f, 0.0f);
                        characterPlayer.current_direction = 0;
                    }
                    else if (characterPlayer.current_direction == 1)
                    {
                        character_rb.transform.Rotate(0.0f, -90.0f, 0.0f);
                        characterPlayer.current_direction = 2;
                    }
                    else
                    {
                        character_rb.transform.Rotate(0.0f, -90.0f, 0.0f);
                        characterPlayer.current_direction = 3;
                    }
                }
                else if (type == platformType.SwitchRight && rotate)
                {
                    character_rb.constraints = RigidbodyConstraints.None;
                    rotate = false;
                    if (characterPlayer.current_direction == 3)
                    {
                        character_rb.transform.Rotate(0.0f, 90.0f, 0.0f);
                        characterPlayer.current_direction = 0;
                    }
                    else if (characterPlayer.current_direction == 2)
                    {
                        character_rb.transform.Rotate(0.0f, 90.0f, 0.0f);
                        characterPlayer.current_direction = 1;
                    }
                    else if (characterPlayer.current_direction == 1)
                    {
                        character_rb.transform.Rotate(0.0f, 90.0f, 0.0f);
                        characterPlayer.current_direction = 3;
                    }
                    else
                    {
                        character_rb.transform.Rotate(0.0f, 90.0f, 0.0f);
                        characterPlayer.current_direction = 2;
                    }
                }
            }
            else if (type == platformType.Glue)
            {
                characterPlayer.platformAllowJump = false;
                character_rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        float xpos, zpos, oldx, oldz;
        bool isEqual = false;
        oldx = transform.position.x;
        oldz = transform.position.z;
        for (xpos = 0.0f; !isEqual; xpos += 10.0f)
        {
            if ((xpos - Mathf.Abs(oldx)) <= 5.0f && (xpos - Mathf.Abs(oldx)) >= -(5.0f))
            {
                isEqual = true;
            }
        }
        xpos -= 10.0f;
        isEqual = false;
        for (zpos = 0.0f; !isEqual; zpos += 10.0f)
        {
            if ((zpos - Mathf.Abs(oldz)) <= 5.0f && (zpos - Mathf.Abs(oldz)) >= -(5.0f))
            {
                isEqual = true;
            }
        }
        zpos -= 10.0f;
        isEqual = false;
        if (Mathf.Sign(oldx) == -1)
        {
            xpos = xpos * -(1.0f);
        }
        if (Mathf.Sign(oldz) == -1)
        {
            zpos = zpos * -(1.0f);
        }
        rb.position = new Vector3(xpos, transform.position.y, zpos);
    }

    private void unfreezeX()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
    }

    private void unfreezeZ()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
    }

    private void FixedUpdate()
    {
        if ((type == platformType.Xdir || type == platformType.Omni || subType == platformType.Xdir || subType == platformType.Omni))
        {
            if (glbls.moveDirection == 1)
            {
                oldPosition = transform.position;
                unfreezeX();
                glbls.platIsMoving = true;
                rb.velocity = new Vector3(maxSpeed, 0.0f, 0.0f);
                glbls.spriteState = 1;
            }
            if (glbls.moveDirection == 2)
            {
                oldPosition = transform.position;
                unfreezeX();
                glbls.platIsMoving = true;
                rb.velocity = new Vector3(-(maxSpeed), 0.0f, 0.0f);
                glbls.spriteState = 2;
            }
        }
        if ((type == platformType.Zdir || type == platformType.Omni || subType == platformType.Zdir || subType == platformType.Omni))
        {
            if (glbls.moveDirection == 3)
            {
                oldPosition = transform.position;
                unfreezeZ();
                glbls.platIsMoving = true;
                rb.velocity = new Vector3(0.0f, 0.0f, maxSpeed);
                glbls.spriteState = 3;
            }
            if (glbls.moveDirection == 4)
            {
                oldPosition = transform.position;
                unfreezeZ();
                glbls.platIsMoving = true;
                rb.velocity = new Vector3(0.0f, 0.0f, -(maxSpeed));
                glbls.spriteState = 4;
            }
        }
        if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
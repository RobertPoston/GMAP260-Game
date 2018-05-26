using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_controller : MonoBehaviour {

    public enum platformType { None, Zdir, Xdir, Glue, Omni, SwitchLeft, SwitchRight };
    public platformType type = platformType.Zdir;
    public platformType subType = platformType.None;
    public float force = 15000.0f;
    public float maxSpeed = 60.0f;
    public Rigidbody rb;
    public Rigidbody character_rb;
    public character_controller characterPlayer;

    private bool enableZcontrol = false;
    private bool enableXcontrol = false;
    private bool isMoving = false;
    private bool rotate = true;

    private Vector3 oldPosition;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        oldPosition = transform.position;

        switch (type)
        {
            case platformType.Omni:
                enableXcontrol = true;
                enableZcontrol = true;
                break;
            case platformType.Zdir:
                enableZcontrol = true;
                break;
            case platformType.Xdir:
                enableXcontrol = true;
                break;
            case platformType.Glue:
                if (subType == platformType.Omni)
                {
                    enableXcontrol = true;
                    enableZcontrol = true;
                }
                else if (subType == platformType.Xdir)
                {
                    enableXcontrol = true;
                }
                else if (subType == platformType.Zdir)
                {
                    enableZcontrol = true;
                }
                break;
        }
	}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Cube" && isMoving && type != platformType.Omni)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            isMoving = false;
        }
        else
        {
            isMoving = false;
        }
        if ((collision.gameObject.name == "XdirPlatform" || collision.gameObject.name == "ZdirPlatform" || collision.gameObject.name == "OmniPlatform" || collision.gameObject.name == "StickyPlatform") && isMoving)
        {
            isMoving = false;
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
        if (collision.gameObject.name == "Cube" && isMoving)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            isMoving = false;
            if (transform.childCount > 0)
            {
                if (oldPosition != transform.position)
                {
                    characterPlayer.platformAllowJump = true;
                    characterPlayer.deParent = true;
                }
            }
        }
        if ((collision.gameObject.name == "XdirPlatform" || collision.gameObject.name == "ZdirPlatform" || collision.gameObject.name == "OmniPlatform" || collision.gameObject.name == "StickyPlatform") && isMoving)
        {
            isMoving = false;
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
        float xpos, zpos;
        for (xpos = 0.0f; !Mathf.Approximately(xpos, Mathf.Abs(transform.position.x)); xpos += 10.0f)
        { }
        xpos = (transform.position.x / Mathf.Abs(transform.position.x)) * xpos;
        for (zpos = 0.0f; !Mathf.Approximately(zpos, Mathf.Abs(transform.position.z)); zpos += 10.0f)
        { }
        zpos = (transform.position.z / Mathf.Abs(transform.position.z)) * zpos;
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
        if (enableXcontrol && !isMoving)
        {
            if (Input.GetKey(KeyCode.D))
            {
                oldPosition = transform.position;
                unfreezeX();
                isMoving = true;
                rb.AddForce(force, 0.0f, 0.0f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                oldPosition = transform.position;
                unfreezeX();
                isMoving = true;
                rb.AddForce(-(force), 0.0f, 0.0f);
            }
        }
        if (enableZcontrol && !isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                oldPosition = transform.position;
                unfreezeZ();
                isMoving = true;
                rb.AddForce(0.0f, 0.0f, force);
            }
            if (Input.GetKey(KeyCode.S))
            {
                oldPosition = transform.position;
                unfreezeZ();
                isMoving = true;
                rb.AddForce(0.0f, 0.0f, -(force));
            }
        }
        if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
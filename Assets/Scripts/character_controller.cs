using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_controller : MonoBehaviour {

    public Rigidbody rb;
    public GameObject character;
    public GameObject jiangshi;
    public GameObject winScreen;
    public GameObject lossScreen;
    public Animator anim;
    public float jump_timer = 2.0f;
    public int current_direction = 0;
    public int room_size_x = 50;
    public int room_size_z = 50;
    //Directions as follows 0: Positive Z; 1: Negative Z; 2: Positive X; 3: Negative X
    public float jumpForceY = 3000.0f;
    public float jumpForceH = 1739.139f;
    public float downForce = 50.0f;
    public bool platformAllowJump = true;
    public bool deParent = false;

    public bool jump = false;

    private bool start = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator jump_wait()
    {
        //Wait until the specified time to begin jumping
        yield return new WaitForSeconds(jump_timer);

        //Begin jumping
        jump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        anim.SetBool("isJumping", false);
        if (collision.gameObject.name == "Cube" || collision.gameObject.name == "ZdirPlatform" || collision.gameObject.name == "XdirPlatform" || collision.gameObject.name == "StickyPlatform" || collision.gameObject.name == "OmniPlatform")
        {
            if (collision.gameObject.name == "StickyPlatform" && transform.parent != collision.gameObject.transform)
            {
                transform.SetParent(collision.gameObject.transform);
            }
            if ((transform.position.z > room_size_z || transform.position.z < -room_size_z || transform.position.x > room_size_x || transform.position.x < -room_size_x) && !start)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            StartCoroutine(jump_wait());
        }
        else if (collision.gameObject.name == "Coffin")
        {
            winScreen.SetActive(true);
        }
    }

    Vector3 deathPosition = new Vector3(-100.0f, -100.0f, -100.0f);

    private void Update()
    {
        if (transform.position.y < -1.0f)
        {
            lossScreen.SetActive(true);
            anim.SetBool("isFalling", true);
            if (deathPosition == new Vector3(-100.0f, -100.0f, -100.0f))
            {
                deathPosition = transform.position;
            }
            jiangshi.transform.position = new Vector3(deathPosition.x, transform.position.y, deathPosition.z);
            jiangshi.transform.rotation = transform.rotation;
        }
        else
        {
            anim.SetBool("isFalling", false);
            jiangshi.transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            jiangshi.transform.rotation = transform.rotation;
        }
    }

    void FixedUpdate()
    {
        if (deParent)
        {
            transform.SetParent(null);
            deParent = false;
        }
        if (transform.position.z > room_size_z && current_direction == 0)
        {
            current_direction = 1;
        }
        if (transform.position.z < -room_size_z && current_direction == 1)
        {
            current_direction = 0;
        }
        if (transform.position.x > room_size_x && current_direction == 2)
        {
            current_direction = 3;
        }
        if (transform.position.x < -room_size_x && current_direction == 3)
        {
            current_direction = 2;
        }

        if (jump == true && platformAllowJump && transform.position.y > -0.005f && transform.position.y < 2.0f)
        {
            if (start)
            {
                start = false;
            }
            rb.constraints = RigidbodyConstraints.None;
            if (current_direction == 3)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                rb.AddForce(-jumpForceH, jumpForceY, 0.0f);
                anim.SetBool("isJumping", true);
            }
            else if (current_direction == 2)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                rb.AddForce(jumpForceH, jumpForceY, 0.0f);
                anim.SetBool("isJumping", true);
            }
            else if (current_direction == 1)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                rb.AddForce(0.0f, jumpForceY, -jumpForceH);
                anim.SetBool("isJumping", true);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                rb.AddForce(0.0f, jumpForceY, jumpForceH);
                anim.SetBool("isJumping", true);
            }
            jump = false;
        }
        if (rb.velocity.y < 0 && transform.position.y > 1.5f)
        {
            rb.AddForce(0.0f, -(downForce), 0.0f);
        }
    }

}

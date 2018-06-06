using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class globals : MonoBehaviour {

    public Image uiguy;
    public Sprite[] sprites = new Sprite[8];
    public GameObject MusicController;
    public AudioSource winSound;
    public float spriteCooldown = 1.0f;
    public bool isComplete = false;
    public bool platIsMoving = false;
    public int spriteState = 0;
    public int[] completeLvls = new int[12];
    public int moveDirection = 0;
    public bool playWinSound = false;

    private bool needsupdate = true;

    private void Start()
    {
        MusicController = GameObject.Find("MusicController");
        winSound = GetComponent<AudioSource>();
    }

    public void CompleteLevel(int level)
    {

        level -= 1;
        completeLvls[level] = 1;

    }

    IEnumerator SpriteUpdate()
    {
        yield return new WaitForSeconds(spriteCooldown);
        spriteState = 0;
    }

    private void Update()
    {
        if (isComplete)
        {
            spriteState = 6;
            MusicController.SetActive(false);
            if (playWinSound)
            {
                winSound.Play();
                playWinSound = false;
            }
        }
        if (spriteState == 1)
        {
            uiguy.sprite = sprites[1];
            needsupdate = true;
        }
        else if (spriteState == 2)
        {
            uiguy.sprite = sprites[2];
            needsupdate = true;
        }
        else if (spriteState == 3)
        {
            uiguy.sprite = sprites[3];
            needsupdate = true;
        }
        else if (spriteState == 4)
        {
            uiguy.sprite = sprites[4];
            needsupdate = true;
        }
        else if (spriteState == 5)
        {
            uiguy.sprite = sprites[5];
            needsupdate = false;
        }
        else if (spriteState == 6)
        {
            uiguy.sprite = sprites[6];
            needsupdate = false;
        }
        else
        {
            uiguy.sprite = sprites[0];
        }

        if (needsupdate)
        {
            needsupdate = false;
            StartCoroutine(SpriteUpdate());
        }

        if (!platIsMoving)
        {
            if (isComplete)
            {
                platIsMoving = true;
            }
            moveDirection = 0;
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveDirection = 1;
                platIsMoving = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                moveDirection = 2;
                platIsMoving = true;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveDirection = 3;
                platIsMoving = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                moveDirection = 4;
                platIsMoving = true;
            }
        }

    }

}

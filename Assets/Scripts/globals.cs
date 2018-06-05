using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class globals : MonoBehaviour {

    public Image uiguy;
    public Sprite[] sprites = new Sprite[8];
    public float spriteCooldown = 1.0f;
    public bool isComplete = false;
    public bool platIsMoving = false;
    public int spriteState = 0;
    public int[] completeLvls = new int[12];
    public int moveDirection = 0;

    private bool needsupdate = true;

    public void CompleteLevel(int level)
    {

        level -= 1;
        completeLvls[level] = 1;

    }

    IEnumerator SpriteUpdate()
    {
        spriteState = 0;

        yield return new WaitForSecondsRealtime(spriteCooldown);
    }

    private void Update()
    {
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
            needsupdate = true;
        }
        else if (spriteState == 6)
        {
            uiguy.sprite = sprites[6];
            needsupdate = true;
        }
        else
        {
            uiguy.sprite = sprites[0];
            needsupdate = true;
        }

        if (needsupdate)
        {
            SpriteUpdate();
            needsupdate = false;
        }

        if (!platIsMoving)
        {
            if (isComplete)
            {
                platIsMoving = true;
            }
            moveDirection = 0;
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection = 1;
                platIsMoving = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection = 2;
                platIsMoving = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection = 3;
                platIsMoving = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection = 4;
                platIsMoving = true;
            }
        }

    }

}

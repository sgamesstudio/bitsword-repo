using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerv2 : MonoBehaviour
{

    public GameObject attackobject;
    public float speed;
    public float jumpHeight;
    Rigidbody2D rb;
    Animator anim;

    private float moveHorizontal;
    private bool jumping, attacking, falling, jumpFrame, jumpforce,flag1, grounded, running;
    private float prevY;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumping = false;
        attacking = false;
        falling = false;
        jumpFrame = false;
        grounded = true;
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControl();
        Input();
        
    }
    //For physics calculations
    private void FixedUpdate()
    {
        MovePlayer();
        CheckFalling();
        CheckGrounded();
        JumpPlayer();
    }

    void Input()
    {
        bool up = UnityEngine.Input.GetKeyDown(KeyCode.UpArrow);
        bool space = UnityEngine.Input.GetKeyDown(KeyCode.Space);

        moveHorizontal = UnityEngine.Input.GetAxisRaw("Horizontal");

        if (!falling && !jumping && !attacking)
        {
            if (moveHorizontal != 0)
            {
                running = true;
            }
            else
            {
                running = false;
            }
        }


        if (up || space)
        {
            if (!attacking && !jumping && !falling)
            {
                if (up)
                {
                    jumping = true;
                    running = false;
                    grounded = false;
                }
                else if (space)
                {
                    StartCoroutine("AttackSeq");
                    attacking = true;
                    running = false;
                    grounded = false;

                }
                
            }
            
        }

        if (falling)
        {
            running = false;
            grounded = false;
            jumping = false;
            falling = true;
        }

        


        print("Attacking" + attacking + " jumping:"+jumping +" falling:"+ falling +" grounded"+ grounded + " running:"+running);
    }
    void AnimationControl()
    {

        if (attacking)
        {
            anim.SetBool("Attack", true);
            anim.SetBool("Run", false);
        }
        if (!attacking)
        {
            anim.SetBool("Attack", false);
        }
     
        if (jumping)
        {
            anim.SetBool("Jump",true);
            anim.SetBool("Fall", false);
            anim.SetBool("Grounded", false);
            anim.SetBool("Run", false);
        }
        if (falling)
        {
            anim.SetBool("Fall", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Grounded", false);
            anim.SetBool("Run", false);
        }
        if (!falling)
        {
            anim.SetBool("Fall", false);
        }
        if (running)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Fall", false);
            anim.SetBool("Jump", false);
        }
        else if (!running)
        {
            anim.SetBool("Run", false);
            
        }

    }
    void MovePlayer()
    {
        //FLIP IMAGE BASED ON DIRECTION
        if (moveHorizontal < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }
        else if (moveHorizontal > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }
        if (!attacking)
        {
            if (jumping)
            {
                if (jumpforce)
                {
                    rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y) * Time.deltaTime;
                }
            }
            else
            {
                rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y) * Time.deltaTime;
            }

        }
    }
    void JumpPlayer()
    {
        if (jumping)
        {
            if (jumpFrame)
            {
                jumpforce = true;
                jumpFrame = false;
                StartCoroutine("JumpSeq");
            }
            if (jumpforce)
            {
                rb.AddForce(Vector2.up * (jumpHeight*Time.deltaTime));
            }
        }
    }

    public void FrameJump()
    {
        jumpFrame = true;
    }

    IEnumerator JumpSeq()
    {
        
        yield return new WaitForSeconds(0.5f);
        jumping = false;
        jumpforce = false;
    }

    IEnumerator AttackSeq()
    {
        attacking = true;
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }



    void CheckFalling()
    {
        if (!flag1)
        {
            prevY = rb.position.y;
            flag1 = true;
            
        }
        else
        {
            if (rb.position.y < prevY)
            {
                falling = true;
            }
            else if (rb.position.y == prevY)
            {
                falling = false;
            }
            flag1 = false;
        }
    }


    void CheckGrounded()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            falling = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    void AttackCol()
    {
        attackobject.GetComponent<AttackControl>().AttackCol();
    }
    void DestroyAttackCol()
    {
        attackobject.GetComponent<AttackControl>().DestoryAttackCol();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject attackobject;

    Rigidbody2D rb;
    Animator anim;

    public float attackTime;


    public bool attacking;
    public bool running;
    public bool jumping;
    public bool falling;
    private bool facingRight;
    private bool facingLeft;

    public float speed;

    private float moveHorizontal;
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        attacking = false;
        jumping = false;
        falling = false;
    }
	
	// Update is called once per frame
	void Update () {
        Input();
        Physics();
        //Debug.Log("isrunning: " + isRunning);
        CheckAnimation();
	}






    void CheckAnimation()
    {

        if (attacking && !jumping)
        {
            anim.SetBool("isAttacking", true);
        }
        else if (jumping)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("isJumping", false);
            //Debug.Log("isrunning: " + isRunning);
            if (running)
            {
                anim.SetBool("isRunning", true);

            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            
            
        }
    }

    void Input()
    {
        Movement();
        Jumping();
        Attacking();
    }

    void Movement()
    {
        moveHorizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
    }
    void Jumping()
    {
        bool down = UnityEngine.Input.GetKeyDown(KeyCode.UpArrow);
        bool held = UnityEngine.Input.GetKey(KeyCode.UpArrow);
        bool up = UnityEngine.Input.GetKeyUp(KeyCode.UpArrow);

        if (down)
        {
            if (!attacking && !jumping && !falling)
            {
                //Debug.Log("Space pressed");
                //isAttacking = true;
                StartCoroutine("JumpSeq");
            }


        }
        else if (held)
        {

        }
        else if (up)
        {

        }
    }
    void Attacking()
    {
        bool down = UnityEngine.Input.GetKeyDown(KeyCode.Space);
        bool held = UnityEngine.Input.GetKey(KeyCode.Space);
        bool up = UnityEngine.Input.GetKeyUp(KeyCode.Space);

        if (down)
        {
            if (!attacking && !jumping)
            {
                //Debug.Log("Space pressed");
                //isAttacking = true;
                StartCoroutine("AttackSeq");
            }


        }
        else if (held)
        {

        }
        else if (up)
        {

        }
    }

    IEnumerator AttackSeq()
    {
        attacking = true;
        yield return new WaitForSeconds(attackTime);
        attacking = false;
    }
    IEnumerator JumpSeq()
    {
        jumping = true;
        yield return 0;
        jumping = false;
    }

    void Physics()
    {
        
        //Debug.Log("moveHorizontal: " + moveHorizontal);
        if(moveHorizontal != 0 && attacking == false)
        {
            running = true;

            MovePlayer();

        }
        else
        {
            running = false;
            rb.velocity = new Vector2(0, rb.velocity.y) * Time.deltaTime;
        }
    }
    void MovePlayer()
    {

        //FLIP IMAGE BASED ON DIRECTION
        if(moveHorizontal < 0)
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
        
        //MOVE IMAGE BASED ON DIRECTION
        rb.velocity = new Vector2(moveHorizontal * speed,rb.velocity.y) * Time.deltaTime;
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

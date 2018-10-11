using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject attackobject;

    Rigidbody2D rb;
    Animator anim;

    public float attackTime;


    public bool isAttacking;
    public bool isRunning;
    public float speed;
    private bool facingRight;
    private bool facingLeft;

    
    private float moveHorizontal;
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        isAttacking = false;
    }
	
	// Update is called once per frame
	void Update () {
        Attack();
        Movement();
        //Debug.Log("isrunning: " + isRunning);
        CheckAnimation();
	}






    void CheckAnimation()
    {

        if (isAttacking)
        {
            anim.SetBool("isAttacking", true);
        }
        else
        {
            //Debug.Log("isrunning: " + isRunning);
            if (isRunning)
            {
                anim.SetBool("isRunning", true);

            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            
            anim.SetBool("isAttacking", false);
        }
    }

    void Attack()
    {


        bool down = Input.GetKeyDown(KeyCode.Space);
        bool held = Input.GetKey(KeyCode.Space);
        bool up = Input.GetKeyUp(KeyCode.Space);

        if (down)
        {
            if (!isAttacking)
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
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
    }

    void Movement()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        //Debug.Log("moveHorizontal: " + moveHorizontal);
        if(moveHorizontal != 0 && isAttacking == false)
        {
            isRunning = true;

            MovePlayer();

        }
        else
        {
            isRunning = false;
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

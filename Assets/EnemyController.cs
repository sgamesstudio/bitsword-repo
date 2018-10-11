using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {


    //originRay is a gameobject which store position that raycast will use for starting position
    public GameObject player,originRay;

    //seeDistance is the length of the raycast
    public float seeDistance;

    //health will be subtracted when hit


    public float health;

    //Needs 2 states... AWARE and UNAWARE

    //UNAWARE: RANDOM STATES - IDLE, WALKING...
    //AWARE: WALKING TOWARDS PLAYER / ATTACK PLAYER

    bool aware;
    public bool dead;

    //Need var for direction
    int direction;

    //bool for animator
    bool walking;

    //need var to id action
    int action;

    //Need var for breaking between actions
    bool alreadyRoaming;

    //bool for seenPlayer() func
    bool seenPlayer;

    //Var for RayCast hit
    RaycastHit2D rayHit;

    //Get objects vars
    Rigidbody2D rb,playerRB;
    Animator anim;

    //Raycast obj
    RaycastHit2D hit;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        direction = getDirection();
        walking = false;
        dead = false;
    }

    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>



    // Update is called once per frame
    void Update()
    {
        //check which direction i am facing
        direction = getDirection();
        //Flip origin
        flipOrigin();
        //Am i aware of the player?
        aware = lookForPlayer();
        

        if (!dead)
        {
            if (aware)
            {
                //print("is aware");
                walking = false;
            }
            else
            {
                //print("not aware");
                roam();
            } 
        }
        //Check animation
        checkAnimation();
    }


    //checks which direction this is facing and flip x-scale accordingly
   int getDirection()
    {
        if (direction == 1)
        {
            //facing right
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
            return 1;
        }
        else
        {
            //facing left
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
            return 2;
        }
    }

    //Flips the originRay gameObject based on direction of this
    void flipOrigin()
    {
        if (direction == 1)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            originRay.transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            originRay.transform.localScale = newScale;
        }
    }
    //Looks for player using RayCast and returns bool 
    bool lookForPlayer()
    {
        //the bool to return - always start at false
        seenPlayer = false;


        //Draw RayCast
        if (direction == 1)
        {
            hit = Physics2D.Raycast(originRay.transform.position, transform.right, seeDistance);
            Debug.DrawRay(originRay.transform.position, Vector2.right * seeDistance);
        }
        else
        {
            hit = Physics2D.Raycast(originRay.transform.position, -transform.right, seeDistance);
            Debug.DrawRay(originRay.transform.position, Vector2.left * seeDistance);
        }

       
        
        //Check if RayCast hit
        if (hit)
        {
            //Debug.Log("hit something");
            if (hit.collider.gameObject.tag == "Player")
            {
                //Debug.Log("hit Player");
                seenPlayer = true;
            }
            if (hit.collider.gameObject.tag == "Enemy")
            {
                //Debug.Log("hit Enemy");
            }
        }
        else
        {
            //Debug.Log("hit Nothing");
        }
        return seenPlayer;
    }
   //When I can't see player, roam
    void roam()
    {
        if (alreadyRoaming)
        {
            if (action == 1)
            {
                rb.velocity = new Vector2(10, rb.velocity.y) * Time.deltaTime;
            }
            else if(action == 2)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y) * Time.deltaTime;
            }
            else
            {
                //nothing
            }
        }
        else
        {
            //Get a random int between 1 and 3
            action = (Random.Range(1, 4));
            if (action == 1)
            {
                //Go Left
                direction = 1;
                walking = true;
                StartCoroutine("doAction");
                alreadyRoaming = true;
                //Set alreadRoaming as true so above alreadyRoaming routine can run until it is set to false
                //by the Coroutine doAction
            }
            if(action == 2)
            {
                walking = true;
                direction = 2;
                //go Right
                StartCoroutine("doAction");
                alreadyRoaming = true;
                //Set alreadRoaming as true so above alreadyRoaming routine can run until it is set to false
                //by the Coroutine doAction
            }
            if (action == 3)
            {
                //Stand still
                walking = false;
                StartCoroutine("doAction");
                alreadyRoaming = true;
                //Set alreadRoaming as true so above alreadyRoaming routine can run until it is set to false
                //by the Coroutine doAction
            }
        }
    }

    IEnumerator doAction()
    {
        //print("do action");
        if (action < 3)
        {
            yield return new WaitForSeconds(4f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        alreadyRoaming = false;
    }
    void checkAnimation()
    {
        if (walking)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        if (dead)
        {
            anim.SetBool("dead", true);
        }
        else
        {
            anim.SetBool("dead", false);
        }
        

    }

    public void kill()
    {
        dead = true;
    }
}

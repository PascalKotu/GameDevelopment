using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    Rigidbody2D rb = default;
    Animator animator = default;

    [SerializeField] float speed = 5f;
    float movingDirection = 0;

    [SerializeField] float jumpSpeed = 7f;
    [SerializeField] float jumpHeigt = 100f;
    bool jump = false;
    bool grounded = false;
    //needed to fine tune the ground-detection
    [SerializeField] float groundOffset = 2f;

    //needed for different attack animations
    [SerializeField] int attacks = 2;
    int currentAttack = 0;
    

    void Start() {
        //get all needed components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update() {
        CheckGrounded();

        //player can jump if he is standing on the ground and if no attack-animation is playing
        if (Input.GetButtonDown("Jump") && grounded && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            jump = true;
        }
        //player can attack if no attack-animation is playing
        if (Input.GetButtonDown("Fire1") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            //starts the attack animation
            animator.SetTrigger("Attack");

            //calculates which animation should play
            animator.SetFloat("AttackAnimation", (float)currentAttack/(float)(attacks - 1));
            currentAttack = (currentAttack + 1) % attacks;
            
        }
        //set other essential variables needed by the state machine
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("Vertical", rb.velocity.y);
    }
    
    private void FixedUpdate() {
        //get the direction the player is moving in
        movingDirection = Input.GetAxisRaw("Horizontal");

        //if player pressd jump since last FixedUpdate call
        if (jump) {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector2.up * jumpHeigt);
            jump = false;
        }

        //Player moves at different speeds whether he is grounded or not
        if (grounded) {
            rb.velocity = new Vector2(movingDirection * speed, rb.velocity.y );
        } else {
            rb.velocity = new Vector2(movingDirection * jumpSpeed, rb.velocity.y);
        }

        //player can't move during attacks on the ground
        if(grounded && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") ) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }


        
        animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        //alligns sprite with walking direction
        if (movingDirection < 0) {
            transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        } else if (movingDirection > 0) {
            transform.GetChild(0).localScale = new Vector3(1, 1, 1);
        }

    }

    void CheckGrounded() {
        //Cast a ray downwards to detect if the player stands on ground
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, groundOffset);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.tag == "Ground") {
                grounded = true;
                break;
            } else {
                grounded = false;
            }
        }
    }
}

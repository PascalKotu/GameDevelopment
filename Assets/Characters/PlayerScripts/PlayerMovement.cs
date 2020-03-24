using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
    Rigidbody2D rb = default;
    Animator animator = default;
    SpriteRenderer sprite = default;

    bool isDead = false;

    [SerializeField] float speed = 5f;
    float movingDirection = 0;
    

    [SerializeField] float jumpSpeed = 7f;
    [SerializeField] float jumpHeigt = 100f;
    [SerializeField] float fallGravity = 2.5f;
    [SerializeField] float jumpGravity = 2f;
    bool jump = false;
    bool grounded = false;
    //needed to fine tune the ground-detection
    [SerializeField] float groundOffset = 2f;

    //needed for different attack animations
    [SerializeField] int attacks = 2;
    int currentAttack = 0;


    [SerializeField] PlayerStats playerStats = default;
    [SerializeField] Transform arrowSpawn = default;
    [SerializeField] GameObject arrow = default;
    int dmg = 1;
    int maxMunition = 3;
    int currentMunition = 0;

    [SerializeField] Material hitMaterial = default;
    Material defaultMaterial = default;

    [SerializeField] List<AudioClip> jumpSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> attackSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> bowSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> hitSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> deathSounds = new List<AudioClip>();

    void Start() {
        //get all needed components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        defaultMaterial = sprite.material;
        GameEvents.PlayerDead.AddListener(SetDead);
        GameEvents.PlayerHit.AddListener(GotHit);
        dmg = playerStats.meleeDamage;
        maxMunition = playerStats.maxMunition;
        currentMunition = playerStats.munition;
    }


    void Update() {
        CheckGrounded();
        if (!isDead) {
            ProcessInput();
        }

        //set other essential variables needed by the state machine
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("Vertical", rb.velocity.y);
    }
    
    private void FixedUpdate() {
        //get the direction the player is moving in
        if (!isDead) {
            movingDirection = Input.GetAxisRaw("Horizontal");
        } else {
            movingDirection = 0;
        }

        //if player pressd jump since last FixedUpdate call
        if (jump) {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector2.up * jumpHeigt);
            GameEvents.PlaySound.Invoke(new AudioEventData(jumpSounds[Random.Range(0, jumpSounds.Count)], 0.2f));
            jump = false;
        }

        if(rb.velocity.y < 0) {
            //gives the fall more impact
            rb.gravityScale = fallGravity;
        }else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            //allows high and low jumps
            rb.gravityScale = jumpGravity;
        } else {
            rb.gravityScale = 1f;
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

    void ProcessInput() {
        //player can jump if he is standing on the ground and if no attack-animation is playing
        if (Input.GetButtonDown("Jump") && grounded && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            jump = true;
        }
        //player can attack if no attack-animation is playing
        //sword
        if (Input.GetButtonDown("Fire1") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !IsMouseOverUI()) {
            //starts the attack animation
            animator.SetTrigger("Attack");
            
            //calculates which animation should play
            animator.SetFloat("AttackAnimation", (float)currentAttack / (float)(attacks - 1));
            currentAttack = (currentAttack + 1) % attacks;

        }
        //bow
        if (Input.GetButtonDown("Fire2") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !IsMouseOverUI()) {
            //starts the attack animation
            animator.SetTrigger("Range");
        }

        if (Input.GetAxis("Vertical") < 0 && grounded && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            GameEvents.PlatformPass.Invoke();
        }
    }

    void SpawnArrow() {
        if(currentMunition > 0) {
            GameObject x = Instantiate(arrow, arrowSpawn.position, Quaternion.identity);
            GameEvents.PlaySound.Invoke(new AudioEventData(bowSounds[Random.Range(0, bowSounds.Count)], 0.7f));
            //let the arrow face into the right direction
            x.transform.localScale = transform.GetChild(0).localScale;
            GameEvents.ChangeMunition.Invoke(-1);
            currentMunition -= 1;
        }
        
    }

    bool IsMouseOverUI() {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if(raycastResults.Count > 0) {
            for (int i = 0; i < raycastResults.Count; i++) {
                if (raycastResults[i].gameObject.tag == "NoClickTrough") {
                    return true;
                }
            }
        }
        
        return false;
    }

    void GotHit(HitData hitData) {
        //apply the hit effects
        if (!isDead) {
            GameEvents.PlaySound.Invoke(new AudioEventData(hitSounds[Random.Range(0, hitSounds.Count)], 0.7f));
        }
        sprite.material = hitMaterial;
        Invoke("ResetHit", 0.2f);
    }

    void ResetHit() {
        //revert the hit effects
        sprite.material = defaultMaterial;
    }

    void PlayAttackSound() {
        GameEvents.PlaySound.Invoke(new AudioEventData(attackSounds[currentAttack % attacks], 0.7f));
    }

    void SetDead() {
        //play death animation and disable any character movement
        if (!isDead) {
            isDead = true;
            GameEvents.PlaySound.Invoke(new AudioEventData(deathSounds[Random.Range(0, deathSounds.Count)], 0.7f));
            rb.velocity = Vector3.zero;
            animator.SetTrigger("Dead");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy") {
            GameEvents.enemyHit.Invoke(new HitData(transform, collision.gameObject, dmg));
            GameEvents.CameraShake.Invoke(0.1f);
        }
    }
}

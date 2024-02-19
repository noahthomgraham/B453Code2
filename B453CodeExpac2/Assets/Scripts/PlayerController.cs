using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private const float ATTACK_TIME = 0.4f; 
    private const byte MAX_HP = 100;


    public float jumpForce;
    public float moveSpeed;
    private float inputX;
    public float damageCooldown;
    public float baseAttack;
    public float attackDmgVariance;
    public AudioClip attackSE;

    public GameObject hpBar;
    private Slider hpBarSlider;
    private TMPro.TextMeshProUGUI hpDisplayValue;
    public byte hp;

    private Animator anim;
    private AudioSource audioSource;
    private Rigidbody2D rigidBody;

    
    private bool playerIsDeath = false;
    private bool isFacingRight = true;
    private bool isJumping = true;
    private bool isAttacking = false;
    private bool isTakingDamage = false;
    
    void Start()
    {
        hp = MAX_HP;
        
        rigidBody = gameObject.GetComponent<Rigidbody2D>(); //this is fine
        anim = GetComponent<Animator>(); //plays running and attacking animation
        audioSource = GetComponent<AudioSource>(); //just for the attck
        
        hpBarSlider = hpBar.GetComponent<Slider>(); //this is for health ui
        hpDisplayValue = hpBar.GetComponentInChildren<TMPro.TextMeshProUGUI>(); //this is for health ui

        MyEvents.OnHealthPickUp.AddListener(addHealth);
        MyEvents.OnPlayerDamaged.AddListener(subtractHealth);

        updateHUD();
    }

    void Update()
    {
       getControllerInput();
       updateAnimation();
            
          
    }

    private void getControllerInput() //overall fine, but jumping needs to be fixed
    {
        inputX = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(inputX * moveSpeed, 0, 0) * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && !isJumping) //this needs to changed
        {
            isJumping = true;
            rigidBody.AddForce(new Vector3(0, 1 * jumpForce, 0), ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking) //fine
        {
            StartCoroutine(performAttack(ATTACK_TIME));
        }
    }

    private void updateHUD()
    {
        hpBarSlider.value = (1.0f * hp) / MAX_HP;
        hpDisplayValue.text = hp + " / " + MAX_HP;
    }

    private void updateAnimation()
    {
        anim.SetBool("isRunning", inputX != 0);
        if (inputX < 0 && isFacingRight)
        {
            isFacingRight = false;
            flipFacingDirection();
        }
        else if (inputX > 0 && !isFacingRight)
        {
            isFacingRight = true;
            flipFacingDirection();
        }
    }

    private void flipFacingDirection() //this is fine since it changes the scale of everything of the player
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1,
        gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }
    
    IEnumerator performAttack(float waitTime)
    {
        isAttacking = true;
        audioSource.PlayOneShot(attackSE);
        anim.SetBool("isAttacking", isAttacking);
        yield return new WaitForSeconds(waitTime);
        
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJumping = false;
        }

        if (!isTakingDamage && collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(waitForDamageCooldown(damageCooldown));
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bullet")
        { 
            if (!isTakingDamage)
            {
                hp -= 10;
                StartCoroutine(waitForDamageCooldown(damageCooldown));
            }
            Destroy(collider.gameObject);
        }
    }

    IEnumerator waitForDamageCooldown(float damageCooldownTime)
    {
        isTakingDamage = true;
        yield return new WaitForSeconds(damageCooldownTime);

        isTakingDamage = false;
    }

    public bool isPlayerDeath()
    {
        return playerIsDeath;
    }

    public void addHealth(byte value)
    { 
        hp += value;
        if(hp >= MAX_HP)
        {
            hp = MAX_HP;
        }
        updateHUD();
    }

    public void subtractHealth(byte value)
    {
        if(hp <= value)
        {
            hp = 0;
            MyEvents.OnPlayerDeath.Invoke();
        }
        hp -= value;
        updateHUD();
    }

    public void changeSpeed(float duration, float modifier)
    {
        StartCoroutine(changeSpeedForGivenTime(duration, modifier));
    }

    IEnumerator changeSpeedForGivenTime(float duration, float modifier)
    {
        moveSpeed *= modifier;
        Debug.Log("new speed: " + moveSpeed);
        yield return new WaitForSeconds(duration);
        moveSpeed /= modifier;
        Debug.Log("rollback speed: " + moveSpeed);
    }

    public void changeGravity(float duration, float modifier)
    {
        StartCoroutine(changeGravityForGivenTime(duration, modifier));
    }

    IEnumerator changeGravityForGivenTime(float duration, float modifier)
    {
        gameObject.GetComponent<SpriteRenderer>().flipY = true;
        Physics2D.gravity *= modifier;
        jumpForce *= modifier;
        yield return new WaitForSeconds(duration);
        gameObject.GetComponent<SpriteRenderer>().flipY = false;
        Physics2D.gravity *= modifier;
        jumpForce *= modifier;
    }
}

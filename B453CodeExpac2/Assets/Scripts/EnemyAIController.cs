using System;
using System.Collections;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    private Transform playerPosition;

    [SerializeField] byte MAX_HP = 100;
    private byte hp;
    public float moveSpeed;
    public byte damageOnHit; //damage to player

    public GameObject fireProjectile;
    public float fireRate;
    public float projectileSpeed;

    private bool isFacingRight = true;
    private bool isPursuingPlayer = false;

    private bool onFiringCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnedEnemyTracker = GameObject.Find("AllEnemiesOnStage");
        transform.SetParent(spawnedEnemyTracker.transform);
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        updateMovement();
        if (!onFiringCooldown && fireProjectile)
        {
            updateFiringProjectile();
        }
    }

    private void updateMovement()
    {
        // check line-of-sight (linear / horizontal)
        float los = gameObject.transform.position.y - playerPosition.position.y;
        if (Mathf.Abs(los) < 2.0f)
        {
            isPursuingPlayer = true;
            // try to move toward player
            if (gameObject.transform.position.x > playerPosition.position.x)
            {
                if (isFacingRight)
                {
                    isFacingRight = false;
                    flipFacingDirection();
                }
            }
            else if (!isFacingRight)
            {
                isFacingRight = true;
                flipFacingDirection();
            }
        }
        else
        {
            isPursuingPlayer = false;
            // walk forward mindlessly
        }

        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void flipFacingDirection()
    {
        moveSpeed *= -1.0f;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1,
            gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    private void updateFiringProjectile()
    {
        // check line of sight - radius
        Vector3 diff = gameObject.transform.position - playerPosition.position;
        float distance = (float) Math.Sqrt(Math.Pow(diff.x, 2f) + Math.Pow(diff.y, 2f));
        
        if (Mathf.Abs(distance) < 8.0f)
        {
            // try to fire toward player
            GameObject bullet = Instantiate(fireProjectile, gameObject.transform.position, Quaternion.identity);
            BulletProperty bulletProp = bullet.GetComponent<BulletProperty>();
            bulletProp.assignSpeedAndDirection(projectileSpeed, 
                (playerPosition.transform.position -  gameObject.transform.position).normalized);
        }

        StartCoroutine(waitForCooldown(fireRate));
    }

    IEnumerator waitForCooldown(float delay)
    {
        onFiringCooldown = true;
        
        yield return new WaitForSeconds(delay);
        
        onFiringCooldown = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.transform.parent == null)
        {
            MyEvents.OnPlayerDamaged.Invoke(damageOnHit);
        }
        
        if (!isPursuingPlayer && (
            collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Enemy" ||
            collision.gameObject.tag == "Item"))
        {
            isFacingRight = !isFacingRight;
            flipFacingDirection();
        }
    }

    public void TakeDamage(byte playerDamage)
    {
        if(playerDamage >= hp)
        {
            gameObject.SetActive(false);
            hp = MAX_HP;
        }
        hp -= playerDamage;
    }
}

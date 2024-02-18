using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    public float projectileSpeed;
    public float attackDecayTime;
    public Vector3 direction;
    
    void Start()
    {
        StartCoroutine(selfDestruct(attackDecayTime));
    }

    void Update()
    {
        gameObject.transform.position +=  Time.deltaTime * projectileSpeed * direction;
    }

    IEnumerator selfDestruct(float decayTime)
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }

    public float getDamage()
    {
        return damage;
    }

    public void setAttackProperties(float damage, float attackDecayTime, float projectileSpeed, Vector3 direction)
    {
        this.damage = damage;
        this.attackDecayTime = attackDecayTime;
        this.projectileSpeed = projectileSpeed;
        this.direction = direction;
    }
}

using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] byte minimumDamage;
    [SerializeField] byte maximumDamage;
    [SerializeField] byte critChance;
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
           byte diceRoll =  (byte) Random.Range(1, 101);
        
           if (diceRoll >= 100 - critChance)
           {
              collision.gameObject.GetComponent<EnemyAIController>().TakeDamage((byte)(2 * maximumDamage));
                Debug.Log("Critical Hit!");
              Debug.Log("Player damage is:" + (byte)(2 * maximumDamage));
           }

           else if (diceRoll - critChance < 0)
           {
                Debug.Log("Critical Fail!");
                Debug.Log("Player damage is: 0");
           }

           else 
           {
              byte normalDamage = (byte)(Random.Range(minimumDamage, (maximumDamage + 1)));
              Debug.Log("Took normal damage");
              Debug.Log("Player damage is: " + normalDamage);
              collision.gameObject.GetComponent<EnemyAIController>().TakeDamage(normalDamage);
           }
        }
    }
}

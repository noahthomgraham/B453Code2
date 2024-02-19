using System;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public enum EffectKind
    {
        HEAL, MOVEMENT, GRAVITY, KILL_ALL, SEAL
    }

    public enum ButtonToSeal
    {
        NONE, JUMP, FIRE, LEFT, RIGHT
    }

    public EffectKind effect;
    public float effectDuration;
    public float amount;
    public byte healAmount;
    public ButtonToSeal sealButton;
    public AudioClip effectSE;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioSource audioSource = other.gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(effectSE);
            PlayerController target = other.gameObject.GetComponent<PlayerController>();
            if (effect == EffectKind.HEAL)
            {
                MyEvents.OnHealthPickUp.Invoke(healAmount);
            }

            if (effect == EffectKind.MOVEMENT)
            {
                target.changeSpeed(effectDuration, amount);
            }

            if (effect == EffectKind.GRAVITY)
            {
                target.changeGravity(effectDuration, amount);
            }

            if (effect == EffectKind.KILL_ALL)
            {
                GameObject enemyTracker = GameObject.Find("AllEnemiesOnStage");
                int enemyCount = enemyTracker.transform.childCount;
                foreach (Transform child in enemyTracker.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + enemyCount);
            }
            
            Destroy(gameObject);
        }
        
        
    }
}
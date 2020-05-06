using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 50;               // The amount of health taken away per attack.
    public float pushbackForce = 200f;
    public AudioClip attackClip;
    public AudioClip swingClip;
    public AudioSource attack;

    Animator anim;                              // Reference to the animator component.
    GameObject enemy;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    bool enemyInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.
    float rangeTimer = 0f;
    float timeBetweenAttacks = 0f;
    AudioSource playerSound;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerSound = GetComponent<AudioSource>();
    }


    void OnTriggerEnter(Collider other)
    {
        enemy = other.gameObject;

        enemyHealth = enemy.GetComponent<EnemyHealth>();

        // If the entering collider is the enemy
        if (other.gameObject.tag == "Enemy")
        {
            enemyInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {

        
        // If the exiting collider is the enemy
        if (other.gameObject.tag == "Enemy")
        {
            enemyInRange = false;
            enemy = null;
            enemyHealth = null;
        }
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;
        timeBetweenAttacks += Time.deltaTime;

        float animationPercent = 0f;
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animationPercent = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        else animationPercent = 0f;

        //Debug.Log(animationPercent);

        //Debug.Log(rangeTimer);

        //// If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (animationPercent > .5 && animationPercent < .75)
        {
           if(enemyInRange && timeBetweenAttacks > 1)
            {
                timeBetweenAttacks = 0f;
                Attack();
                attack.clip = attackClip;
                attack.pitch = Random.Range(.8f, 1.2f);
                attack.Stop();
                attack.Play();

            }
            else
            {
                playerSound.clip = swingClip;
                playerSound.pitch = Random.Range(.8f, 1.2f);
                if(!playerSound.isPlaying) playerSound.Play();
            }

        }

        //// If the player has zero or less health...
        //if (playerHealth.currentHealth <= 0)
        //{
        //    // ... tell the animator the player is dead.
        //    anim.SetTrigger("PlayerDead");    
        //}
    }


    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose...
        if (enemyHealth.currentHealth > 0)
        {
            // ... damage the player.
            enemyHealth.TakeDamage(attackDamage, this.transform, pushbackForce);
        }
    }
}
using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
	public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
	public int attackDamage = 10;               // The amount of health taken away per attack.
    public float pushbackForce = 200f;
    public float animationPercentage = .5f;
    public AudioClip attackClip;

    Animator anim;                              // Reference to the animator component.
	GameObject player;                          // Reference to the player GameObject.
	PlayerHealth playerHealth;                  // Reference to the player's health.
	EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    AudioSource enemyAudio;
	bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
	float timer;                                // Timer for counting up to the next attack.
    float rangeTimer = 0f;
    float timeWithinRange = .2f;
    float animationPercent;

	void Awake()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();
		anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
	}


	void OnTriggerEnter(Collider other)
	{
		// If the entering collider is the player...
		if (other.gameObject == player)
		{
			// ... the player is in range.
			playerInRange = true;
		}
	}


	void OnTriggerExit(Collider other)
	{
		// If the exiting collider is the player...
		if (other.gameObject == player)
		{
			// ... the player is no longer in range.
			playerInRange = false;
		}
	}


	void Update()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) rangeTimer += Time.deltaTime;
        else rangeTimer = 0f;


        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            // ... attack.
            if (rangeTimer < .01f)
            {
                anim.SetTrigger("Attack");
                //play attack sound
                enemyAudio.clip = attackClip;
                enemyAudio.pitch = Random.Range(.8f, 1.2f);
                enemyAudio.Play();
            }

            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                animationPercent = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else animationPercent = 0f;

            //Debug.Log(animationPercent);

            if (animationPercent > animationPercentage && animationPercent < .75)
            {
                //Debug.Log(rangeTimer);
                Attack();
                rangeTimer = 0f;
            }
			
		}

		// If the player has zero or less health...
		if (playerHealth.currentHealth <= 0)
		{
			// ... tell the animator the player is dead.
			anim.SetTrigger("PlayerDead");


        }
	}


	void Attack()
	{
		// Reset the timer.
		timer = 0f;

		// If the player has health to lose...
		if (playerHealth.currentHealth > 0)
		{
			// ... damage the player.
			playerHealth.TakeDamage(attackDamage, this.transform, pushbackForce);
		}
	}
}
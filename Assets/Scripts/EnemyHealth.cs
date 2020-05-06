using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
    public int dropRate = 50;
    public AudioClip deathClip;                 // The sound to play when the enemy dies.
    public AudioClip hurtClip;
    public GameObject mesh;
    public GameObject[] drops;
    public UnityEvent onDeath;


    Animator anim;                              // Reference to the animator.
    AudioSource enemyAudio;                     // Reference to the audio source.
    ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
    SphereCollider sphereCollider;            // Reference to the capsule collider.
    Rigidbody rb;
    RoundManager roundManager;

    bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.
    float timer;
    bool damaged;


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        roundManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoundManager>();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }

    void Update()
    {
        timer += Time.deltaTime;
        FlashWhenHit();

        // If the enemy should be sinking...
        if (isSinking)
        {
            if(timer > 2.5)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
            }

        }
    }


    public void TakeDamage(int amount, Transform enemy, float force)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;
        timer = 0f;

        GetComponent<NavMeshAgent>().enabled = false;

        //player hitback
        rb.AddForce(enemy.forward * force);
        rb.AddForce(enemy.forward * force);
        rb.AddForce(enemy.forward * force);

        GetComponent<NavMeshAgent>().enabled = true;

        // Set the health bar's value to the current health.
        //healthSlider.value = currentHealth;

        //Play the hurt sound effect.
        enemyAudio.clip = hurtClip;
        enemyAudio.pitch = Random.Range(.8f, 1.2f);
        enemyAudio.Play();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // The enemy is dead.
        isDead = true;

        // Turn the collider into a trigger so shots can pass through it.
        sphereCollider.isTrigger = true;

        //drops
        int roll = Random.Range(1, 101);
        if(roll <= dropRate)
        {
            int index = Random.Range(0, drops.Length);

            Instantiate(drops[index], new Vector3(transform.position.x, transform.position.y + .4f, transform.position.z), drops[index].transform.rotation);
        }
        StartSinking();

        // Tell the animator that the enemy is dead.
        anim.SetTrigger("Dead");

        roundManager.enemyCountDecrement();

        Debug.Log(roundManager.enemiesRemaining);

        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }


    public void StartSinking()
    {
        timer = 0f;

        // Find and disable the Nav Mesh Agent.
        GetComponent<NavMeshAgent>().enabled = false;

        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        GetComponent<Rigidbody>().isKinematic = true;

        // The enemy should no sink.
        isSinking = true;

        // Increase the score by the enemy's score value.
        //ScoreManager.score += scoreValue;

        // After 2 seconds destory the enemy.
        Destroy(gameObject, 10f);
    }

    void FlashWhenHit()
    {
        Renderer renderer = mesh.GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time * 5, .5f);
        Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

        Color finalColor;

        if (timer <= .5)
        {
            finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        }
        else
        {
            finalColor = baseColor * Mathf.LinearToGammaSpace(0);
        }

        mat.SetColor("_EmissionColor", finalColor);
    }
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public float stunTime = 10f;
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip hurtClip;                                 // The audio clip to play when the player dies.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    public GameObject mesh;


    Animator anim;                                              // Reference to the Animator component.
    Rigidbody rb;
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    PlayerController playerController;                             // Reference to the player's movement
    bool isDead;                                                // Whether the player is dead.
    float currentSpeed;
    bool damaged;                                               // True when the player gets damaged.
    float timer = 2f;


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update()
    {

        timer += Time.deltaTime;

        if (playerController.speed < 0.1 && timer >= stunTime)
        {
            playerController.speed = currentSpeed;
        }

        FlashWhenHit();

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // If the player has just been damaged...
        if (damaged)
        {
            //    // ... set the colour of the damageImage to the flash colour.
            //    damageImage.color = flashColour;
            
        }
        //// Otherwise...
        //else
        //{
        //    // ... transition the colour back to clear.
        //    damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        //}

        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage(int amount, Transform enemy, float force)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        currentSpeed = playerController.speed;
        playerController.speed = 0f;
        timer = 0f;


        //player hitback
        rb.AddForce(enemy.forward * force);

        // Play the hurt sound effect.
        playerAudio.clip = hurtClip;
        playerAudio.pitch = Random.Range(.8f, 1.2f);
        playerAudio.Play();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // Turn off the movement and shooting scripts.
        playerController.enabled = false;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    Transform t;
    public float bounce = .2f;
    public float speed = 1f;

    public int addHealth = 10;

    PlayerHealth ph;
    AudioSource dropSound;
    float currentY;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        dropSound = GetComponent<AudioSource>();
        t = GetComponent<Transform>();
        currentY = t.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        float val = bounce * Mathf.Sin(Time.time * speed);

        t.position = new Vector3(t.position.x, currentY+val, t.position.z);

    }

    void OnTriggerEnter(Collider other)
    {

        
        // If the entering collider is the player...
        if (other.gameObject.tag == "Player" && timer >= 1f)
        {
            Debug.Log("drop");
            ph = other.GetComponent<PlayerHealth>();
            // ... the player is in range.
            ph.currentHealth += addHealth;
            if (ph.currentHealth > 100) ph.currentHealth = 100;

            //play sound
            dropSound.Play();

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            Destroy(this.gameObject, 1f);
        }

    }
}

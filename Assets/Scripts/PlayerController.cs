using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 2f;
    public float attackPenalty = 3f;
    public float deadzone = .2f;
    public Camera arCamera;
    //public GameObject dustTrail;

    bool isRunning = false;
    bool isAttacking = false;
    bool inAnimation = false;
   

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame  
    void Update()
    {

        //Debug.Log(joystick.Horizontal);
        //Debug.Log(joystick.Vertical); 
        anim.SetBool("isRunning", isRunning);

        //if (isRunning) dustTrail.SetActive(true);
        //else dustTrail.SetActive(false);

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            isAttacking = true;
        }
        else isAttacking = false;
    
        MovePlayer();

    }   

    private void MovePlayer()
    {

        float speedFactor = 0f;

        

        if (joystick.Horizontal >= deadzone || joystick.Horizontal <= -deadzone ||
            joystick.Vertical >= deadzone || joystick.Vertical <= -deadzone)
        {
            speedFactor = speed * Time.deltaTime;
            SetRotation();
            isRunning = true;
        }
        else
        {
            isRunning = false;
            speedFactor = 0f;
        }

        if (isAttacking) speedFactor /= attackPenalty;

        transform.Translate(Vector3.forward * speedFactor, transform);

        //transform.position += new Vector3(horMove, 0f, vertMove);
    }

    private void SetRotation()
    {

        //transform.Rotate()

        var playerBearing = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

        transform.rotation = Quaternion.LookRotation(playerBearing);

        Quaternion cameraBearing = arCamera.transform.rotation;
        cameraBearing.z = 0;
        cameraBearing.x = 0;

        transform.rotation *= cameraBearing;

    }

    public void Attack()
    {
        anim.SetTrigger("toAttack");
    }
}

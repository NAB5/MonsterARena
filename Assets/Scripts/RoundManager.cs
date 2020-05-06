using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerSpawn;
    public GameObject countdown;
    public Text roundText;
    public UnityEvent onWin;
    public UnityEvent onDeath;

    //public Button countdown;
    public Text countDownText;
    public int totalRounds = 3;
    public int enemiesRemaining = 0;
    public float spawnTime = 3f;
    public int enemiesPerRound = 1;
    public int bossRound = 4;


    public AudioClip roundStartClip;
    public AudioClip roundEndClip;
    public AudioClip taunt;
    public AudioClip bossRoundClip;

    GameObject[] enemies;
    EnemyManager em;
    EnvironmentManager env;
    int currentRound = 0;
    int spawnCount = 0;
    bool roundInProgress = false;
    bool roundEnded = false;
    int currentSpawnCount;
    float timer;
    float startTimer;
    AudioSource announcer;
    
    // Start is called before the first frame update
    void Start()
    {
        em = GetComponent<EnemyManager>();
        env = GetComponent<EnvironmentManager>();
        announcer = GetComponent<AudioSource>(); 
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //for(int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].GetComponent<NavMeshAgent>().enabled = false;
        //}

        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerAttack>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        if(player.GetComponent<PlayerHealth>().currentHealth <= 0)
        {
            onDeath.Invoke();
        }

        roundText.text = currentRound.ToString();
        timer += Time.deltaTime;
        if (enemiesRemaining == 0 && !roundEnded)
        {
            //player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<PlayerAttack>().enabled = false;
            //player.transform.position = playerSpawn.transform.position;
            //env.SpawnTerrain();
            announcer.clip = roundEndClip;
            announcer.Play();
            roundEnded = true;
        }
        if (roundEnded)
        {
            if (currentRound == bossRound)
            {
                announcer.clip = taunt;
                announcer.Play();
                onWin.Invoke();
            }
            else
            {
                countdown.SetActive(true);
                startTimer += Time.deltaTime;
                countDownText.text = (Mathf.Abs((int)startTimer - 3)).ToString();
                roundInProgress = false;
            }
        }

        //if all enemies dead on current round, round start if not exceeded total rounds
        if (enemiesRemaining == 0 && currentRound <= totalRounds && !roundInProgress && startTimer >= 3f) {

            countdown.SetActive(false);
            startTimer = 0f;
            roundEnded = false;
            roundStart();
        }

        if(roundInProgress && timer >= spawnTime && currentSpawnCount < spawnCount)
        {
            timer = 0f;
            Debug.Log("Spawn");
            if(currentRound == bossRound)
            {
                em.SpawnBoss();
            }
            else
            {
                em.Spawn();
            }
            
            currentSpawnCount++;
        }

        
    }

    void roundStart()
    {
        announcer.clip = roundStartClip;
        announcer.Play();

        
        roundInProgress = true;
        currentSpawnCount = 0;

        currentRound++;
        spawnCount += enemiesPerRound;

        if (currentRound == bossRound)
        {
            announcer.clip = bossRoundClip;
            announcer.Play();
            spawnCount = 1;
        }

        enemiesRemaining = spawnCount;

        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerAttack>().enabled = true;

        //em.Spawn();

        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].GetComponent<NavMeshAgent>().enabled = true;
        //    enemies[i].GetComponent<Animator>().SetTrigger("RoundStart");
        //}
    }

    public void enemyCountDecrement() { enemiesRemaining--; }
}

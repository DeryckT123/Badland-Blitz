using System.Collections;
using System.Collections.Generic;
//using Unity.AI.Navigation;
using TMPro;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    bool isPlaying = false;
    IEnumerator PlaySounds()
    {
       
        isPlaying = true;


        yield return new WaitForSeconds(2);

        if (!audioSource.isPlaying)
        {
            audioSource.clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
            audioSource.Play();
        }

        yield return new WaitForSeconds(3);
        isPlaying = false;
    }

    
    public GameObject player, lava, turret, dragon;
    public Animator enemyAnimator;
    public float damage = .00001f;
    public float health = 100f;
    public GameManager gameManager;
    public float raycastDistance = 50f;

    public Slider slider;

    public bool playerInReach;
    private float attackDelayTimer;
    public float attackAnimStartDelay;
    public float delayBetweenAttacks;

    public AudioSource audioSource;
    public AudioClip[] zombieSounds;

    Vector3 destination;
    NavMeshAgent agent;

    public TextMeshProUGUI scoreWord;
    public static double score = 0;

    void Start()
    {
        // Cache agent component and destination
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        lava = GameObject.FindGameObjectWithTag("Lava");
        turret = GameObject.FindGameObjectWithTag("Turret");
        dragon = GameObject.FindGameObjectWithTag("Dragon");
        if (gameManager.GetComponent<GameManager>().round == 1)
        {
            score = 0;
        }
        scoreWord = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        slider.maxValue = health;
        slider.value = health;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlaySounds());
        }
        slider.transform.LookAt(player.transform);
        //GetComponent<NavMeshAgent>().destination = player.transform.position;
        //Debug.Log(player.transform.position);
        destination = player.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        if (Vector3.Distance(destination, player.transform.position) < 1.0f)
        {
            destination = player.transform.position;
            agent.destination = destination;
          //  Debug.Log("Enemy " + agent.destination + "    Player " + destination);
        }

        //if (GetComponent<NavMeshAgent>().velocity.magnitude >= 0 && Vector3.Distance(agent.destination, player.transform.position) > 1.5f)
        if(GetComponent<NavMeshAgent>().velocity.magnitude > 0 && !playerInReach)
        {
            //Debug.Log(agent.transform.position + "      " + destination);
            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            //Debug.Log(GetComponent<NavMeshAgent>().velocity.magnitude);
            enemyAnimator.SetBool("isRunning", false);
        }

        //if (player.GetComponent<PlayerManager>().isDead == true || gameManager.GetComponent<GameManager>().restart == true || player.GetComponent<PlayerManager>().health <= 10)
        //{
        //    Debug.Log("HAHAHAHAHAH no..........");
        //    score = 0;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerInReach = true;
        }
    }

    public void Hit(float damage)
    {
        health -= damage;
        slider.value = health;
        score += 20;
        scoreWord.text = "Score: " + score.ToString();
        if (health <= 0)
        {
            enemyAnimator.SetTrigger("isDead");
            gameManager.enemiesAlive--;
            Destroy(gameObject, 10f);
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<EnemyManager>());
            Destroy(GetComponent<CapsuleCollider>());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (playerInReach)
        {
            attackDelayTimer += Time.deltaTime;
        }
        else
            attackDelayTimer = 0;

        //if (attackDelayTimer >= delayBetweenAttacks - attackAnimStartDelay && attackDelayTimer <= delayBetweenAttacks && playerInReach)
        //{
            enemyAnimator.SetTrigger("isAttacking");
        //}

        if (attackDelayTimer >= delayBetweenAttacks && playerInReach)
        {
            player.GetComponent<PlayerManager>().Hit(damage);
            attackDelayTimer = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject == player)
        //{
            playerInReach = false;
            attackDelayTimer = 0;
        //}
    }
}

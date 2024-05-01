using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class dragonManager : MonoBehaviour
{
    public GameObject player, lava, turret;
    public Animator enemyAnimator;
    int IdleSimple;
    int FlyingFWD;
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
        enemyAnimator = GetComponent<Animator>();
        IdleSimple = Animator.StringToHash("IdleSimple");
        FlyingFWD = Animator.StringToHash("FlyingFWD");
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if (!audioSource.isPlaying)
        //{
        //     audioSource.clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
        //     audioSource.Play();
        // }


        //GetComponent<NavMeshAgent>().destination = player.transform.position;
        //Debug.Log(player.transform.position);
        if (Vector3.Distance(destination, player.transform.position) > 1.0f)
        {

            destination.x = player.transform.position.x;
            destination.z = player.transform.position.z;
            agent.destination = new Vector3(destination.x, agent.transform.position.y, destination.z);

           // Debug.Log("Enemy " + agent.destination + "    position " + agent.transform.position);
        }
        Debug.Log(agent.transform.position + "    " + agent.destination);
        if (GetComponent<NavMeshAgent>().velocity.magnitude < 1)
        {
            //enemyAnimator.SetBool("isRunning", true);
            //enemyAnimator.SetBool(FlyingFWD, true);
            enemyAnimator.SetBool(FlyingFWD, true);
        }
        else
        {
            //enemyAnimator.SetBool(FlyingFWD, true);
            enemyAnimator.SetBool(IdleSimple, true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject == player)
        {
            playerInReach = true;
        }*/
    }

    public void Hit(float damage) //Dragon Gets hit
    {
        //health -= damage;
        //slider.value = health;
        //score += 20;
        //scoreWord.text = "Score: " + score.ToString();
        /*if (health <= 0)
        {
            enemyAnimator.SetTrigger("isDead");
            gameManager.enemiesAlive--;
            Destroy(gameObject, 10f);
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<EnemyManager>());
            Destroy(GetComponent<CapsuleCollider>());
        }*/
    }

    private void OnCollisionStay(Collision collision)
    {
        /*if (playerInReach)
        {
            attackDelayTimer += Time.deltaTime;
        }
        else
            attackDelayTimer = 0;

        if (attackDelayTimer >= delayBetweenAttacks - attackAnimStartDelay && attackDelayTimer <= delayBetweenAttacks && playerInReach)
        {
            enemyAnimator.SetTrigger("isAttacking");
        }

        if (attackDelayTimer >= delayBetweenAttacks && playerInReach)
        {
            player.GetComponent<PlayerManager>().Hit(damage);
            attackDelayTimer = 0;
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
        /*if (collision.gameObject == player)
        {
            playerInReach = false;
            attackDelayTimer = 0;
        }*/
    }
}

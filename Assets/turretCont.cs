using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class turretCont : MonoBehaviour
{
    Transform Player;
    float dist, damage = 20f;
    public float health = 100f;
    public Image images;
    public GameManager gameManager;
    public bool died = false, shot;

    public AudioSource source;
    public AudioClip clip;

    public float howClose, fireRate, nextFire, projectileSpeed;

    public Transform head, barrel1, barrel2;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Player.GetComponent<PlayerManager>().isDead == true)
        {
            died = true;
        }
        
        dist = Vector3.Distance(Player.position, transform.position);
        if(dist <= howClose ) 
        {
            head.LookAt(Player);
            if (Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                
                shoot();
                
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player)
        {
            //DMG(damage);
            Debug.Log("HIT");
        }
    }

    public void shoot()
    {
        shot = true;
        

        GameObject clone1 = Instantiate(bullet, barrel1.position, head.rotation);
        GameObject clone2 = Instantiate(bullet, barrel2.position, head.rotation);

        

        //forward force
        clone1.GetComponent<Rigidbody>().AddForce(head.forward * projectileSpeed);
        clone2.GetComponent<Rigidbody>().AddForce(head.forward * projectileSpeed);

        if (clone1.transform.position == Player.position)
            Debug.Log("Hit");
        if (clone2.transform.position == Player.position)
            Debug.Log("Hit");

        source.clip = clip;
        source.Play();

        Destroy(clone1, 1);
        Destroy(clone2, 1);
        
    }
}

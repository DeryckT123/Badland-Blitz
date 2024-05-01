using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dragonDamage : MonoBehaviour
{
    Transform Player;//, terrain;
    float dist, damage = 20f;
    public float health = 100f;
    public GameManager gameManager;
    public bool died = false;

    Rigidbody bRigidBody;

    float TimeT;

    public float howClose, fireRate, nextFire, projectileSpeed;

    public Transform head, barrel1, barrel2;
    public GameObject bullet, pp, terrain, dragon, path;

    public AudioSource source;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        terrain = GameObject.FindGameObjectWithTag("MeshGen");
        dragon = GameObject.FindGameObjectWithTag("Dragon");
        pp = GameObject.FindGameObjectWithTag("Player");
        path = GameObject.FindGameObjectWithTag("DragonPath");
        bRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pp.GetComponent<PlayerManager>().isDead == true)
        {
            died = true;
        }

        dist = Vector3.Distance(Player.position, transform.position);
        if (dist <= howClose)
        {
            //Debug.Log("distance " +  dist);
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
            bRigidBody.constraints = RigidbodyConstraints.FreezePosition;
        }
        if (collision.gameObject == terrain.transform)
        { 
            bRigidBody.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    void shoot()
    {
        GameObject clone1 = Instantiate(bullet, barrel1.position, head.rotation);

        


        //forward force
        clone1.GetComponent<Rigidbody>().AddForce((head.forward * projectileSpeed) + new Vector3(Random.Range(-1500, 1500), 0, Random.Range(-1500, 1500)));

        if (clone1.transform.position == Player.position)
        {
            Debug.Log("Hit");
        }

        //Physics.IgnoreCollision(clone1.GetComponent<Collider>(), path.GetComponent<Collider>());

        source.clip = clip;
        source.Play();

        //if (Vector3.Distance(pp.transform.position, clone1.transform.position) <= 25)
        //{
        ///    Destroy(clone1, 0);
        //}
        //if (clone2.transform.position == Player.position)
        //    Debug.Log("Hit");

        Destroy(clone1, 1f); //Add a feature that tracks player and dragon distance, then shoot if they are in a certain range
        //Destroy(clone, 1);
    }
}

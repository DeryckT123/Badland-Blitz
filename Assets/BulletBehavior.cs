using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BulletBehavior : MonoBehaviour
{
    Transform Player;
    float dist, damage = 20f;
    public float health = 100f;
    public Image images;
    public GameManager gameManager;
    GameObject path;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        path = GameObject.FindGameObjectWithTag("DragonPath");
        ParticleSystem particle = GetComponent<ParticleSystem>();
        particle.Play();
        Physics.IgnoreCollision(GetComponent<Collider>(), path.GetComponent<Collider>());

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "First_Person_Player (1)")
        {
           // Debug.Log("Entered collision with " + collision.gameObject.name);
            DMG(damage);   
        }
        else
            Debug.Log("No");
        /*if (collision.gameObject == Player)
        {
            //DMG(damage);
            Debug.Log("HIT");
        }
    }*/

    public void DMG(float damage)
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonMesh : MonoBehaviour
{
    public NavMeshSurface surface;

    public AudioSource audioSource;
    public AudioClip clip;

    public GameObject fireball, path;

    bool isPlaying = false;
    
    IEnumerator PlaySounds()
    {

        isPlaying = true;


        yield return new WaitForSeconds(2);

        if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        yield return new WaitForSeconds(16);
        isPlaying = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
        fireball = GameObject.FindGameObjectWithTag("fireball");
        path = GameObject.FindGameObjectWithTag("DragonPath");
        //Physics.IgnoreCollision(fireball.GetComponent<Collider>(), GetComponent<Collider>(), true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == "fireball")
        //{
            //Physics.IgnoreCollision(collis, path.GetComponent<Collider>());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isPlaying)
        {
            StartCoroutine(PlaySounds());
        }
    }
}

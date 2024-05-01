using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiosss : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public GameObject turret;

    void Start()
    {
        turret = GameObject.FindGameObjectWithTag("Turret");
    }

    // Update is called once per frame
    void Update()
    {
        if (turret.GetComponent<turretCont>().shot)
        {
            source.PlayOneShot(clip);
            source.Play();
        }
    }
}

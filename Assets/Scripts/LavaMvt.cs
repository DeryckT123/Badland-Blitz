using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMvt : MonoBehaviour
{
    public float scrollSpeedx;
    public float scrollSpeedy;
    Renderer rend;
    GameObject player;
    public bool died = false;
    



    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer> ();
        player = GameObject.FindGameObjectWithTag("Player");
        //zombie = GameObject.FindGameObjectWithTag("Zombie");
        //EnemyManager em = new EnemyManager();

        //score = zombie.GetComponent<EnemyManager>().score;
        //double score = em.score;
    }

    // Update is called once per frame
    void Update()
    {

        if (player.GetComponent<PlayerManager>().isDead == true)
        {
            died = true;
        }
        rend.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup * scrollSpeedx, Time.realtimeSinceStartup * scrollSpeedy);
    }
}

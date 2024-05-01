using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    public GameObject player;
    void start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject == player)
        //{
        player.GetComponent<PlayerManager>().health--;
        // player.GetComponent<PlayerManager>().images.fillAmount = player.GetComponent<PlayerManager>().health/100f;
        //Debug.Log(player.GetComponent<PlayerManager>().health);
        //player.GetComponent<PlayerManager>().images.fillAmount = (player.GetComponent<PlayerManager>().health / 100f);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

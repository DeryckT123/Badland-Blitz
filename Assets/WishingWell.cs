using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishingWell : MonoBehaviour, IInteractable
{
    public GameObject player;

    public void Interact()
    {
        //Debug.Log("YOU TOUCHED A WELL");
        if (player.GetComponent<PlayerManager>().health > 0 && player.GetComponent<PlayerManager>().health < 100)
        {
            //Debug.Log(player.GetComponent<PlayerManager>().health);
            player.GetComponent<PlayerManager>().heal(5);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}

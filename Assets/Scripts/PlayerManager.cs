using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour { 
    public Image images;
    public float health = 100f;

    public TextMeshProUGUI healthNum;
    public GameManager gameManager;
    public GameObject playerCamera, player;
    public CanvasGroup hurtPanel;
    private float shakeTime;
    private float shakeDuration;
    private Quaternion playerCameraOriginalRotation;

    public bool isDead = false;

    GameObject wishingWell;

    public AudioSource source;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start() {
        // playerCameraOriginalRotation = playerCamera.transform.localRotation;
        wishingWell = GameObject.FindGameObjectWithTag("Well");
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update() {
        //transform.Rotate(0, 0, 0);

        if (hurtPanel.alpha > 0) {
            hurtPanel.alpha -= Time.deltaTime;
        }
        if (shakeTime < shakeDuration) {
            shakeTime += Time.deltaTime;
            CameraShake();
        } else if(playerCamera.transform.localRotation != playerCameraOriginalRotation) {
            playerCamera.transform.localRotation = playerCameraOriginalRotation;
        }

        //if (Physics.Linecast(Player.transform.position, wishingWell.transform.position))
       // {
       //     Debug.Log("Looking at well");
            //if (Input.GetKeyDown(KeyCode.E))//&& playerCamera.transform.localRotation == )
            //{

               // heal(5);
         //   }
        //}
       // Debug.Log(health);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Projectile(Clone)")
        {
            //Debug.Log("Entered collision with " + collision.gameObject.name);
            Hit(20);
        }
        
        if (collision.gameObject.name == "Fireball(Clone)")
        {
            //Debug.Log("Entered collision with " + collision.gameObject.name);
            Hit(40);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.name == "Lava")
        {
           // Debug.Log("Entered collision with " + collision.gameObject.name);
            Hit(1f);
            ParticleSystem particle = GetComponent<ParticleSystem>();
            particle.Play();
        }
    }

    public void Hit(float damage) {
        health -= damage/15;//change this to modify health bar decrease speed
        images.fillAmount = (health / 100f);
        // Debug.Log(health);
        source.clip = clip;
        source.Play();

        //healthNum.text = health.ToString() + " Health ";
        if (health <= 0) {
            images.fillAmount = 0f;
            isDead = true;
            gameManager.EndGame();
        } else {
            shakeTime = 0;
            shakeDuration = .2f;
            hurtPanel.alpha = .7f;
        }


    }

    public void heal(float heals)
    {
        if (health >= 0)
        {
            health += heals;//change this to modify health bar decrease speed
            images.fillAmount = (health / 100f);
        }
    }

    public void CameraShake()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(Random.Range(-2, 2), 0, 0);
    }
}

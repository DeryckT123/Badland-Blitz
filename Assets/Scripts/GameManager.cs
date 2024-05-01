using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public int enemiesAlive = 0;

    public int round = 0;

    public LayerMask groundMask;

    public bool restart;

    public GameObject Player, buildingRotate;
    public GameObject DragonPath;

    public GameObject[] spawnPoints;

    public GameObject enemyPrefab;
    public GameObject dragon;

    public GameObject pauseMenu;

    public TextMeshProUGUI roundNum;
    public TextMeshProUGUI roundsSurvived;
    public GameObject endScreen;

    public Animator blackScreenAnimator;

    Transform wishingWell;

   // public objectScript;

    void Start()
    {
        //PositionRaycast();
        Cursor.lockState = CursorLockMode.Locked;
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.LookAt(Vector3.up);
        DragonPath = GameObject.FindGameObjectWithTag("DragonPath");
        buildingRotate = GameObject.FindGameObjectWithTag("rotate");
        restart = false;
        GameObject enemyDragon = Instantiate(dragon, Player.transform.position + new Vector3(300, DragonPath.transform.position.y - Player.transform.position.y, 300), Quaternion.identity);
        enemyDragon.GetComponent<dragonManager>().gameManager = GetComponent<GameManager>();
        //  objectScript = 
    }

    // Update is called once per frame
    void Update() {
        //healthNum.text = "Health " + player.health.ToString();
        if (enemiesAlive == 0) {
            round++;
            NextWave(round);
            roundNum.text = "Round: " + round.ToString();
        }

        

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    public void NextWave(int round) {
        
        for (int i = 0; i < round; i++) {

            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, 100f))
            {
                //Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                //GameObject clone = Instantiate(obj, hit.point, spawnRotation);
                //Player.transform.position = new Vector3(hit.point.x + Random.Range(35, 300), hit.point.y + Random.Range(0, 10), hit.point.z + Random.Range(35, 300));
                //GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject spawnPoint = spawnPoints[0];

                GameObject enemySpawned = Instantiate(enemyPrefab, Player.transform.position + new Vector3(6, hit.point.y-1f, 6), Quaternion.identity);
                GameObject enemySpawned2 = Instantiate(enemyPrefab, Player.transform.position + new Vector3(7, hit.point.y-1f, 7), Quaternion.identity);
                GameObject enemySpawned3 = Instantiate(enemyPrefab, Player.transform.position + new Vector3(8, hit.point.y-1f, 8), Quaternion.identity);
                //GameObject enemyDragon = Instantiate(dragon, Player.transform.position + new Vector3(30, hit.point.y+70f, 30), Quaternion.identity);
                
                
                enemySpawned.GetComponent<EnemyManager>().gameManager = GetComponent<GameManager>();
                enemySpawned2.GetComponent<EnemyManager>().gameManager = GetComponent<GameManager>();
                enemySpawned3.GetComponent<EnemyManager>().gameManager = GetComponent<GameManager>();
                
                //enemiesAlive += 3;
                enemiesAlive += 3 ;
               // Debug.Log("PPWorking!!!");
               

            }
           /// else
                //Debug.Log("PPNOOOO");
           
        }
    }

    public void EndGame() {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        endScreen.SetActive(true);
        roundsSurvived.text = round.ToString();
        Player.GetComponent<PlayerManager>().isDead = true;

    }

    public void ReplayGame() {
        //Debug.Log("Game Restarting!!!!!!!!");
        
        SceneManager.LoadScene("CopyScene");
        buildingRotate.GetComponent<objectScript>().startGame();
        restart = true;
        Player.GetComponent<PlayerManager>().isDead = true;
        Time.timeScale = 1;
        round = 0;
    }

    public void MainMenu() {
        Time.timeScale = 1; 
        AudioListener.volume = 1;
        blackScreenAnimator.SetTrigger("FadeIn");
        Invoke("LoadMainMenuScene", 0f);
    }

    void LoadMainMenuScene() {
        SceneManager.LoadScene(0);
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.volume = 0;
    }

    public void UnPause() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.volume = 1;
    }
}

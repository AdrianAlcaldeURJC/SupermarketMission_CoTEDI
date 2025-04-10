using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclesGame : MonoBehaviour
{
    private int MAX_NUMOBSTACLES = 50;

    [SerializeField] private GameObject[] obstacle;
    [SerializeField] private GameObject sideObstacle;
    [SerializeField] private GameObject stand;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject CongratsPanel;

    [SerializeField] TMP_Text numObstaclesText;

    private Vector3[] spawnPoints = { new Vector3(-4.5f, 0, 63.2000008f), new Vector3(0, 0, 63.2000008f), new Vector3(4.5f, 0, 63.2000008f) };
    private Vector3[] standSpawnPoints = { new Vector3(11.0100002f, 3.72000003f, 63.7299995f), new Vector3(-10.79f, 3.72000003f, 63.7299995f) };
    private Quaternion[] charactersRotation = {new Quaternion(0, 0, 0, 1), new Quaternion(0, -0.707106829f, 0, 0.707106829f), new Quaternion(0, 0.707106829f, 0, 0.707106829f), new Quaternion(0, 0.382683426f, 0, 0.923879564f), new Quaternion(0, -0.382683426f, 0, 0.923879564f) };
    //private Vector3[] standSpawnPoints = { new Vector3(-11.5100002f, 2.99039865f, 63.8699989f), new Vector3(11.7370729f, 2.99039841f, 63.8699989f) };

    private int playerLifes;
    private bool isGameOver;
    private int numObstacles;

    [SerializeField] private List<Image> hearts = new List<Image>(3);

    [SerializeField] private LevelLoader introLoader;
    private ExplanationCanvas explanationCanvas;

    // Start is called before the first frame update
    void Start()
    {
        this.GameOverPanel.SetActive(false);
        this.CongratsPanel.SetActive(false);
        introLoader = FindObjectOfType<LevelLoader>();

        if (!GameManager.GetInstance().firstTimeScene[6])
            StartGame();

        explanationCanvas = FindObjectOfType<ExplanationCanvas>();
        explanationCanvas.SetTextChecking(6, "Hemos llegado al tramo final de esta misión, hay que salir de este lugar. ¿Serás capaz de esquivar todos los obstáculos que hay de camino a las cajas? " +
                        "Te deseo muy buena suerte, Agente " + GameManager.GetInstance().playerInitial + ". \nDesplázate con las flechas del teclado y ten cuidado de no quedarte sin vidas.");
        
        numObstacles = 0;
        isGameOver = false;
        playerLifes = hearts.Count-1;
        numObstaclesText.text = numObstacles + "/"+ MAX_NUMOBSTACLES;
        AudioManager.GetInstance().PlayMusicClip(AudioManager.GetInstance().obstaclesSceneMusic);
        StartCoroutine(SpawnStands());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        Quaternion rotation;
        while (!isGameOver&&numObstacles< MAX_NUMOBSTACLES)
        {
            var index = Random.Range(0, 3);
            rotation = new Quaternion(0, 0, 0, 1);
            yield return new WaitForSeconds(1f);
            if (index != 1)
            {
                var rand = Random.Range(0, obstacle.Length + 1);
                if (rand == obstacle.Length)
                {
                    var obs = Instantiate(sideObstacle);
                    obs.gameObject.transform.SetPositionAndRotation(spawnPoints[index], new Quaternion(0, 0, 0, 1));
                }
                else
                {
                    if (rand == 0 || rand == 1)
                    {
                        var rand2 = Random.Range(0, charactersRotation.Length);
                        rotation = charactersRotation[rand2];
                    }
                    var obs = Instantiate(obstacle[rand]);
                    obs.gameObject.transform.SetPositionAndRotation(spawnPoints[index], rotation);
                }
            }
            else
            {
                var rand = Random.Range(0, obstacle.Length);
                if(rand ==0|| rand == 1)
                {
                    var rand2 = Random.Range(0, charactersRotation.Length);
                    rotation = charactersRotation[rand2];
                }
                var obs = Instantiate(obstacle[rand]);
                obs.gameObject.transform.SetPositionAndRotation(spawnPoints[index], rotation);
            }
        }
        isGameOver = true;
        GameOver();
    }

    IEnumerator SpawnStands()
    {
        while (!isGameOver && numObstacles < MAX_NUMOBSTACLES)
        {
            yield return new WaitForSeconds(1.2f);
            if ((!isGameOver && numObstacles < MAX_NUMOBSTACLES))
            {
                var obs = Instantiate(stand);
                var obs2 = Instantiate(stand);
                obs.gameObject.transform.SetPositionAndRotation(standSpawnPoints[0], new Quaternion(0, 0, 0, 1f));
                obs2.gameObject.transform.SetPositionAndRotation(standSpawnPoints[1], new Quaternion(0, 1f, 0, 0));
            }
        }
    }

    public void DamagePlayer()
    {
        if (!isGameOver)
        {
            Debug.Log("AUCH");
            hearts[playerLifes].gameObject.SetActive(false);
            playerLifes--;
            if (playerLifes < 0)
            {
                this.GameOver();
            }
        }
    }

    public void ObstacleReachedTheEnd()
    {
        this.numObstacles++;
        numObstaclesText.text = numObstacles + "/"+ MAX_NUMOBSTACLES;
        Debug.Log(numObstacles);
    }

    private void GameOver()
    {
        ObstacleMovement[] elements = FindObjectsOfType<ObstacleMovement>();
        foreach(ObstacleMovement elem in elements)
        {
            elem.SetStop(true);
        }
        Debug.Log("GameOver");
        this.isGameOver = true;
        //Pausar carrito y obstaculos
        //Mostrar canvas de GameOver para pasar a la siguiente escena
        if (playerLifes >= 0)
            this.CongratsPanel.SetActive(true);
        else
            this.GameOverPanel.SetActive(true);
    }

    public void OnClickedContinue()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
        introLoader.LoadNextLevel("FinalCinematic");
    }
}

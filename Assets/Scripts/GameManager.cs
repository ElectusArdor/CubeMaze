using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player, mCamera, pauseMenu, victoryPanel, gameOverPanel, prison;
    [SerializeField] private GameObject[] floorPrefabs, floorSpawnPoints;
    [SerializeField] private Image timerPool;
    [SerializeField] private float maxTime; //  Max game time

    private List<GameObject> floors = new List<GameObject>();
    private List<FinishController> finishs = new List<FinishController>();
    private List<Transform> starts = new List<Transform>();
    private int lvl;
    private float tmr;
    private bool stopTimer, started;

    void Start()
    {
        tmr = maxTime;

        lvl = 0;

        //  Create random level
        for (int i = 0; i < floorSpawnPoints.Length; i++)
        {
            floors.Add(Instantiate(floorPrefabs[Random.Range(0, floorPrefabs.Length)]) as GameObject);  //  Choose a random prefab
            floors[i].transform.SetParent(floorSpawnPoints[i].transform, false);
            floors[i].transform.localPosition = Vector3.zero;
            floors[i].transform.localRotation *= Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);

            //  Random flip the level on x/z axis
            int scaleX, scaleZ;
            if (Random.Range(0, 2) == 0) scaleX = -1; else scaleX = 1;
            if (Random.Range(0, 2) == 0) scaleZ = -1; else scaleZ = 1;
            if (scaleX < 0 | scaleZ < 0)
            {
                FloorProperty fp = floors[i].GetComponent<FloorProperty>();
                for (int k = 0; k < fp.walls.Length; k++)
                {
                    fp.walls[k].localPosition = new Vector3(scaleX * fp.walls[k].localPosition.x,
                        fp.walls[k].localPosition.y, scaleZ * fp.walls[k].localPosition.z);
                }
            }

            finishs.Add(floors[i].GetComponent<FloorProperty>().finish);
            starts.Add(floors[i].GetComponent<FloorProperty>().startPosition);
        }

        prison.SetActive(false);
    }

    /// <summary>
    /// Start the game
    /// </summary>
    public void OnStartClick()
    {
        prison.SetActive(true);
        player.transform.position = starts[0].transform.position;
        mCamera.GetComponent<MoveCamera>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        started = true;
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    private void Victory()
    {
        victoryPanel.SetActive(true);
        OutOfGame();
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        OutOfGame();
    }

    /// <summary>
    /// Used during between games
    /// </summary>
    private void OutOfGame()
    {
        Time.timeScale = 0;
        started = false;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void Timer()
    {
        if (started)
        {
            tmr -= Time.deltaTime;

            if (tmr <= 0)
            {
                GameOver();
                stopTimer = true;
                tmr = 0;
            }

            timerPool.fillAmount = tmr / maxTime;
        }
    }

    public void OnPause()
    {
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnPause();

        //  Check for start next lvl or victory
        if (finishs[lvl].isFinished == true)
        {
            finishs[lvl].isFinished = false;

            if (lvl == floorSpawnPoints.Length - 1)
                Victory();
            else
            {
                lvl++;
                player.transform.position = starts[lvl].position;
            }
        }

        if (!stopTimer)
            Timer();
    }
}

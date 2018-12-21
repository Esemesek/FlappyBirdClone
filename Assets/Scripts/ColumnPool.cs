using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour
{


    private BetterRandom betterRandom;

    public static ColumnPool instance;
    public int columnPoolSize = 7;
    public float spawnRate = 1.2f; //1.2f
    public float columnMin = -2.5f;
    public float columnMax = 2.3f;
    public GameObject columnPrefab;

    private GameObject[] columns;
    private float timeSinceLastSpawned = 3;
    private float spawnXPosition = 10f;
    private int currentColumn = 0;

    private GameObject firstColumn;
    private bool firstColumnInit;

    void Awake()
    {
        betterRandom = new BetterRandom();
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InitFirstColumn()
    {
        float spawnYPosition = Random.Range(columnMin, columnMax);
        firstColumn = (GameObject)Instantiate(ColumnPool.instance.columnPrefab, new Vector2(6.5f, spawnYPosition), Quaternion.identity);
    }

    // Use this for initialization
    void Start()
    {
        firstColumnInit = false;
        Vector2 objectPoolPosition = new Vector2(-15f,-25f);

        columns = new GameObject[columnPoolSize];
        for (int i = 0; i < columnPoolSize; i++)
        {
            columns[i] = (GameObject)Instantiate(columnPrefab, objectPoolPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!GameController.instance.gameOver && timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0;

            float spawnYPosition = (float) betterRandom.Range(columnMin, columnMax);//Random.Range(columnMin, columnMax);
            columns[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);

            currentColumn++;

            if (currentColumn > 6)
            {
                currentColumn = 0;
            }
        }
    }
    
    public void ResetColumns()
    {
        Vector2 orginalPosition = new Vector2(-15f, -25f);
        for (int i = 0; i < columnPoolSize; i++)
        {
            columns[i].transform.position = orginalPosition;
        }
        float spawnYPosition = Random.Range(columnMin, columnMax);
        firstColumn.transform.position = new Vector2(6.5f, spawnYPosition);
    }
}
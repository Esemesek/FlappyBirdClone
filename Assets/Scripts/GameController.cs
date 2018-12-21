using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject gameOverText;
    public Text scoreText;
    public bool gameOver = false;
    public float scrollSpeed = -3f;

    private int score = 0;

    public GameObject startPrefab;
    private GameObject start;

    // Use this for initialization
    void Awake () {
		if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
	}

    private void Start()
    {
        start = (GameObject)Instantiate(startPrefab, new Vector2(0f, 0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
		if (gameOver && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}

    public void BirdScored() {
        if (gameOver) {
            return;
        }
        score++;
        scoreText.text = "Score: " + score.ToString();
    }

    public void BirdDied() {
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void ResetCamera()
    {
        Vector2 orginalPosition = new Vector2(0f, 0f);
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = orginalPosition;
    }

    public void ResetBackground()
    {
        Vector2[] orginalPositionGrounds = new Vector2[]
        {
            new Vector2(-4f, -2.6f),
            new Vector2(16.48f, -2.6f),
            new Vector2(36.96f, -2.6f),
            new Vector2(57.44f, -2.6f)
        };
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        for(int i = 0; i < grounds.Length; i++)
        {
            grounds[i].transform.position = orginalPositionGrounds[i];
        }
    }

    public void ResetLevel()
    {
        ColumnPool.instance.ResetColumns();
        GeneticController.instance.ResetBirds();
        //ResetCamera();
        ResetBackground();
        score = 0;
        start.transform.position = new Vector2(-1f, 0f);
    }

    public GameObject getStartObject()
    {
        return start;
    }
}

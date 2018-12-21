using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticController : MonoBehaviour {

    public static GeneticController instance;
    private GeneticAlgorithm<double> ga;
    int populationSize = 10;
    int dnaSize = 15;
    float mutationRate = 0.02f;//0.05f;
    int elitism = 5;
    private System.Random random;

    public static float genMin = -20f;
    public static float genMax = 20f;

    public GameObject birdPrefab;
    private GameObject[] birds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        ColumnPool.instance.InitFirstColumn();
        random = new System.Random();
        ga = new GeneticAlgorithm<double>(populationSize, dnaSize, random, GetRandomGen, FitnessFunction, elitism, mutationRate);

        GenerateBirds();
    }

    // Update is called once per frame
    void FixedUpdate () {
        bool allDead = true;
        for(int i = 0; i < birds.Length; i++)
        {
            if(!birds[i].GetComponent<Bird>().isDead)
            {
                allDead = false;
            }

        }
		if(allDead)
        {
            ga.NewGeneration();
            GameController.instance.ResetLevel();
            for (int i = 0; i < populationSize; i++)
            {
                birds[i].GetComponent<Bird>().UpdateWeights(ga.Population[i].Genes);
            }
        }
	}

    private void GenerateBirds()
    {
        Vector2 objectPoolPosition = new Vector2(0f, 0f);
        birds = new GameObject[populationSize];
        for (int i = 0; i < populationSize; i++)
        {
            birds[i] = (GameObject)Instantiate(birdPrefab, objectPoolPosition, Quaternion.identity);
            birds[i].GetComponent<Bird>().InitBirdNeuralNetwork(ga.Population[i].Genes);
        }

        for(int i = 0; i < populationSize; i++)
        {
            for(int j = i + 1; j < populationSize; j++)
            {
                Physics2D.IgnoreCollision(birds[i].GetComponent<Collider2D>(), birds[j].GetComponent<Collider2D>());
            }
        }

    }

    private double GetRandomGen()
    {
        return Random.Range(genMin, genMax);
    }

    private float FitnessFunction(int index)
    {
        float score = 0;

        score = birds[index].GetComponent<Bird>().GetBirdPosition();// - (float) birds[index].GetComponent<Bird>().distance;
        //if (GameController.instance.lastScore != 0)
        //    score *= GameController.instance.lastScore;
        //else
        //    score *= 0.5f;
        //Debug.Log(score);
        return score;
    }

    public void ResetBirds()
    {
        Vector2 orginalPosition = new Vector2(0f, 0f);
        for (int i = 0; i < populationSize; i++)
        {
            birds[i].transform.position = orginalPosition;
            birds[i].GetComponent<Bird>().isDead = false;
        }
    }

    public GameObject[] getBirds()
    {
        return birds;
    }

}

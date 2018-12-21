using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneticController : MonoBehaviour {

    public static GeneticController instance;
    private GeneticAlgorithm<double> ga;
    int populationSize = 10;
    int dnaSize = 15;
    float mutationRate = 0.1f;//0.05f;
    int elitism = 4;

    public static float genMin = 0f;
    public static float genMax = 2f;

    public GameObject birdPrefab;
    private GameObject[] birds;

    private int first = 0;

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
        ga = new GeneticAlgorithm<double>(populationSize, dnaSize, GetRandomGen, FitnessFunction, elitism, mutationRate);

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
            if(GameController.instance.score == 0 && (first == 0 || first > 10) )
            {
                ga = new GeneticAlgorithm<double>(populationSize, dnaSize, GetRandomGen, FitnessFunction, elitism, mutationRate);
                first = 1;
            }
            else
            {
                ga.NewGeneration();
            }

            if (GameController.instance.score == 0)
            {
                first++;
            }
            else
            {
                first = 1;
            }




            GameController.instance.ResetLevel();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //

            Debug.Log("Nr populacji:" + ga.Generation);
            
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
        BetterRandom random = new BetterRandom();
        return random.Range(genMin, genMax);
        //return Random.Range(genMin, genMax);
    }

    private float FitnessFunction(int index)
    {
        float score = 0;

        //score = birds[index].GetComponent<Bird>().GetBirdPosition() + (1 - (float) birds[index].GetComponent<Bird>().simpleDistance);
        //if (score < 0)
        score = birds[index].GetComponent<Bird>().GetBirdPosition();
        return score;
    }

    public void ResetBirds()
    {
        Vector2 orginalPosition = new Vector2(0f, 0f);
        for (int i = 0; i < populationSize; i++)
        {
            birds[i].transform.position = orginalPosition;
            birds[i].GetComponent<Bird>().isDead = false;
            birds[i].GetComponent<Bird>().SetAnimatorIdle();
        }
    }

    public GameObject[] getBirds()
    {
        return birds;
    }

}

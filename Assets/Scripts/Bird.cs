using System.Linq;
using UnityEngine;

public class Bird : MonoBehaviour {

    public float upForce = 400f;
    public int rotationSpeed = 4;
    public int rotationDelay = 24;
    public int minAngle = -90;
    public int maxAngle = 30;

    private int delay = 0;
    private int rotation = 0;
    public bool isDead { get; set; }
    private Rigidbody2D rb2d;
    private Animator animator;

    SimpleNeuralNetwork network = new SimpleNeuralNetwork(2);

    GameObject[] goalObjects;
    Transform[] transforms;
    public Transform closestColumn;
    public double distance;
    public double simpleDistance;

    public double[] weights { get; set; }
    
    // Use this for initialization
    void Start () {
        isDead = false;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //weights = new double[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, //hidden layer
        //                        1, 1, 1, 1, 1 }; //outputlayer
    }

    public void InitBirdNeuralNetwork(double[] weights)
    {
        this.weights = weights;
        NeuralNetworkInitializer();
    }

    public void UpdateWeights(double[] weights)
    {
        network.UpdateWeightsLayers(weights);
    }

        // Update is called once per frame
    void FixedUpdate () {
	    if (!isDead) {
            if(Input.GetMouseButtonDown(0))
            {
                Flap();
            }
            else if (rotation > minAngle && delay == 0) {
                rotation -= rotationSpeed;
            }
 
            // Delay rotation of bird after 'Flap'
            if (delay > 0) {
                delay--;
            }
 
            rb2d.MoveRotation(rotation);

            UpdateDistanceMeasure();

            //w gore 5.4
            //w dol -2.685

            //miedzy kolumnami 3.66
            
            double yDistance = (transform.position.y - closestColumn.position.y + 8.085f) / 16.17f;
            double xDistance = 0;
            if (GameController.instance.score > 0)
                xDistance = distance / 8.875f;
            else
                xDistance = distance / 10.374f;
            
            if (xDistance > 1)
                xDistance = 1;



            network.PushInputValues(new double[] {(1 - yDistance), xDistance*0});
            var outputs = network.GetOutput();

            //Debug.Log("y: " + yDistance);
            //Debug.Log("x: " + xDistance);
            //Debug.Log("Output: " + outputs.First());

            if (outputs.First() == 1)
            {
                Flap();              
            }
        }
    }

    private void UpdateDistanceMeasure()
    {
        goalObjects = GameObject.FindGameObjectsWithTag("MiddlePoint");
        transforms = goalObjects.Select(y => y.transform).ToArray();
        closestColumn = GetClosestColumn(transforms);
        //distance = GetDistanceBetweenColumnObjectAndBird(closestColumn);
        distance = GetSqrtDistanceBetweenColumnObjectAndBird(closestColumn);
        simpleDistance = GetDistanceBetweenColumnObjectAndBird(closestColumn);
    }

    public float GetBirdPosition()
    {
        return GameController.instance.getStartObject().transform.position.x * -1 + transform.position.x;
    }

    private double GetDistanceBetweenColumnObjectAndBird(Transform objectTransform)
    {
        double distance = objectTransform.position.x - transform.position.x;
        if (distance < 0)
            return 0;
        else
            return distance;
    }

    private double GetSqrtDistanceBetweenColumnObjectAndBird(Transform objectTransform)
    {
        float distance = objectTransform.position.x - transform.position.x;
        if (distance < 0)
            distance = 0;
        return Mathf.Sqrt(
                Mathf.Pow(objectTransform.position.y - transform.position.y, 2)
                +
                Mathf.Pow(distance, 2)
            );
    }

    private Transform GetClosestColumn(Transform[] transforms)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in transforms)
        {
            if (potentialTarget.position.x < 0) continue;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            //close and ahead of us
            if (dSqrToTarget < closestDistanceSqr && potentialTarget.position.x > currentPosition.x)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
    

    public void NeuralNetworkInitializer()
    {

        var layerFactory = new NeuralLayerFactory();
        network.AddLayer(layerFactory.CreateNeuralLayer(5, weights.Take(10).ToArray(), new StepActivationFunction(0.5),
                                                        new WeightedSumFunction()));
        network.AddLayer(layerFactory.CreateNeuralLayer(1, weights.Skip(10).Take(5).ToArray(), new StepActivationFunction(0.5), //
                                                        new WeightedSumFunction()));
    }

    public void Flap()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2(0, upForce));
        animator.SetTrigger("Flap");

        // Rotation
        rotation = maxAngle;
        delay = rotationDelay;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        rb2d.velocity = Vector2.zero;
        isDead = true;
        animator.SetTrigger("Die");


        //GameController.instance.BirdDied();
    }

    public void SetAnimatorIdle()
    {
        animator.ResetTrigger("Die");
    }
   
}


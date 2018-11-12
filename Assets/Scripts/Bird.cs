using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    public float upForce = 400f;
    public int rotationSpeed = 4;
    public int rotationDelay = 24;
    public int minAngle = -90;
    public int maxAngle = 30;

    private int delay = 0;
    private int rotation = 0;
    private bool isDead = false;
    private Rigidbody2D rb2d;
    private Animator animator;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!isDead) {
            if(Input.GetMouseButtonDown(0)) {
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(0, upForce));
                animator.SetTrigger("Flap");
                
                // Rotation
                rotation = maxAngle;
                delay = rotationDelay;
            } else if (rotation > minAngle && delay == 0) {
                rotation -= rotationSpeed;
            }
 
            // Delay rotation of bird after 'Flap'
            if (delay > 0) {
                delay--;
            }
 
            rb2d.MoveRotation(rotation);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        rb2d.velocity = Vector2.zero;
        isDead = true;
        animator.SetTrigger("Die");
        GameController.instance.BirdDied();
    }
}

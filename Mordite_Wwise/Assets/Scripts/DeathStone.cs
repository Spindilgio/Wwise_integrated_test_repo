using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class DeathStone : MonoBehaviour
{

    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        velocity = Vector3.ClampMagnitude(velocity, 1);
        transform.position += velocity * Time.deltaTime;
    }

    public void AddVelocity(Vector3 velocityToAdd)
    {
        velocity += velocityToAdd;
    }

    public void FinishingTouch(float distance)
    {
        Vector3 push = new Vector3(transform.position.x + distance, 0, 0);
        transform.position += push;
    }
}
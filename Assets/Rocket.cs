using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            Thrust();
            Debug.Log("huh");
        }

        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        } else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
    }

    private void RotateLeft()
    {
        transform.Rotate(Vector3.forward);
    }

    private void RotateRight()
    {
        transform.Rotate(Vector3.back);

    }

    private void Thrust()
    {
        rb.AddRelativeForce(Vector3.up);
        Debug.Log("thrust");
    }
}

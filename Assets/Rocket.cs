using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;

    AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
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
            if (!sound.isPlaying)
            {
                sound.Play();
            }
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

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            sound.Stop();
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

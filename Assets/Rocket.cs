using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;

    AudioSource sound;


    [SerializeField]
    float mainThrust = 10f;

    [SerializeField]
    float rsThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            if (!sound.isPlaying)
            {
                sound.Play();
            }
            rb.AddRelativeForce(Vector3.up * mainThrust);
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            sound.Stop();
        }
    }

    private void Rotate()
    {
        rb.freezeRotation = true; //nullify environmental impact on rotation
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rsThrust * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rsThrust * Time.deltaTime);
        }

        rb.freezeRotation = false; //resume normal rotation physics
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Fuel":
                print("Added Fuel");
                break;
            case "Finish":
                print("Congratulations!");
                break;
            default:
                print("Dead");
                break;
        }
    }
}

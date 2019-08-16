using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;

    AudioSource audioSource;

    [SerializeField] float mainThrust = 10f;

    [SerializeField] float rsThrust = 100f;

    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip victorySound;

    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem victoryParticles;

    [SerializeField] float levelLoadDelay = 2f;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive) {
            ThrustInput();
            Rotate();
        }
    }

    private void ThrustInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            engineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineSound);
        }
        engineParticles.Play();
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
        if (state != State.Alive) { return; } //Ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //nothing happens
                break;
            case "Fuel": //todo Add Fuel
                print("Added Fuel");
                break;
            case "Finish":
                SuccessSequence();
                break;
            default:
                DeathSequence();
                break;
        }
    }

    private void SuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(victorySound);
        engineParticles.Stop();
        victoryParticles.Play();
        state = State.Transcending;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void DeathSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        engineParticles.Stop();
        explosionParticles.Play();
        state = State.Dying;
        Invoke("LoadFirstScene", levelLoadDelay);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

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

    bool isTransitioning = false;

    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning) {
            ThrustInput();
            RotationInput();
            if (Debug.isDebugBuild)
            {
                DebugInput();
            }            
        }
    }

    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
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

    private void RotationInput()
    {
        
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(Vector3.back);
        }

    }

    private void Rotate(Vector3 rotationVector)
    {
        rb.freezeRotation = true; //nullify environmental impact on rotation
        transform.Rotate(rotationVector * rsThrust * Time.deltaTime);
        rb.freezeRotation = false; //resume normal rotation physics
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled) { return; } //Ignore collisions when dead

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
        isTransitioning = true;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void DeathSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        engineParticles.Stop();
        explosionParticles.Play();
        isTransitioning = true;
        Invoke("ReloadScene", levelLoadDelay);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else
        {
            LoadFirstScene();
        }
    }
}

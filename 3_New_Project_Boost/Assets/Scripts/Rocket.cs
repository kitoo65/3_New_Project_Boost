using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 150f; //Reaction Control System 
    [SerializeField] float mainThrust = 1500f; //Thrusting

    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip deathSFX;

    [SerializeField] ParticleSystem mainEnginePS;
    [SerializeField] ParticleSystem successPS;
    [SerializeField] ParticleSystem deathPS;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    enum State {Alive, Transcending, Dying };
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
     
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
        
    }
          
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive){ return; } //ignore collisions when dead
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();


                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        successPS.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        Invoke("LoadNextScene", 2f); //#todo parameterise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        deathPS.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX);
        Invoke("LoadFirstLevel", 2f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); //#TODO allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }    

    private void RespondToThrustInput()
    {
        
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            ApplyThrust();

        }
        else
        {
            audioSource.Stop();
            mainEnginePS.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust*Time.fixedDeltaTime);
        if (!audioSource.isPlaying) //Para que no se superponga 
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }
        mainEnginePS.Play();
    }

    void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //Taking manual control of rotation
       
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
           
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //Resume physics control of rotation
    }
    
   
}


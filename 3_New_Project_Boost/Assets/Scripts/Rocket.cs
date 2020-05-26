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

    [SerializeField] float levelLoadDelay = 2f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    enum State {Alive, Transcending, Dying};
    State state = State.Alive;

    bool collisionsDisabled = false;

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
        if (Debug.isDebugBuild)
        {
            RespondToDebugLevelKeys();
        }
        

    }

    void RespondToDebugLevelKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
           
        }
    }
          
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled ){ return; }
      
        
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
        Invoke("LoadNextScene", levelLoadDelay); 
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        deathPS.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX);
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        int cantidadEscenas = SceneManager.sceneCountInBuildSettings;
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneBuildIndex + 1;
        if (nextSceneIndex == cantidadEscenas)
        {
            nextSceneIndex = 0; //loop back to start
        }

        SceneManager.LoadScene(nextSceneIndex);
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


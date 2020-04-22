using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 150f; //Reaction Control System 
    [SerializeField] float mainThrust = 1500f; //Thrusting

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
        Thrust();
    } 
 
   
    

    void OnCollisionEnter(Collision collision)
    {
        print("Collided");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly Collision"); //todo remove this line
                break;
            case "Fuel":
                print("Fuel Collision"); //todo remove this line
                break;
            default:
                print("Death Collision"); //todo remove this line
                break;
        }
    }
    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            print("Thrusting");
            if (!audioSource.isPlaying) //Para que no se superponga 
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
    void Rotate()
    {
        rigidBody.freezeRotation = true; //Taking manual control of rotation
       
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
            print("Rotating Left");

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            print("Rotating Right");
        }
        rigidBody.freezeRotation = false; //Resume physics control of rotation
    }
    
   
}

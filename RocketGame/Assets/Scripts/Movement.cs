﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Variables
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem downBooster;
    [SerializeField] ParticleSystem upBooster;

    [SerializeField] ParticleSystem mainBooster;

    //References
    AudioSource rocketBoost;
    Rigidbody rb;

    // States

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the audio source from the Gameobject
        rocketBoost = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }
    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        // if main booster particles are not playing then play them.
        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }

        //Debug.Log("Space is pressed");
        if (!rocketBoost.isPlaying)
        {
            //AudioSource.Play can only be used when there is only 1 audio. This func cant take parameters.
            //rocketBoost.Play();
            rocketBoost.PlayOneShot(mainEngine);
        }
    }

    private void StopThrusting()
    {
        if (rocketBoost.isPlaying)
        {
            rocketBoost.Stop();
        }
        if (mainBooster.isPlaying)
        {
            mainBooster.Stop();
        }
    }


    void ProcessRotation(){

        if(Input.GetKey(KeyCode.A))
        {
            RotateUp();

        }
        // else cond used because users shouldn't press a and d at the same time. Cant go right and left at the sime time right :)
        else if(Input.GetKey(KeyCode.D))
        {
            RotateDown();
        }
        else
        {
            StopRotation();
        }


    }

    private void StopRotation()
    {
        upBooster.Stop();
        downBooster.Stop();
    }

    private void RotateDown()
    {
        //Debug.Log("Rotating down");
        Rotate(-rotationThrust); // 0,0,-1 -z axis
        if (!downBooster.isPlaying)
        {
            downBooster.Play();
        }
    }

    private void RotateUp()
    {
        //Debug.Log("Rotating Left");
        Rotate(rotationThrust); // 0,0,1 -> change z axis.
        if (!upBooster.isPlaying)
        {
            upBooster.Play();
        }
    }



    // ctrl + . over copied line of code to make a new method.
    // method for rotating the rocket.
    private void Rotate(float rotationDirection)
    {
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate. Not to affect from collision.
        transform.Rotate(Vector3.forward * rotationDirection * Time.deltaTime);
        rb.freezeRotation = false; // we manually rotated, now physics system can take over.
    }

}

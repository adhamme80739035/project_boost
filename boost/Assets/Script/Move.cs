using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineThruster;
    [SerializeField] ParticleSystem leftEngineThruster;
    [SerializeField] ParticleSystem rightEngineThruster;

    Rigidbody rb;
    AudioSource audio;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrust();
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.D))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void StartThrust()
    {
        //Debug.Log("Pressed space - thrusting");
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audio.isPlaying)
            {
                audio.PlayOneShot(mainEngine);
            }
            mainEngineThruster.Play();
    }


    void StopThrusting()
    {
        audio.Stop();
        mainEngineThruster.Stop();
    }




    void RotateRight()
    {
        ApplyRotation(rotationThrust);
            if (!rightEngineThruster.isPlaying)
            {
                rightEngineThruster.Play();
            }
    }

    void RotateLeft()
    {
        ApplyRotation(-rotationThrust);
            if (!leftEngineThruster.isPlaying)
            {
                leftEngineThruster.Play();
            }
    }

    void StopRotating()
    {
        rightEngineThruster.Stop();
        leftEngineThruster.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing rotation so the physics system can take over
    }
}

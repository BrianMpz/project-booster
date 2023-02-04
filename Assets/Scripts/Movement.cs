using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
   // PARAMETERS - for tuning, typically set in the editor
   // CACHE - e.g. references for readability or speed
   // STATE - private instance (member) variables
   [SerializeField] float mainThrust = 100f;
   [SerializeField] float rotationThrust = 1f;
   [SerializeField] AudioClip mainEngine;
   [SerializeField] float CheatlevelLoadDelay = 2f;
   [SerializeField] AudioClip success;
   [SerializeField] ParticleSystem CheatsuccessParticles;

   [SerializeField] ParticleSystem mainEngineParticles;
   [SerializeField] ParticleSystem leftThrusterParticles;
   [SerializeField] ParticleSystem rightThrusterParticles;

   Rigidbody rb;
   AudioSource audioSource;

   // Start is called before the first frame update
   void Start()
   {
      rb = GetComponent<Rigidbody>();
      audioSource = GetComponent<AudioSource>();
   }

   // Update is called once per frame
   void Update()
   {
      ProcessThrust();
      ProcessRotation();
      CheckingforL();
   }

   void ProcessThrust()
   {
      if (Input.GetKey(KeyCode.Space))
      {
         StartThrusting();
      }
      else
      {
         StopThrusting();
      }
   }

   void ProcessRotation()
   {
      if (Input.GetKey(KeyCode.A))
      {
         RotateLeft();
      }
      else if (Input.GetKey(KeyCode.D))
      {
         RotateRight();
      }
      else
      {
         StopRotating();
      }
   }

   void StartThrusting()
   {
      rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
      if (!audioSource.isPlaying)
      {
         audioSource.PlayOneShot(mainEngine);
      }
      if (!mainEngineParticles.isPlaying)
      {
         mainEngineParticles.Play();
      }
   }

   void StopThrusting()
   {
      audioSource.Stop();
      mainEngineParticles.Stop();
   }

   void RotateLeft()
   {
      ApplyRotation(-rotationThrust);
      if (!rightThrusterParticles.isPlaying)
      {
         leftThrusterParticles.Play();
      }
   }

   void RotateRight()
   {
      ApplyRotation(rotationThrust);
      if (!leftThrusterParticles.isPlaying)
      {
         rightThrusterParticles.Play();
      }
   }

   void StopRotating()
   {
      rightThrusterParticles.Stop();
      leftThrusterParticles.Stop();
   }

   void ApplyRotation(float rotationThisFrame)
   {
      rb.freezeRotation = true;  // freezing rotation so we can manually rotate
      transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
      rb.freezeRotation = false;  // unfreezing rotation so the physics system can take over
   }
   void CheckingforL()
   {
      if (Input.GetKey(KeyCode.L))
      {
         audioSource.Stop();
         audioSource.PlayOneShot(success);
         CheatsuccessParticles.Play();
         GetComponent<Movement>().enabled = false;
         Invoke("CheatLoadNextLevel", CheatlevelLoadDelay);
      }
   }
   void CheatLoadNextLevel()
   {
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
   }
}
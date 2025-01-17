using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("Delay Variables")]

    [SerializeField] float waitTimeAfterCrash = 3f;
    [SerializeField] float waitTimeAfterLevelFinish = 3f;

    [Space(20)]

    [Header("Sound Effect Clips")]

    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip finishLevelClip;

    [Space(20)]

    [Header("Particle Effects")]

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem explosionParticles;

    // Cached Variables

    AudioSource audioSource;
    BoxCollider boxCollider;

    // State variables

    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        { 
            LoadNextLevel();
        }

        else if (Keyboard.current.rKey.wasPressedThisFrame)
        { 
            ReloadLevel();
        }

        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
            Debug.Log("Current collision status: " + isCollidable);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isControllable && isCollidable)
        {
            switch (collision.gameObject.tag)
            {
                case "Fuel":
                    FuelCollected();
                    break;

                case "Friendly":
                    FriendlyAreaTouched();
                    break;

                case "Finish":
                    FinishAreaTouched();
                    break;

                default:
                    RocketCrashed();
                    break;
            }
        }

        else
        {
            return;
        }

        
    }

    void FinishAreaTouched()
    {
        Debug.Log("Level Complete! You Survived!");

        

        audioSource.Stop();

        GetComponent<Movement>().enabled = false;
        isControllable = false;

        audioSource.PlayOneShot(finishLevelClip);

        successParticles.Play();

        Invoke("LoadNextLevel", waitTimeAfterLevelFinish);

        
    }

    void FuelCollected()
    {
        Debug.Log("You have collected fuel!");
    }

    void FriendlyAreaTouched()
    {
        Debug.Log("This is safe to touch, you are still flying!");
    }

    void RocketCrashed()
    {
        
        Debug.Log("Well... mistakes were made!");

        
        audioSource.Stop();

        GetComponent<Movement>().enabled = false;
        isControllable = false;

        audioSource.PlayOneShot(explosionClip);

        explosionParticles.Play();

        Invoke("ReloadLevel", waitTimeAfterCrash);

        
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        int maxNumberOfScene = SceneManager.sceneCountInBuildSettings;

        if (nextScene < maxNumberOfScene)
        {
            SceneManager.LoadScene(nextScene);
        }

        else
        {
            SceneManager.LoadScene(0);
        }
        
    }

    

}

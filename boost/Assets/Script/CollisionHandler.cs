using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    ParticleSystem particles;
    AudioSource audio;

    bool isTransitioning = false;
    bool collisionDisable = false;

    void Start() {
        audio = GetComponent<AudioSource>();   
    }

    void Update() {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; // toggle collision
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisionDisable)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                //Debug.Log("Congratulations finished the level");
                break;
            default:
                StartCrashSequence();
                //Debug.Log("sorry, you blew up");
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audio.Stop();
        audio.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Move>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audio.Stop();
        audio.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Move>().enabled = false;
        Invoke("ReloadLevel",levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
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

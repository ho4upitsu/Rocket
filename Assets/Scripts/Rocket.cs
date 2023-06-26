using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float loadLevelDelay = 1f;
    
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip death;
    
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem deathParticles;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    enum State
    {
        Alive,
        Dying,
        Transcending
    }

    private State _state = State.Alive;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (_state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void RespondToRotateInput()
    {
        _rigidbody.freezeRotation = true;
        float rotationSpeed = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        _rigidbody.freezeRotation = false;
    }
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            _audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rigidbody.AddRelativeForce(Vector3.up * (mainThrust));
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly" :
                break;
            case "Finish" :
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        _state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", loadLevelDelay);
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", loadLevelDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
}

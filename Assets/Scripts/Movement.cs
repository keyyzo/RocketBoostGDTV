using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Input Action Variables")]

    [SerializeField] InputAction thrustAction;
    [SerializeField] InputAction rotateAction;

    [Space(20)]

    [Header("Movement Variables")]

    [SerializeField] float baseThrustForce = 10f;

    [Space(20)]

    [Header("Rotation Variables")]

    [SerializeField] float baseRotationSpeed = 1.0f;

    [Space(20)]

    [Header("Sound Effect Clips")]

    [SerializeField] AudioClip mainEngine;

    [Space(20)]

    [Header("Particle Effects")]

    [SerializeField] ParticleSystem mainThrustParticles;
    [SerializeField] ParticleSystem leftSideThrustParticles;
    [SerializeField] ParticleSystem rightSideThrustParticles;


    // Cached variables

    Rigidbody rb;
    AudioSource audioSource;

    private void OnEnable()
    {
        thrustAction.Enable();
        rotateAction.Enable();
    }

    private void OnDisable()
    {
        thrustAction.Disable();
        rotateAction.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ApplyThrust();
        ProcessRotation();
    }

    //----- PRIVATE METHODS BELOW -----//

    void ApplyThrust()
    {
        if (thrustAction.IsPressed())
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
        rb.AddRelativeForce(Vector3.up * baseThrustForce * Time.fixedDeltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }


    private void StopThrusting()
    {
        audioSource.Stop();
        mainThrustParticles.Stop();
    }


    void ProcessRotation()
    {
        float rotationValue = rotateAction.ReadValue<float>();

        if (rotationValue < 0f)
        {
            RotateRight();
        }

        else if (rotationValue > 0f)
        {
            RotateLeft();
        }

        else
        {
            StopRotating();
        }

    }

    void ApplyRotation(float rotSpeedVal)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotSpeedVal * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

    private void RotateLeft()
    {
        ApplyRotation(-baseRotationSpeed);

        if (!leftSideThrustParticles.isPlaying)
        {
            leftSideThrustParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(baseRotationSpeed);

        if (!rightSideThrustParticles.isPlaying)
        {
            rightSideThrustParticles.Play();
        }
    }

    private void StopRotating()
    {
        rightSideThrustParticles.Stop();
        leftSideThrustParticles.Stop();
    }

    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public float stepRate = 0.5f;
    private float stepTimer;
    private CharacterController controller;

    void Start()
    {
        Debug.Log("PlayerFootsteps STARTED");

        controller = GetComponent<CharacterController>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (footstepClips.Length > 0)
            audioSource.PlayOneShot(footstepClips[0]);
    }

    void Update()
    {
        Debug.Log("PlayerFootsteps UPDATE");

        Debug.DrawRay(transform.position, Vector3.down * 1.2f, Color.red);

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is null!");
        }
        if (footstepClips.Length == 0)
        {
            Debug.LogWarning("No footstep clips assigned!");
        }
        Debug.Log($"Velocity: {controller.velocity.magnitude}, Grounded: {controller.isGrounded}");
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Trying to play footstep sound...");
            if (footstepClips.Length > 0 && audioSource != null)
            {
                audioSource.PlayOneShot(footstepClips[0]);
            }
        }
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f && footstepClips.Length > 0 && audioSource != null)

        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                Debug.Log("Playing footstep sound");
                PlayFootstep();
                stepTimer = stepRate;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        int index = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[index]);
    }
}

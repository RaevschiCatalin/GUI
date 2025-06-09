
using UnityEngine;
using System.Collections;


public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public float stepRate = 0.5f;
    private float stepTimer;
    private CharacterController controller;
    private Coroutine footstepCoroutine;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        bool isMoving = controller.velocity.magnitude != 0;
        
        if (isMoving && footstepCoroutine == null)
        {
            footstepCoroutine = StartCoroutine(PlayFootsteps());
        }
        else if (!isMoving && footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
    }

    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (footstepClips.Length == 0 || audioSource == null)
                yield break;

            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);

            // Wait for sound to finish before next step
            yield return new WaitForSeconds(clip.length);
        }
    }
}

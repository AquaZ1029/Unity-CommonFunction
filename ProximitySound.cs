using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProximitySound : MonoBehaviour
{
    public Transform targetObject;
    public AudioClip soundClip;
    public float detectRange = 5f;
    public float cooldownTime = 3f;

    private AudioSource audioSource;
    private float nextPlayTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextPlayTime && targetObject != null && soundClip != null)
        {
            if (IsInRange())
            {
                audioSource.PlayOneShot(soundClip);
                nextPlayTime = Time.time + cooldownTime;
            }
        }
    }

    private bool IsInRange()
    {
        return (transform.position - targetObject.position).sqrMagnitude <= detectRange * detectRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = (targetObject != null && IsInRange()) ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (targetObject != null)
        {
            Gizmos.DrawLine(transform.position, targetObject.position);
        }
    }
}
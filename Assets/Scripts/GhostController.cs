
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent (typeof(Rigidbody))]
public class GhostController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public AudioClip scareA;
    public AudioClip scareB;
    public AudioClip scareC;
    public AudioSource audioSource;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private static readonly int HauntStart = Animator.StringToHash("HauntStart");
    private static readonly int HauntEnd = Animator.StringToHash("HauntEnd");
    private Hauntable Haunted { get; set; }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        var distance = movementSpeed * Time.deltaTime;
        var movement = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
                movement = transform.up * distance;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement = -transform.up * distance;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement = -transform.right * distance;
            _spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = transform.right * distance;
            _spriteRenderer.flipX = true;
        }

        transform.position += movement;

        if (Input.GetKey(KeyCode.E))
        {
            Scare();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.tag.Equals("Hauntable")) return;
        var hauntable = other.gameObject.GetComponent<Hauntable>();
        HauntObject(hauntable);
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.tag.Equals("Hauntable")) return;
        UnHauntObject();
    }

    private void HauntObject(Hauntable toHaunt)
    {
        Haunted = toHaunt;
        Haunted.Haunt();
        _animator.SetTrigger(HauntStart);
    }

    private void UnHauntObject()
    {
        Haunted.UnHaunt();
        _animator.SetTrigger(HauntEnd);
        Haunted = null;
    }

    private void Scare()
    {
        if (audioSource.isPlaying) return;
        audioSource.clip = Random.Range(0, 2) switch
        {
            0 => scareA,
            1 => scareB,
            _ => scareC
        };
        audioSource.Play();
    }
}

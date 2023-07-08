using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
public class Hauntable : MonoBehaviour
{
    public static readonly HashSet<Hauntable> Furniture = new HashSet<Hauntable>();

    private SpriteRenderer _spriteRenderer;

    // public AudioClip scareSound;
    public Sprite normalSprite;
    public Sprite hauntedSprite;
    
    private static readonly int IsHaunted = Animator.StringToHash("IsHaunted");
    private static readonly int Scare = Animator.StringToHash("Scare");

    private void Start()
    {
        Furniture.Add(this);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        
    }
    
    public void Haunt()
    {
        _spriteRenderer.sprite = hauntedSprite;
    }
    
    public void UnHaunt()
    {
        _spriteRenderer.sprite = normalSprite;
    }

    public void ObjectScare()
    {
        
    }
}

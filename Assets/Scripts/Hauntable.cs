using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hauntable : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public Material hauntMaterial;
    public Material unHauntMaterial;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        
    }
    
    public void Haunt()
    {
        _spriteRenderer.material = hauntMaterial;
    }
    
    public void UnHaunt()
    {
        _spriteRenderer.material = unHauntMaterial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class DoorController : MonoBehaviour
{
    public Sprite doorOpen;
    private Sprite _doorClosed;

    private SpriteRenderer _spriteRenderer;
    private bool _doorOpen;
    
    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _doorClosed = _spriteRenderer.sprite;
    }

    public void Toggle()
    {
        _doorOpen = !_doorOpen;
        _spriteRenderer.sprite = _doorOpen ? doorOpen : _doorClosed;
    }

    public void Open()
    {
        _doorOpen = true;
        _spriteRenderer.sprite = doorOpen;
    }
    
    public void Close()
    {
        _doorOpen = false;
        _spriteRenderer.sprite = _doorClosed;
    }
    
    // Update is called once per frame
    private void Update()
    {
        
    }
}

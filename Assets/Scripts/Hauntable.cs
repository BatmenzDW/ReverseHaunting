using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
public class Hauntable : MonoBehaviour
{
    public static readonly HashSet<Hauntable> Furniture = new HashSet<Hauntable>();

    [SerializeField] private FurnitureSprites furnitureSprites;
    
    private SpriteRenderer _spriteRenderer;

    private Sprite _normalSprite;
    private Sprite _hauntedSprite;
    private int _spriteIndex;
    
    private static readonly int IsHaunted = Animator.StringToHash("IsHaunted");
    private static readonly int Scare = Animator.StringToHash("Scare");

    private void Start()
    {
        Furniture.Add(this);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        var spriteName = _spriteRenderer.sprite.name.Split('_').Last();
        _spriteIndex = int.Parse(spriteName);

        _normalSprite = furnitureSprites.GetNormalSprite(_spriteIndex);
        _hauntedSprite = furnitureSprites.GetHauntedSprite(_spriteIndex);
        _spriteRenderer.sprite = _normalSprite;
    }
    
    private void Update()
    {
        if (39 <= _spriteIndex && _spriteIndex <= 41)
        {
            if (Random.Range(0f, 1f) >= 0.9999f)
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }
    
    public void Haunt()
    {
        _spriteRenderer.sprite = _hauntedSprite;
    }
    
    public void UnHaunt()
    {
        _spriteRenderer.sprite = _normalSprite;
    }

    public void ObjectScare()
    {

    }
}

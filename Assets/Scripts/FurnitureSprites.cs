using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "FurnitureSprites")]
public class FurnitureSprites : ScriptableObject
{
    public List<Sprite> normalSprites;
    public List<Sprite> hauntedSprites;

    public Sprite GetNormalSprite(int index) => normalSprites[index];
    public Sprite GetHauntedSprite(int index) => hauntedSprites[index];
}

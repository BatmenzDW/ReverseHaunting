using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "FurnitureSprites")]
public class FurnitureSprites : ScriptableObject
{
    public List<Sprite> normalSprites;
    public List<Sprite> hauntedSprites;

    public Sprite GetNormalSprite(int index) => normalSprites.FirstOrDefault(h => h.name == $"furniture_{index}");
    public Sprite GetHauntedSprite(int index) => hauntedSprites.FirstOrDefault(h => h.name == $"furniture_haunted_{index}");
}

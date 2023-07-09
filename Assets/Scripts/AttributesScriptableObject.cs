using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "AttributeObject")]
public class AttributesScriptableObject : ScriptableObject
{
    public Sprite Icon;

    [Header("Character Attributes")]
    public string Name;
    public int Health;
    [Min(0), Tooltip("Speed in which something moves.")]
    public float Speed;
    [Min(1), Tooltip("Player use stamina to do actions.")]
    public int Stamina;
    [Min(0), Tooltip("NPCs scare metter when full they leave.")]
    public int ScareMeter;
    [Min(0), Tooltip("Range to posse objects.")]
    public float PossesionRange;
    [Min(0), Tooltip("Heavier items slow the player down.")]
    public int Weight;

    [Header("Details")]
    [Multiline(5)]
    public string Description;

    [Header("Designer Only (not shown to players)")]
    public string Notes;

}

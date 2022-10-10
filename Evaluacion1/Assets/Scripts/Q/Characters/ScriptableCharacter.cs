using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesCharacters { Chasing, Attacking }
public enum CharactersFactions { Hero, Soldier }

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data")]
public class ScriptableCharacter : ScriptableObject
{


    public uint ActionPoints;
    public uint AttackPoints;
    public uint MovePoints;
    public uint HealPoints;
    public uint DefensePoints;
}

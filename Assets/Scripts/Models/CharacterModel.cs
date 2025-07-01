using System;
using UnityEngine;

[Serializable]
public class CharacterModel : ICharacterModel
{
    [field: SerializeField] public float Acceleration { get; set; } = 5;
    [field: SerializeField] public float Speed { get; set; } = 2;
    [field: SerializeField] public float JumpForce { get; set; } = 5;
}

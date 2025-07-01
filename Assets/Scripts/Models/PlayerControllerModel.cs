using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerControllerModel : IPlayerControllerModel
{
    [field: SerializeField] public InputActionReference MoveInput { get; set; }
    [field: SerializeField] public InputActionReference JumpInput { get; set; }
    [field: SerializeField] public float AirborneSpeedMultiplier { get; set; } = .5f;
}

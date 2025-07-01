using UnityEngine.InputSystem;

public interface IPlayerControllerModel
{
    InputActionReference MoveInput { get; }
    InputActionReference JumpInput { get; }
    float AirborneSpeedMultiplier { get; }
}

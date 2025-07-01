
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour, ISetup<IPlayerControllerModel>
{
    public IPlayerControllerModel Model { get; set; }

    private ICharacter _character;
    private StateMachine _stateMachine;
    private IJumpAbility _jumpAbility;

    private void Awake()
    {
        _character = (ICharacter)GetComponent<Character>();
    }

    public void Setup(IPlayerControllerModel model)
    {
        Model = model;

        
        _stateMachine = new StateMachine();
        _stateMachine.Initialize(
            new GroundState(_stateMachine, Model.AirborneSpeedMultiplier),
            _character
        );

        
        _jumpAbility = new DoubleJumpAbility(_stateMachine, _character);

        if (Model.MoveInput?.action != null)
        {
            Model.MoveInput.action.started += HandleMoveInput;
            Model.MoveInput.action.performed += HandleMoveInput;
            Model.MoveInput.action.canceled += HandleMoveInput;
        }
        if (Model.JumpInput?.action != null)
            Model.JumpInput.action.performed += HandleJumpInput;
    }

    private void OnDisable()
    {
        if (Model.MoveInput?.action != null)
        {
            Model.MoveInput.action.started -= HandleMoveInput;
            Model.MoveInput.action.performed -= HandleMoveInput;
            Model.MoveInput.action.canceled -= HandleMoveInput;
        }
        if (Model.JumpInput?.action != null)
            Model.JumpInput.action.performed -= HandleJumpInput;
    }

    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        _stateMachine.Move(dir);
    }

    private void HandleJumpInput(InputAction.CallbackContext ctx)
    {
        _jumpAbility.TryJump();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        foreach (var contact in other.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.up) < 5f)
            {
                _jumpAbility.OnLand();
                _stateMachine.Land();
            }
        }
    }
}

public interface IJumpAbility
{
   
    bool TryJump();
    
    void OnLand();
}

public class DoubleJumpAbility : IJumpAbility
{
    private readonly StateMachine _sm;
    private readonly ICharacter _character;
    private int _jumpsDone;

    public DoubleJumpAbility(StateMachine sm, ICharacter character)
    {
        _sm = sm;
        _character = character;
        _jumpsDone = 0;
    }

    public bool TryJump()
    {
        if (_jumpsDone >= 2)
            return false;

        if (_jumpsDone == 0)
        {
            
            _sm.Jump();
        }
        else
        {
            
            if (_character is MonoBehaviour mb)
                mb.StartCoroutine(_character.Jump());
        }

        _jumpsDone++;
        return true;
    }

    public void OnLand()
    {
        _jumpsDone = 0;
    }
}


public interface IPlayerState
{
    void Enter(ICharacter character);
    void HandleMove(ICharacter character, Vector2 direction);
    void HandleJump(ICharacter character);
    void HandleLand(ICharacter character);
    void Exit(ICharacter character);
}

public class StateMachine
{
    private IPlayerState _current;
    private ICharacter _character;

    public void Initialize(IPlayerState startState, ICharacter character)
    {
        _character = character;
        _current = startState;
        _current.Enter(_character);
    }

    public void ChangeState(IPlayerState nextState)
    {
        _current.Exit(_character);
        _current = nextState;
        _current.Enter(_character);
    }

    public void Move(Vector2 direction) => _current.HandleMove(_character, direction);
    public void Jump() => _current.HandleJump(_character);
    public void Land() => _current.HandleLand(_character);
}

public class GroundState : IPlayerState
{
    private readonly StateMachine _sm;
    private readonly float _airborneSpeed;

    public GroundState(StateMachine sm, float airborneSpeed)
    {
        _sm = sm;
        _airborneSpeed = airborneSpeed;
    }

    public void Enter(ICharacter c) { }
    public void HandleMove(ICharacter c, Vector2 dir) => c.SetDirection(dir.x);

    public void HandleJump(ICharacter c)
    {
        if (c is MonoBehaviour mb)
            mb.StartCoroutine(c.Jump());
        _sm.ChangeState(new JumpState(_sm, _airborneSpeed));
    }

    public void HandleLand(ICharacter c) { }
    public void Exit(ICharacter c) { }
}

public class JumpState : IPlayerState
{
    private readonly StateMachine _sm;
    private readonly float _airborneSpeed;

    public JumpState(StateMachine sm, float airborneSpeed)
    {
        _sm = sm;
        _airborneSpeed = airborneSpeed;
    }

    public void Enter(ICharacter c) { }
    public void HandleMove(ICharacter c, Vector2 dir) => c.SetDirection(dir.x * _airborneSpeed);
    public void HandleJump(ICharacter c) { }

    public void HandleLand(ICharacter c)
    {
        _sm.ChangeState(new GroundState(_sm, _airborneSpeed));
    }

    public void Exit(ICharacter c) { }
}

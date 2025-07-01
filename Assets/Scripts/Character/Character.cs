using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour, ISetup<CharacterModel>, ICharacter
{
    private float _direction = 0;
    private Rigidbody2D _rigidbody;
    [field: SerializeField] public CharacterModel Model { get; set; } = new();

    public Vector2 Velocity => _rigidbody?.linearVelocity ?? Vector2.zero;

    private void Awake()
        => _rigidbody = GetComponent<Rigidbody2D>();

    public void Setup(CharacterModel model)
        => Model = model;

    private void FixedUpdate()
    {
        var scaledDirection = Vector2.right * (_direction * Model.Acceleration);
        if (Mathf.Abs(_rigidbody.linearVelocity.x) < Model.Speed)
            _rigidbody.AddForce(scaledDirection, ForceMode2D.Force);
    }

 
    public void SetDirection(float direction)
        => _direction = direction;

   
    public IEnumerator Jump()
    {
        yield return new WaitForFixedUpdate();
        _rigidbody.AddForce(Vector3.up * Model.JumpForce, ForceMode2D.Impulse);
    }

  
}

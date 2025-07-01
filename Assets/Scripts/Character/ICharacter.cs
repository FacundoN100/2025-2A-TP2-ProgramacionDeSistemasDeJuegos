using System.Collections;
using UnityEngine;


public interface ICharacter
{
  
    IEnumerator Jump();

   
    void SetDirection(float x);

    
    Transform transform { get; }
}

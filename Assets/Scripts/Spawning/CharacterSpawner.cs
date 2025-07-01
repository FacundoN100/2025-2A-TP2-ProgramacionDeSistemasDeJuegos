using UnityEngine;

[RequireComponent(typeof(Transform))]
public class CharacterSpawner : MonoBehaviour
{
    
    public static CharacterSpawner Instance { get; private set; }

    [Header("Prefab del Character")]
    [SerializeField] private Character prefab;

    [Header("Model de Character y Controller")]
    [SerializeField] private CharacterModel characterModel;
    [SerializeField] private PlayerControllerModel controllerModel;

    [Header("Animator Override (Opcional)")]
    [SerializeField] private RuntimeAnimatorController animatorController;

    private void Awake()
    {
       
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        
        GameServiceRegistry.Add<CharacterSpawner>(this);
    }

   
    public void Spawn()
    {
        if (prefab == null)
        {
            Debug.LogError(" Prefab (Character) no asignado en CharacterSpawner.");
            return;
        }

      
        var result = Instantiate(prefab, transform.position, transform.rotation);

     
        result.Setup(characterModel);

       
        if (!result.TryGetComponent<PlayerController>(out var controller))
            controller = result.gameObject.AddComponent<PlayerController>();
        controller.Setup(controllerModel);

       
        if (animatorController != null)
        {
            var anim = result.GetComponentInChildren<Animator>();
            if (anim != null)
                anim.runtimeAnimatorController = animatorController;
        }
    }
}

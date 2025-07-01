using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpawnButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        
        var spawner = CharacterSpawner.Instance;
        if (spawner != null)
            spawner.Spawn();
        else
            Debug.LogError(" No existe CharacterSpawner.Instance en la escena.");
    }
}

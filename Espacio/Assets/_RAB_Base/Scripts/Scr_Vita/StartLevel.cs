using UnityEngine;
using UnityEngine.InputSystem;

public class LevelStartFix : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;   // снимаем паузу, если прошлый уровень её оставил

        PlayerInput pi = FindAnyObjectByType<PlayerInput>();
        if (pi != null)
        {
            pi.ActivateInput();                     // включаем управление
            pi.SwitchCurrentActionMap("Gameplay");  // нужная карта кнопок
        }
    }
}
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 100f;

    public CanvasGroup parentGroup;
    void FixedUpdate() // вращение картинки (анимация загрузки)
    {
        if (parentGroup.interactable) // анимация будет остановленна когда пользователь не видит экран загрузки
        {
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }
}
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 100f;

    public CanvasGroup parentGroup;
    void FixedUpdate() // вращение картинки (анимация загрузки)
    {
        if (parentGroup.interactable)
        {
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }
}
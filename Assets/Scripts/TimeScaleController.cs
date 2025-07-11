using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float timeScale = 1f; // Значение от 0 (стоп) до 1 (нормальная скорость)

    void Update()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Чтобы физика правильно работала при замедлении
    }
}
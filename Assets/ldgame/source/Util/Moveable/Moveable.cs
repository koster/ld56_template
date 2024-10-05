using System;
using UnityEngine;

public class MoveableBalatro : MoveableBase
{
    private Vector2 velocity; // Текущая скорость
    private float maxVelocity; // Максимальная скорость

    private void Update()
    {
        // Предполагая, что realDt является дельтой времени между кадрами
        float realDt = Mathf.Clamp(Time.smoothDeltaTime, 1 / 50f, 1 / 100f);
        
        // Вычисляем затухание и максимальную скорость
        float expTimeXY = Mathf.Exp(-50 * realDt);
        maxVelocity = 70 * realDt;

        MoveXY(realDt, expTimeXY);
    }

    private void MoveXY(float dt, float expTimeXY)
    {
        Vector2 T = targetPosition; // Целевая позиция
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y); // Текущая позиция
        
        // Применяем экспоненциальное затухание к скорости
        velocity = expTimeXY * velocity + (1 - expTimeXY) * (T - currentPos) * 35 * dt;
        
        // Ограничиваем скорость
        if (velocity.sqrMagnitude > maxVelocity * maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        // Обновляем позицию
        transform.position += new Vector3(velocity.x, velocity.y, 0) * dt * 100f;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCam : MonoBehaviour
{
    public Transform _target;
    public float smoothTime = 0.2f;

    private Vector3 _velocity = Vector3.zero;




    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(//сохраняем координаты Z, меняя  значения X и Y
            _target.position.x,
            _target.position.y,
            transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);//плавный переход от текущей к целевой позиции

    }
}

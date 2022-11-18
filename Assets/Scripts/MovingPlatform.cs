using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  

public class MovingPlatform : MonoBehaviour
{
    public Vector3 finishPos=Vector3.zero;//целевое положение
    public float speed = 0.1f;

    public Vector3 _startPos;
    private float _trackPercent=0;//насколько далеко наше положение  от старта до финиша
    private int _direction = 1;//направление движения

    
    void Start()
    {
        _startPos = transform.position;//положенрие в сцене -точка  от которой начинается движение
    }

   
    void Update()
    {
        _trackPercent+=_direction*speed*Time.deltaTime;
        float x=(finishPos.x-_startPos.x)*_trackPercent+ _startPos.x;
        float y=(finishPos.y-_startPos.y)*_trackPercent+ _startPos.y;
        transform.position = new Vector3(x, y, _startPos.z);

        if ((_direction == 1 && _trackPercent > .9f) || (_direction == -1 && _trackPercent < .1f))
        {
            //меняем направление движения
            _direction *= -1;
        }
    }

    //метод отрисовки вспомогательной линии только во вкладке Scene
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;//задает цвет линии
        Gizmos.DrawLine(transform.position, finishPos);// заставляет Unity нарисовать линию от места, где находится платформа, до целевой точки
    }
}

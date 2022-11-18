using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    public float speed = 400.0f;
    private float jumpForce = 7.0f;

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();//нужно чтобы к объекту GameObject был прикреплен этот второй компонент
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();//получаем этот компонент для проверки поверхности
    }

    
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 move = new Vector2(deltaX, _body.velocity.y);//задаем  только горизонтальное движение и сохраняем вертикальное смещение
        _body.velocity = move;

        Vector3 max= _boxCollider.bounds.max;
        Vector3 min = _boxCollider.bounds.min;

        //проверяем значение минимальной Y-координаты коллайдера
        Vector2 corner1=new Vector2(max.x, max.y-0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2F);
        Collider2D hitCollider2D = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        if (hitCollider2D != null)//если под персонажем обнаружен коллайдер
        {
            grounded = true;
        }

        _body.gravityScale = (grounded && Mathf.Approximately(deltaX,0)) ? 0 : 1;//остановка при нахождении на поверхности и в статичном состоянии

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _body.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);//сила добавляется при нажатии на пробел
        }

        MovingPlatform platform = null;
        if (hitCollider2D != null)
        {
            platform=hitCollider2D.gameObject.GetComponent<MovingPlatform>();//проверка может ли двигаться платвформа над персонажем
        }

        if (platform != null)
        {
            transform.parent=platform.transform;//выполняем связывание с платформой или очищаем переменную transform.parent.
        }
        else
        {
                transform.parent=null;
        }

        _animator.SetFloat("speed",Mathf.Abs(deltaX));//скоротсть больше нуля даже при отрицательных значениях

        Vector3 pScale=Vector3.one;//при нахождении вне движущейся платформы масштаб равен 1
        if (platform != null)
        {
            pScale=platform.transform.localScale;
        }
       

        if (deltaX!=0)//сравниваем значения методом Approximately
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX)/pScale.x, 1/pScale.y, 1);//в процесе движения масштабируем положительную или отрицательную 1 для поворота направо или налево
        }
    }
}

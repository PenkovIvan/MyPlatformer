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
        _body = GetComponent<Rigidbody2D>();//����� ����� � ������� GameObject ��� ���������� ���� ������ ���������
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();//�������� ���� ��������� ��� �������� �����������
    }

    
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 move = new Vector2(deltaX, _body.velocity.y);//������  ������ �������������� �������� � ��������� ������������ ��������
        _body.velocity = move;

        Vector3 max= _boxCollider.bounds.max;
        Vector3 min = _boxCollider.bounds.min;

        //��������� �������� ����������� Y-���������� ����������
        Vector2 corner1=new Vector2(max.x, max.y-0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2F);
        Collider2D hitCollider2D = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        if (hitCollider2D != null)//���� ��� ���������� ��������� ���������
        {
            grounded = true;
        }

        _body.gravityScale = (grounded && Mathf.Approximately(deltaX,0)) ? 0 : 1;//��������� ��� ���������� �� ����������� � � ��������� ���������

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _body.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);//���� ����������� ��� ������� �� ������
        }

        MovingPlatform platform = null;
        if (hitCollider2D != null)
        {
            platform=hitCollider2D.gameObject.GetComponent<MovingPlatform>();//�������� ����� �� ��������� ���������� ��� ����������
        }

        if (platform != null)
        {
            transform.parent=platform.transform;//��������� ���������� � ���������� ��� ������� ���������� transform.parent.
        }
        else
        {
                transform.parent=null;
        }

        _animator.SetFloat("speed",Mathf.Abs(deltaX));//��������� ������ ���� ���� ��� ������������� ���������

        Vector3 pScale=Vector3.one;//��� ���������� ��� ���������� ��������� ������� ����� 1
        if (platform != null)
        {
            pScale=platform.transform.localScale;
        }
       

        if (deltaX!=0)//���������� �������� ������� Approximately
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX)/pScale.x, 1/pScale.y, 1);//� ������� �������� ������������ ������������� ��� ������������� 1 ��� �������� ������� ��� ������
        }
    }
}

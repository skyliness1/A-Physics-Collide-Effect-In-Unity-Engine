using System.Collections.Generic;
using UnityEngine;

//����Shape�ࡣ���ɱ�ʵ������������ʵ�ָ������װ����������
public abstract class Shape:MonoBehaviour 
{
    public Vector2 local_position;

    //��������״��ײ�ĳ��󷽷���������ײ����Ϣ
    public abstract Contact On_Collide_With(Rigidbody self, Rigidbody other, Shape shape);

    //��Բ����ײ�ĳ��󷽷���������ײ����Ϣ
    public abstract Contact On_Collide_With_Circle(Rigidbody self, Rigidbody other, Circle circle,bool isReversed);

    //�������ײ�ĳ��󷽷���������ײ����Ϣ
    public abstract Contact On_Collide_With_Rectangle(Rigidbody self, Rigidbody other, Rectangle rectangle, bool isReversed);
}
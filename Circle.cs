using System;
using UnityEngine;

public class Circle:Shape
{
    public float radius;

    /// <summary>
    /// self��this��ӵ�е�
    /// </summary>
    /// <param name="self"></param>
    /// <param name="other"></param>
    /// <param name="shape"></param>
    /// <returns></returns>
    //��дԲ����������״��ײ�ķ�����������ײ����Ϣ
    public override Contact On_Collide_With(Rigidbody self, Rigidbody other, Shape shape)
    {
        //��ײ���໥�ģ����Է���������״��other����Բ�Σ�self������ײ��Ϣ
        //�öԷ���ײ
        return shape.On_Collide_With_Circle(other, self, this,true);
    }
    //��дԲ����Բ�Σ���С����С��֮�䣩��ײ�ķ�����������ײ��ľ�����Ϣ
    public override Contact On_Collide_With_Circle(Rigidbody self, Rigidbody other, Circle circle, bool isReversed)
    {
        //����λ������״λ��=>ʵ��λ��
        var self_position= self.position+local_position;
        var other_position= other.position+circle.local_position;
        //����֮���λ�ò�
        var delta_position=self_position-other_position;
        //����֮��λ�ò��ƽ��
        var sqr_distance=delta_position.sqrMagnitude;
        //��ȫ����
        var safe_distance=radius+circle.radius;
        var sqr_safe_distance = safe_distance * safe_distance;
        //ʵ�ʾ�����ڰ�ȫ���룬˵��ûײ��
        if(sqr_distance>sqr_safe_distance)
        {
            return null;
        }
        //λ�ò��ƽ���ٿ������õ����߼�ʵ�ʵľ���
        var distance=MathF.Sqrt(sqr_distance);
        //������ײ����������Ϣ���洢��Contact��
        Vector2 normal;
        //float.Epsilon��float����������С���ܣ��������ֹdistance̫С������
        if (distance>float.Epsilon)
        {
           normal=delta_position/distance;
        }
        else
        {
            normal=new Vector2(1,0);
        }
        return new Contact(self,other,this,circle,
            radius,circle.radius,normal,safe_distance-distance,isReversed);
    }
    //��дԲ������Σ���С����ש��֮�䣩��ײ�ķ�����������ײ��ľ�����Ϣ
    public override Contact On_Collide_With_Rectangle(Rigidbody self, Rigidbody other, Rectangle rectangle, bool isReversed)
    {
       return rectangle.On_Collide_With_Circle(other, self, this, !isReversed);
    }


}
using UnityEngine;

public class Contact
{
    public Rigidbody bodyA, bodyB;
    public Shape shapeA, shapeB;
    public float radiusA, radiusB;

    public Vector2 normal;//��ײ�淨��,������Զ����ײ��
    public Vector2 Collide_point;//��ײ��
    public float depth;//��ײ�α�̶ȣ��൱����������ײʱ�ཻ����ȣ�

    public Contact(Rigidbody bodyA, Rigidbody bodyB, Shape shapeA, Shape shapeB,float radiusA,float radiusB, Vector2 normal, float depth,bool isReversed)
    {
        if(isReversed)
        {
            this.bodyA = bodyB;
            this.bodyB = bodyA;
            this.shapeA = shapeB;
            this.shapeB = shapeA;
            this.radiusA = radiusB;
            this.radiusB = radiusA;
            this.normal = -normal;
        }
        else
        {
            this.bodyA = bodyA;
            this.bodyB = bodyB;
            this.shapeA = shapeA;
            this.shapeB = shapeB;
            this.radiusA = radiusA;
            this.radiusB = radiusB;
            this.normal = normal;
        }
        this.depth = depth;
    }
    public bool VelocitySolution()
    {
        //��̬
        if(bodyB.isStatic)
        {
            //�ٶȷ����ϵ�ͶӰ
            var normal_speed = Vector2.Dot(bodyA.velocity,normal);
            //��˴����㣬˵���ٶ��뷨��������ͬ�򣬲�����
            if (normal_speed>=0)
            {
                return false;
            }
            else
            {
                #region �ٶȷֽ�
                //  \------------> tangent
                //  |\
                //  | \
                //  |  \
                //  |   \
                //  |    \
                //  |     \
                //  \/     \
                //  normal  
                #endregion

                //�����ٶ�
                var normal_velocity = normal * normal_speed;
                //�����ٶ�
                var tangent_velocity=bodyA.velocity-normal_velocity;
                //���ٶ�
                var new_velocity = tangent_velocity+ -normal_velocity;

                bodyA.velocity = new_velocity;
            }
        }
        //�Ǿ�̬,������һ��Ϊ��
        else
        {
            var bodyA_normal_speed = Vector2.Dot(bodyA.velocity,normal);
            var bodyB_normal_speed=Vector2.Dot(bodyB.velocity,normal);
            //����
            if(bodyA_normal_speed+bodyB_normal_speed>+0)
            {
                return false;
            }
            //�ӽ�
            else
            {
                var bodyA_normal_velocity = bodyA_normal_speed * normal;
                var bodyB_normal_velocity = bodyB_normal_speed * normal;

                var bodyA_tangent_velocity = bodyA.velocity - bodyA_normal_velocity;
                var bodyB_tangent_velocity = bodyB.velocity - bodyB_normal_velocity;
                //�ٶȽ���
                bodyA.velocity = bodyA_tangent_velocity + bodyB_normal_velocity;
                bodyB.velocity = bodyB_tangent_velocity + bodyA_normal_velocity;


            }
        }
        return true;
    }
    public void PositionSolution()
    {
        //Update depth after collide 
        depth=(radiusA+radiusB)- (Vector2.Dot(bodyA.position,normal)-Vector2.Dot(bodyB.position,normal));
        if(depth<=0)
        {
            return;
        }
        if(bodyA.isStatic)
        {
            bodyA.position += normal * depth * 0.75f;
        }
        else
        {
            //�������ֶ���
            bodyA.position += normal * depth * 0.375f;
            bodyB.position -= normal * depth * 0.375f;
        }
    }
}
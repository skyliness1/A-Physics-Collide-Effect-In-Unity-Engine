using UnityEngine;

public class Contact
{
    public Rigidbody bodyA, bodyB;
    public Shape shapeA, shapeB;
    public float radiusA, radiusB;

    public Vector2 normal;//碰撞面法线,方向永远朝向撞者
    public Vector2 Collide_point;//碰撞点
    public float depth;//碰撞形变程度（相当于两物体碰撞时相交的深度）

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
        //静态
        if(bodyB.isStatic)
        {
            //速度法向上的投影
            var normal_speed = Vector2.Dot(bodyA.velocity,normal);
            //点乘大于零，说明速度与法向量基本同向，不处理
            if (normal_speed>=0)
            {
                return false;
            }
            else
            {
                #region 速度分解
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

                //法线速度
                var normal_velocity = normal * normal_speed;
                //切向速度
                var tangent_velocity=bodyA.velocity-normal_velocity;
                //新速度
                var new_velocity = tangent_velocity+ -normal_velocity;

                bodyA.velocity = new_velocity;
            }
        }
        //非静态,以质量一致为例
        else
        {
            var bodyA_normal_speed = Vector2.Dot(bodyA.velocity,normal);
            var bodyB_normal_speed=Vector2.Dot(bodyB.velocity,normal);
            //分离
            if(bodyA_normal_speed+bodyB_normal_speed>+0)
            {
                return false;
            }
            //接近
            else
            {
                var bodyA_normal_velocity = bodyA_normal_speed * normal;
                var bodyB_normal_velocity = bodyB_normal_speed * normal;

                var bodyA_tangent_velocity = bodyA.velocity - bodyA_normal_velocity;
                var bodyB_tangent_velocity = bodyB.velocity - bodyB_normal_velocity;
                //速度交换
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
            //消除部分抖动
            bodyA.position += normal * depth * 0.375f;
            bodyB.position -= normal * depth * 0.375f;
        }
    }
}
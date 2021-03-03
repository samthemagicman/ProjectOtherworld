using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum AttackDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Attack : MonoBehaviour
    {
        public GameObject attackDebugVisualization;
        public LayerMask ignoreLayers;
        Rigidbody2D rb;
        public GameObject attackHitbox;

        AttackDirection lastDirection = AttackDirection.Right;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Attack"))
            {
                DoAttack();
            }

            Vector3 v = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (v.x > 0)
            {
                lastDirection = AttackDirection.Right;
            }
            else if (v.x < 0)
            {
                lastDirection = AttackDirection.Left;
            }
        }

        void DoAttack()
        {
            AttackDirection direction = GetAttackDirection();
            //GameObject hitBox = Instantiate(attackHitbox);
            RaycastHit2D[] hitList;

            switch(direction)
            {
                case AttackDirection.Up:
                    //hitBox.transform.localScale = new Vector3(1, 5, 1);
                    //hitBox.transform.position = transform.position + new Vector3(0, 2.5f + 2.5f);
                    hitList = BoxCast(transform.position + new Vector3(0, 1.5f), new Vector3(2, 2), 0, Vector3.up, 5, ~ignoreLayers);
                    break;
                case AttackDirection.Down:
                    //hitBox.transform.localScale = new Vector3(1, 5, 1);
                    //hitBox.transform.position = transform.position + new Vector3(0, -2.5f - 2.5f);
                    hitList = BoxCast(transform.position + new Vector3(0, -1.5f), new Vector3(2, 2), 0, -Vector3.up, 5, ~ignoreLayers);
                    break;
                case AttackDirection.Left:
                    //hitBox.transform.localScale = new Vector3(5, 1, 1);
                    hitList = BoxCast(transform.position + new Vector3(-1, 0.5f), new Vector3(2, 2), 0, -Vector3.right, 5, ~ignoreLayers);
                    //hitBox.transform.position = transform.position + new Vector3(-2.5f - 2.5f, 0);
                    break;

                default:
                    //hitBox.transform.localScale = new Vector3(5, 1, 1);
                    //hitBox.transform.position = transform.position + new Vector3(2.5f + 2.5f, 0);
                    hitList = BoxCast(transform.position + new Vector3(1, 0.5f), new Vector3(2, 2), 0, Vector3.right, 5, ~ignoreLayers);
                    break;
            }

            foreach (RaycastHit2D hit in hitList)
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.damage(1);
                }
            }

        }

        AttackDirection GetAttackDirection()
        {
            
            Vector3 v = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (v.x > 0)
            {
                return AttackDirection.Right;
            } else if (v.x < 0)
            {
                return AttackDirection.Left;
            } else if (v.y > 0)
            {
                return AttackDirection.Up;
            } else if (v.y < -0)
            {
                return AttackDirection.Down;
            }

            else return lastDirection;
        }

        public RaycastHit2D[] BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int mask)
        {
            RaycastHit2D[] hit = Physics2D.BoxCastAll(origin, size, angle, direction, distance, mask);

            if (attackDebugVisualization)
            {
                GameObject vis = Instantiate(attackDebugVisualization);
                vis.transform.localScale = size + new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y)) * distance;
                vis.transform.position = new Vector3(origin.x, origin.y) + new Vector3(direction.x * ((vis.transform.localScale.x - size.x) / 2), direction.y * ((vis.transform.localScale.y - size.y) / 2));
                Destroy(vis, 2);
            }

            //Setting up the points to draw the cast
            Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
            float w = size.x * 0.5f;
            float h = size.y * 0.5f;
            p1 = new Vector2(-w, h);
            p2 = new Vector2(w, h);
            p3 = new Vector2(w, -h);
            p4 = new Vector2(-w, -h);

            Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
            p1 = q * p1;
            p2 = q * p2;
            p3 = q * p3;
            p4 = q * p4;

            p1 += origin;
            p2 += origin;
            p3 += origin;
            p4 += origin;

            Vector2 realDistance = direction.normalized * distance;
            p5 = p1 + realDistance;
            p6 = p2 + realDistance;
            p7 = p3 + realDistance;
            p8 = p4 + realDistance;


            //Drawing the cast
            Color castColor = hit.Length > 0 ?  Color.green : Color.red;
            Debug.DrawLine(p1, p2, castColor, 2, false);
            Debug.DrawLine(p2, p3, castColor, 2, false);
            Debug.DrawLine(p3, p4, castColor, 2, false);
            Debug.DrawLine(p4, p1, castColor, 2, false);

            Debug.DrawLine(p5, p6, castColor, 2, false);
            Debug.DrawLine(p6, p7, castColor, 2, false);
            Debug.DrawLine(p7, p8, castColor, 2, false);
            Debug.DrawLine(p8, p5, castColor, 2, false);

            Debug.DrawLine(p1, p5, Color.blue, 2, false);
            Debug.DrawLine(p2, p6, Color.blue, 2, false);
            Debug.DrawLine(p3, p7, Color.blue, 2, false);
            Debug.DrawLine(p4, p8, Color.blue, 2, false);
            if (hit.Length > 0)
            {
                //Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow, 2, false);
            }

            return hit;
        }
    }
}
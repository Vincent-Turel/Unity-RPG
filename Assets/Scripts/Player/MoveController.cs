using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float movingTurnSpeed = 360;
    public float stationaryTurnSpeed = 180;

    private Rigidbody rigidbody;
    private Animator animator;
    private float turnAmount;
    private float forwardAmount;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                RigidbodyConstraints.FreezeRotationZ;
    }


    public void Move(Vector3 move)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        turnAmount = Mathf.Atan2(move.x, move.z);
        forwardAmount = move.z;

        ApplyExtraTurnRotation();

        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    private void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }


    private void OnAnimatorMove()
    {
        if (Time.deltaTime > 0)
        {
            Vector3 v = animator.deltaPosition / Time.deltaTime;

            v.y = rigidbody.velocity.y;
            rigidbody.velocity = v;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    
    public void EndOfAttack()
    {
        animator.ResetTrigger("Attack");
    }
    
    public void Die()
    {
        animator.SetBool("Dead", true);
    }
    public void Hit()
    {
        animator.SetTrigger("Hit");
    } 
    
    public void ResetHit()
    {
        animator.ResetTrigger("Hit");
    }
    
    public float getDurationClip()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
}
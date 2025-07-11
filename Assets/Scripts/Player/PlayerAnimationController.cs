using UnityEngine;

public class PlayerAnimationController : PausedBehaviour
{
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Метод для установки направления движения из другого скрипта
    public void SetMovement(Vector2 move)
    {
        movement = move;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (movement.magnitude > 0)
        {
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}
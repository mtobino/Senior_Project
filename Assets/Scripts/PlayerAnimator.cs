using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_MOVING = "IS_MOVING";
    private const string IS_CROUCHING = "IS_CROUCHING";
    private const string ON_GROUND = "ON_GROUND";
    private const string IS_SPRINTING = "IS_SPRINTING";
    private Animator animator;
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Awake()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_MOVING, player.IsMoving());
        animator.SetBool(IS_CROUCHING, player.IsCrouching());
        animator.SetBool(ON_GROUND, player.OnGround());
        animator.SetBool(IS_SPRINTING, player.IsSprinting());  
    }
}

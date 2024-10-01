using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const string IS_MOVING = "IS_MOVING";
    private const string IS_SPRINTING = "IS_SPRINTING";
    private Animator animator;
    [SerializeField] private EnemyAI enemy;
    // Start is called before the first frame update
    void Awake()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_MOVING, enemy.isMoving());
        //animator.SetBool(IS_SPRINTING, enemy.isSprinting());  
    }
}

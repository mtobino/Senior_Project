using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoEnemyAnimator : MonoBehaviour
{
    private const string IS_MOVING = "IS_MOVING";
    private Animator animator;
    [SerializeField] private Level2EnemyAI enemy;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_MOVING, enemy.isMoving);
    }
}

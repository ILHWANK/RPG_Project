using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    // Character Type
    public enum CharacterType
    {
        Dealer,
        Tanker,
        Supporter
    }

    // State Enum
    public enum CharacterState
    {
        None,
        Idle,
        Search,
        Move,
        Attack,
        Die,
        Victory
    }

    public Enemy[] targetObjects;
    public Rigidbody2D rigidbody2D;
    public Animator animator;

    public CharacterType characterType;
    public CharacterState characterState;

    // Character Stat
    public float moveSpeed;
    public float power;
    public float hp;
    public float attackRange;

    public bool isAlive;

    void Awake()
    {
        transform.localScale = new Vector3(-1, 1, 1);

        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        characterState = CharacterState.Move;
    }

    void Start()
    {
        characterState = CharacterState.Move;

        animator.SetTrigger("WalkTrigger");
    }

    void FixedUpdate()
    {
        if (!isAlive)
            return;

        targetObjects = FindObjectsOfType<Enemy>();

        Vector2 allyPosition = rigidbody2D.position;

        Enemy targetEnemy = null;
        float targetDistance = 0f;

        // Search Target
        for (int i = 0; i < targetObjects.Length; ++i)
        {
            if (i == 0)
            {
                targetDistance = Vector2.Distance(allyPosition, targetObjects[i].transform.localPosition);
                targetEnemy = targetObjects[i];
            }
            else
            {
                if (targetDistance > Vector2.Distance(allyPosition, targetObjects[i].transform.localPosition))
                {
                    targetEnemy = targetObjects[i];
                }
            }
        }

        // Character State
        if (targetEnemy != null)
        {
            switch (characterState)
            {
                case CharacterState.None:
                    {
                        break;
                    }
                case CharacterState.Idle:
                    {
                        break;
                    }
                case CharacterState.Move:
                    {
                        Vector2 targetPosition = new Vector2(targetEnemy.transform.position.x, targetEnemy.transform.position.y);

                        Vector2 dirVec = targetPosition - allyPosition;
                        Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;

                        rigidbody2D.MovePosition(rigidbody2D.position + nextVec);
                        rigidbody2D.velocity = Vector2.zero;

                        break;
                    }
                case CharacterState.Attack:
                    {
                        StartCoroutine(AttackCoroutine());
                        break;
                    }
                case CharacterState.Die:
                    {
                        Dead();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    void LateUpdate()
    {
        if (!isAlive)
            return;
        // ..방향 전환이 필요한 경우 추가
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            characterState = CharacterState.Attack;
        }
    }

    IEnumerator AttackCoroutine()
    {
        animator.SetTrigger("AttackTrigger");

        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("WalkTrigger");

        characterState = CharacterState.Move;
    }

    void Dead()
    {
        animator.SetTrigger("AttackTrigger");
        isAlive = false;
        gameObject.SetActive(false);
    }
}

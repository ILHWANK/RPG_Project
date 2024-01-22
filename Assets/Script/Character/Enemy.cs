using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Character Type
    public enum CharacterType
    {
        Dealers,
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
        Casting,
        Attack,
        Hurt,
        Die,
        Victory
    }

    public Ally[] targetObjects;
    public Rigidbody2D rigidbody2D;
    public Animator animator;

    WaitForFixedUpdate wait;

    IEnumerator charactorActionCoroutine;
    
    public CharacterType characterType;
    public CharacterState characterState;

    // Character Stat
    public float moveSpeed;
    public float power;
    public float curHp;
    public float maxHp;
    public float attackRange;
    public float attackDelay;
    public float skillCoolTimeMax;
    public float skillCoolTimeCur;

    public float damage;

    public string skillName;
    public string skillInfo;

    public bool isAlive;

    Ally moveTarget   = null;
    Ally attackTarget = null;

    GameManager gameManager;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        wait = new WaitForFixedUpdate();

        curHp = maxHp;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        characterState = CharacterState.Move;

        CharacterAction();
    }

    void FixedUpdate()
    {
        if (!isAlive || curHp <= 0)
        {
            characterState = CharacterState.Die;

            return;
        }
        else
        {
            targetObjects = FindObjectsOfType<Ally>();

            Vector2 enemyPosition = rigidbody2D.position;
            float targetDistance = 0f;

            for (int i = 0; i < targetObjects.Length; ++i)
            {
                if (false)
                { // targetObjects[i].characterType == Enemy.CharacterType.Tanker
                    moveTarget = targetObjects[i];

                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        targetDistance = Vector2.Distance(enemyPosition, targetObjects[i].rigidbody2D.position);
                        moveTarget = targetObjects[i];
                    }
                    else if (targetDistance > Vector2.Distance(enemyPosition, targetObjects[i].rigidbody2D.position))
                    {
                        moveTarget = targetObjects[i];
                    }
                }
            }

            if (moveTarget != null)
            {
                Vector2 targetPosition = new Vector2(moveTarget.transform.position.x, moveTarget.transform.position.y);

                Vector2 dirVec = targetPosition - enemyPosition;
                Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;

                if (dirVec.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                targetDistance = Vector2.Distance(enemyPosition, targetPosition);

                if (targetDistance <= attackRange
                    && characterState != CharacterState.Attack
                    && characterState != CharacterState.Hurt)
                {
                    attackTarget = moveTarget;

                    characterState = CharacterState.Attack;

                    CharacterAction();
                }
                else if (characterState == CharacterState.Move)
                {
                    attackTarget = null;

                    animator.SetTrigger("WalkTrigger");

                    rigidbody2D.MovePosition(rigidbody2D.position + nextVec);
                    rigidbody2D.velocity = Vector2.zero;
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

    public void SetCharacterSkill()
    {

    }

    public void CharacterAction()
    {
        if (charactorActionCoroutine != null)
        {
            StopCoroutine(charactorActionCoroutine);

            charactorActionCoroutine = null;
        }

        charactorActionCoroutine = CharactorActionCoroutine();

        StartCoroutine(charactorActionCoroutine);
    }

    IEnumerator CharactorActionCoroutine()
    {
        Vector2 allyPosition = rigidbody2D.position;

        switch (characterState)
        {
            case CharacterState.None:
                {
                    break;
                }
            case CharacterState.Idle:
                {
                    animator.SetTrigger("IdleTrigger");

                    break;
                }
            case CharacterState.Move:
                {
                    animator.SetTrigger("WalkTrigger");

                    break;
                }
            case CharacterState.Casting:
                {
                    animator.SetTrigger("WalkTrigger");

                    while (!animator.GetCurrentAnimatorStateInfo(0).IsName("casting"))
                        yield return null;

                    characterState = CharacterState.Attack;

                    CharacterAction();

                    break;
                }
            case CharacterState.Attack:
                {

                    animator.SetTrigger("AttackTrigger");

                    yield return new WaitForSeconds(attackDelay);

                    if (attackTarget.gameObject.activeSelf)
                    {
                        attackTarget.damage = power;
                        attackTarget.characterState = Ally.CharacterState.Hurt;
                        attackTarget.CharacterAction();

                        attackTarget = null;
                    }

                    characterState = CharacterState.Move;

                    break;
                }
            case CharacterState.Hurt:
                {
                    animator.SetTrigger("HurtTrigger");

                    Vector3 dirVec = transform.position - moveTarget.transform.position;

                    rigidbody2D.AddForce(dirVec.normalized, ForceMode2D.Impulse);

                    yield return new WaitForSeconds(0.25f);
                    //yield return new WaitForSeconds(1f);

                    curHp -= damage;

                    gameManager.enemyCurHp -= damage;

                    damage = 0;

                    if (curHp <= 0)
                    {
                        curHp = 0;

                        characterState = CharacterState.Die;

                        CharacterAction();
                    }
                    else
                    {
                        characterState = CharacterState.Move;
                    }

                    break;
                }
            case CharacterState.Die:
                {
                    animator.SetTrigger("DieTrigger");

                    while (!animator.GetCurrentAnimatorStateInfo(0).IsName("die"))
                        yield return null;

                    isAlive = false;
                    gameObject.SetActive(false);

                    break;
                }
            default:
                {
                    break;
                }
        }

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {

        }
    }
}

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
        Casting,
        Attack,
        Hurt,
        Die,
        Victory
    }

    public enum SkillBuffType
    {
        power,
        moveSpeed,
        attackRange
    }

    public Enemy[] targetObjects;
    public Rigidbody2D rigidbody2D;
    public Animator animator;

    WaitForFixedUpdate wait;

    IEnumerator charactorActionCoroutine;
    IEnumerator skillActionCoroutine;

    public CharacterType characterType;
    public CharacterState characterState;
    public SkillBuffType skillBuffType;

    // Character Stat
    public float moveSpeed;
    public float power;
    public float curHp;
    public float maxHp;
    public float attackRange;
    public float attackDelay;
    public float skillCoolTimeMax;
    public float skillCoolTimeCur;
    public float skillStat;

    public float damage;

    public string skillName;
    public string skillInfo;

    public bool isAlive;

    Enemy moveTarget   = null;
    Enemy attackTarget = null;

    GameManager gameManager;
    ChapterUIManager chapterUIManager;

    void Awake()
    {
        transform.localScale = new Vector3(-1, 1, 1);

        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        wait = new WaitForFixedUpdate();

        curHp = maxHp;

        skillCoolTimeCur = skillCoolTimeMax;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        chapterUIManager = FindObjectOfType<ChapterUIManager>();

        characterState = CharacterState.Move;

        CharacterAction();
    }

    void FixedUpdate()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (!isAlive || curHp <= 0)
        {
            characterState = CharacterState.Die;

            return;
        }
        else
        {
            targetObjects = FindObjectsOfType<Enemy>();

            Vector2 allyPosition = rigidbody2D.position;
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
                        targetDistance = Vector2.Distance(allyPosition, targetObjects[i].rigidbody2D.position);
                        moveTarget = targetObjects[i];
                    }
                    else if (targetDistance > Vector2.Distance(allyPosition, targetObjects[i].rigidbody2D.position))
                    {
                        moveTarget = targetObjects[i];
                    }
                }
            }

            if (moveTarget != null)
            {
                Vector2 targetPosition = new Vector2(moveTarget.transform.position.x, moveTarget.transform.position.y);

                Vector2 dirVec = targetPosition - allyPosition;
                Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;

                if (dirVec.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                targetDistance = Vector2.Distance(targetPosition, allyPosition);

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
                    animator.SetTrigger("WalkTrigger");

                    rigidbody2D.MovePosition(rigidbody2D.position + nextVec);
                    rigidbody2D.velocity = Vector2.zero;
                }
            }

            if (skillCoolTimeCur > 0)
            {
                skillCoolTimeCur -= Time.deltaTime;
            }
        }
    }

    void LateUpdate()
    {
        if (!isAlive)
            return;
    }

    public bool SkillActive()
    {
        return skillCoolTimeCur <= 0;
    }

    public void CharacterAction() {

        if (charactorActionCoroutine != null)
        {
            StopCoroutine(charactorActionCoroutine);

            charactorActionCoroutine = null;
        }

        charactorActionCoroutine = CharactorActionCoroutine();

        StartCoroutine(charactorActionCoroutine);
    }

    public void SkillAction()
    {
        if (skillActionCoroutine != null)
        {
            StopCoroutine(skillActionCoroutine);

            skillActionCoroutine = null;
        }

        skillActionCoroutine = SkillActionCoroutine();

        StartCoroutine(SkillActionCoroutine());
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
                    animator.SetTrigger("CastingTrigger");

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
                        attackTarget.characterState = Enemy.CharacterState.Hurt;
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

                    gameManager.allyCurHp -= damage;

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

    IEnumerator SkillActionCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        switch (skillBuffType)
        {
            case SkillBuffType.power:
                {
                    power += skillStat;

                    break;
                }
            case SkillBuffType.moveSpeed:
                {
                    moveSpeed += skillStat;

                    break;
                }
            case SkillBuffType.attackRange:
                {
                    attackRange += skillStat;

                    break;
                }
            default: break;
        }

        skillCoolTimeCur = skillCoolTimeMax;

        chapterUIManager.CloseSkillAllyInfo();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            
        }
    }
}

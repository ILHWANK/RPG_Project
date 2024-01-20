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
        Attack,
        Hurt,
        Die,
        Victory
    }

    public Ally[] targetObjects;
    public Rigidbody2D rigidbody2D;
    public Animator animator;

    WaitForFixedUpdate wait;

    public CharacterType characterType;
    public CharacterState characterState;

    // Character Stat
    public float moveSpeed;
    public float power;
    public float hp;
    public float attackRange;

    public bool isAlive;

    Ally targetAlly = null;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        wait = new WaitForFixedUpdate();

        characterState = CharacterState.Move;
    }

    void Start()
    {
        characterState = CharacterState.Move;
    }

    void FixedUpdate()
    {
        if (!isAlive)
        {
            characterState = CharacterState.Die;

            return;
        }

        targetObjects = FindObjectsOfType<Ally>();

        Vector2 allyPosition = rigidbody2D.position;

        float targetDistance = 0f;

        for (int i = 0; i < targetObjects.Length; ++i)
        {
            if (i == 0)
            {
                targetDistance = Vector2.Distance(allyPosition, targetObjects[i].transform.localPosition);
                targetAlly = targetObjects[i];
            }
            else
            {
                if (targetDistance > Vector2.Distance(allyPosition, targetObjects[i].transform.localPosition))
                {
                    targetAlly = targetObjects[i];
                }
            }
        }

        //
        if (targetAlly != null)
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
                        Vector2 targetPosition = new Vector2(targetAlly.transform.position.x, targetAlly.transform.position.y);

                        Vector2 dirVec = targetPosition - allyPosition;
                        Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;

                        rigidbody2D.MovePosition(rigidbody2D.position + nextVec);
                        rigidbody2D.velocity = Vector2.zero;

                        break;
                    }
                case CharacterState.Attack:
                    {
                        break;
                    }
                case CharacterState.Hurt:
                    {

                        StartCoroutine(KnockBack());

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
        if (collision.tag == "Ally")
        {
            hp -= collision.gameObject.GetComponent<Ally>().power;
            characterState = CharacterState.Hurt;

            if (hp <= 0)
            {
                characterState = CharacterState.Die;
            }
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 dirVec = transform.position - targetAlly.transform.position;

        rigidbody2D.AddForce(dirVec.normalized, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);

        characterState = CharacterState.Move;
    }

    void Dead()
    {
        isAlive = false;
        gameObject.SetActive(false);
    }
}

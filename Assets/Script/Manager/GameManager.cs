using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ally[]  allys;
    public Enemy[] enemys;

    public float gameSpeed = 1;

    public bool isVictory = false;
    public bool isDefeat  = false;


    public float allyTotalHp = 0;
    public float allyCurHp = 0;

    public float enemyTotalHp = 0;
    public float enemyCurHp = 0;

    public float chapterMaxTime;
    public float chapterCurTime;

    IEnumerator pauseCoroutine = null;

    void Start()
    {
        gameSpeed = 1;

        allys = FindObjectsOfType<Ally>();
        enemys = FindObjectsOfType<Enemy>();

        for (int i = 0; i < allys.Length; ++i)
        {
            allyTotalHp += allys[i].maxHp;
        }

        allyCurHp = allyTotalHp;

        for (int i = 0; i < enemys.Length; ++i)
        {
            enemyTotalHp += enemys[i].maxHp;
        }

        enemyCurHp = enemyTotalHp;
    }

    void Update()
    {
        allys  = FindObjectsOfType<Ally>();
        enemys = FindObjectsOfType<Enemy>();

        int allyAliveCount  = allys.Length;
        int enemyAliveCount = enemys.Length;

        chapterCurTime += Time.deltaTime;

        for (int i = 0; i < allys.Length; ++i)
        {
            if (!allys[i].isAlive)
                allyAliveCount--;
        }

        for (int i = 0; i < enemys.Length; ++i)
        {
            if (!enemys[i].isAlive)
                enemyAliveCount--;
        }

        if (chapterMaxTime - chapterCurTime <= 0)
        {
            chapterCurTime = 0;

            isDefeat = true;
        }

        if (allyAliveCount == 0)
        {
            isDefeat = true;
        }

        if (enemyAliveCount == 0)
        {
            isVictory = true;
        }
    }

    public Ally[] GetAllyAll()
    {
        return allys;
    }

    public Enemy[] GetEnemyAll()
    {
        return enemys;
    }

    public Ally GetAlly(int pIndex)
    {
        return allys[pIndex];
    }

    public Enemy GetEnemy(int pIndex)
    {
        return enemys[pIndex];
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
}

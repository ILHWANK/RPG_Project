using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ally[]  Allys;
    public Enemy[] Enemys;

    public float gameSpeed = 1;

    public bool isVictory = false;
    public bool isDefeat  = false;

    IEnumerator pauseCoroutine = null;

    void Start()
    {
        gameSpeed = 1;
    }

    void Update()
    {
        Allys  = FindObjectsOfType<Ally>();
        Enemys = FindObjectsOfType<Enemy>();

        int allyAliveCount  = Allys.Length;
        int enemyAliveCount = Enemys.Length;

        for (int i = 0; i < Allys.Length; ++i)
        {
            if (!Allys[i].isAlive)
                allyAliveCount--;
        }

        for (int i = 0; i < Enemys.Length; ++i)
        {
            if (!Enemys[i].isAlive)
                enemyAliveCount--;
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
}

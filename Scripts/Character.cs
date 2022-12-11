using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : VulnerableObject
{
    [SerializeField] private GameObject enemy;

    private void Start()
    {
        health = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            health -= 20;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            enemy.SetActive(false);
        }
    }
}

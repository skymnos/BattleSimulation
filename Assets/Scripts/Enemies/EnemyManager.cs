using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum State
{
    idle,
    attack,
    chase
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float defense;
    [SerializeField] private float speed;
    [SerializeField] private float probabilityOfPary;
    [SerializeField] private float viewDistance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float attack;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float defence;
    [SerializeField] private float probabilityOfPary;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float viewDistance;
    [SerializeField] private float intimidation;
    [SerializeField] private float maxEnergy;

    private float fear;
    private float currentSpeed;
    private float currentEnergy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

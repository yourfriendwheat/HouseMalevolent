using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatorController : MonoBehaviour
{
    public NavMeshAgent Enemy;
    Animator hotguy;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = this.GetComponent<NavMeshAgent>();
        hotguy = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

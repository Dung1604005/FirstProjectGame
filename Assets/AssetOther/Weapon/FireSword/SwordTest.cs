using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTest : MonoBehaviour
{
    public Animator animatorSword;

    


    public GameObject gameObject;

    void Start()
    {
        animatorSword = GetComponent<Animator>();


    }
    public void TurnOnSlashEffect()
    {
        gameObject.SetActive(true);
        
    }
    public void EndAttack()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animatorSword.SetTrigger("attack");
            

        }
    }
}

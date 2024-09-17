using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    public float Hp;
    public int Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    public bool isBigGoblin = false;


    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    private bool isDead = false;
    public GameObject miniGoblinPrefab;


    private void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);

    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            Agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
     
        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.UpdatePlayerHP(-Damage);
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.isStopped = false; // fix
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("Speed", Agent.speed);
    }
    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");
        SceneManager.Instance.Player.UpdatePlayerHP(3);
        if (isBigGoblin) { ThrowMiniGoblins(); }

        StartCoroutine(RemoveOffset());
    }

    private void ThrowMiniGoblins()
    {
        
        GameObject miniGoblin1 = Instantiate(miniGoblinPrefab, transform.position, Quaternion.identity);
        GameObject miniGoblin2 = Instantiate(miniGoblinPrefab, transform.position, Quaternion.identity);
       
        Vector3 direction1 = (transform.right + transform.up).normalized; 
        Vector3 direction2 = (-transform.right + transform.up).normalized; 

        float moveSpeed = 10f; 
        float duration = 0.5f; 

        StartCoroutine(MoveMiniGoblin(miniGoblin1.transform, direction1, moveSpeed, duration));
        StartCoroutine(MoveMiniGoblin(miniGoblin2.transform, direction2, moveSpeed, duration));
    }

    private IEnumerator MoveMiniGoblin(Transform goblin, Vector3 direction, float speed, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            goblin.Translate(direction * speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RemoveOffset()
    {
        yield return new WaitForSeconds(1);
        SceneManager.Instance.RemoveEnemie(this);
    }

}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float Hp;
    public Text hpText;
    public float Damage;
    public float Speed = 5f;
    public float RotationSpeed = 5f;
    public float AttackRange = 2;
    private bool isDead = false;
    public Animator AnimatorController;
    public Rigidbody Rigidbody;
    public BtnAttack btnAttack;
    public BtnSAttack btnSAttack;
    public GameObject Sbtn;

    private void Start()
    {
        UpdatePlayerHP();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Hp = 0;
            Die();
            return;
        }

        Move();
        AttackListener();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ) * Speed * Time.deltaTime;

        if (movement.magnitude > 0)  
        {
            Rigidbody.MovePosition(transform.position + movement);
            AnimatorController.SetFloat("Speed", movement.magnitude * Speed);  
        }
        else
        {
            AnimatorController.SetFloat("Speed", 0);  
        }
    }
   

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

    private void AttackListener()
    {
        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;
        float closestDistance = float.MaxValue;

        foreach (var enemie in enemies)
        {
            if (enemie == null) continue;

            var distance = Vector3.Distance(transform.position, enemie.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemie = enemie;
            }
        }

        if (closestEnemie != null)
        {
            
            transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);
            if (btnAttack.IsAttackReady())
            {
                AnimatorController.SetTrigger("Attack");
                btnAttack.OnAttackButtonClick();
                if (closestDistance <= AttackRange) { closestEnemie.Hp -= Damage; }
                    
            }

            if (closestDistance <= AttackRange)
            {
                Sbtn.SetActive(true); 
                if (btnSAttack.IsAttackReady())
                {
                    AnimatorController.SetTrigger("Super Attack");
                    btnSAttack.OnAttackButtonClick();
                    closestEnemie.Hp -= Damage * 2;
                }
            }
            else
            {
                Sbtn.SetActive(false); 
            }
        }
        else
        {
            Sbtn.SetActive(false); 
        }
    }

    public void UpdatePlayerHP(int value = 0)
    {
        Hp += value;
        hpText.text = $"HP {Hp}";
    }

}

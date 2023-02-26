using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMgr : MonoBehaviour
{
    public int Maxhealth = 50;
    public int currentHealth;

    private float timer = 0f;

    public AudioClip[] ImpactAudios;

    public float invincibilityTime = 1f; // The time the player is invincible after taking damage
    public float knockbackForce = 10f; // The force applied to the player when taking damage

    private bool isInvincible = false; // Tracks whether the player is invincible or not

    private ScoreManager scoreManager;

    private UIManager UIMgr;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Maxhealth;
        scoreManager = FindObjectOfType<ScoreManager>();
        UIMgr = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            timer = 0f;
            DecreaseHealth(1);
        }

        UIMgr.OnHp(currentHealth);
    }

    // Called when the player takes damage
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;

            UIMgr.OnHp(currentHealth);

            if (ImpactAudios.Length > 0)
            {
                AudioSource.PlayClipAtPoint(ImpactAudios[Random.Range(0, ImpactAudios.Length)], transform.position);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(BecomeInvincible());
            }
        }
    }

    // Makes the player invincible for a short time
    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Called when the player dies
    void Die()
    {
        //Destroy(gameObject);
        scoreManager.OnPlayerDeath();
        SceneManager.LoadScene(0);
    }
}

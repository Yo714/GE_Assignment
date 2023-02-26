using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMgr : MonoBehaviour
{
    public float health = 100;
    public AudioClip[] ImpactAudios;

    public float invincibilityTime = 1f; // The time the player is invincible after taking damage
    public float knockbackForce = 10f; // The force applied to the player when taking damage

    private bool isInvincible = false; // Tracks whether the player is invincible or not

    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the player takes damage
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            health -= damage;

            if (ImpactAudios.Length > 0)
            {
                AudioSource.PlayClipAtPoint(ImpactAudios[Random.Range(0, ImpactAudios.Length)], transform.position);
            }

            if (health <= 0)
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

    // Called when the player dies
    void Die()
    {
        //Destroy(gameObject);
        scoreManager.OnPlayerDeath();
        SceneManager.LoadScene(0);
    }
}

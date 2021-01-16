using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public struct HealthInfo {
        public float currentHealth;
        public float basicHealthSet;
        public bool isDead;
    }
    public struct DamageTextInfo {
        public float finalDamage;
        public bool isCritical;
        public Vector3 position;
    }
    [HideInInspector] public float armor;
    private float currentHealth;
    public float CurrentHealth {
        get { return currentHealth; }
        set {
            currentHealth = value;
            if (currentHealth > BasicHealthSet) {
                currentHealth = BasicHealthSet; // DamageText
                EventManager.Instance.UpdateEvent("Health_MaxHealth", new DamageTextInfo {
                    finalDamage = BasicHealthSet,
                    position = transform.position,
                });
            }
            if (transform.gameObject.CompareTag("Player"))
                EventManager.Instance.UpdateEvent("Health_UpdateHealthUI",
                    new HealthInfo {
                        currentHealth = CurrentHealth,
                        basicHealthSet = BasicHealthSet,
                        isDead = IsDead
                    });
        }
    }
    private ParticleSystem bloodFx;
    private ParticleSystem deadFx;
    private bool isDead;
    public bool IsDead {
        get { return isDead; }
        set {
            isDead = value;
            if (isDead) {
                transform.gameObject.SetActive(false);
                bloodFx = Instantiate(bloodFxPrefab, transform.position, Quaternion.identity);
                deadFx = Instantiate(deadFxPrefab, transform.position, Quaternion.identity);

                bloodFx.transform.localScale = Vector3.one * transform.localScale.x / 2f;
                deadFx.transform.localScale = Vector3.one * transform.localScale.x / 2f;

                EventManager.Instance.UpdateEvent("Health_TransformIsDead", BasicHealthSet);

                if (transform.gameObject.CompareTag("Player")) { // TutorialText
                    Utility.ShakeCamera(.7f, .2f);
                    EventManager.Instance.UpdateEvent("Health_PlayerIsDead", null);
                    Invoke("RestartLevel", 4f);
                } else {
                    GameManager.instance.EnemiesKilled++;
                }
            } else {
                transform.gameObject.SetActive(true);
            }
        }
    }
    [SerializeField] private float armorSet;
    [SerializeField] private float BasicHealthSet;
    [SerializeField] private ParticleSystem deadFxPrefab;
    [SerializeField] private ParticleSystem bloodFxPrefab;
    private SpriteRenderer spriteRd;

    private void Awake() {
        spriteRd = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }
    private void Start() {
        CurrentHealth = BasicHealthSet;
        if (armorSet < 100)
            armor = armorSet;
        else
            armor = 99;
    }
    public void RestartLevel() {
        LoadScene.LoadSceneName(SceneName.Gameplay);
    }
    public void GainHealth(float health, Vector3 position) {
        CurrentHealth += health;
        spriteRd.color = Color.green;
        Invoke("ResetColor", .2f);
        EventManager.Instance.UpdateEvent("Health_GainHealth",
            new DamageTextInfo {
                finalDamage = health,
                position = position,
            });
    }
    public void TakeDamage(float damage, bool isCritical, Vector3 position) {
        CurrentHealth -= (100 - armor) / 100 * damage;
        spriteRd.color = Color.red;
        Invoke("ResetColor", .2f);
        if (CurrentHealth <= 0) {
            CurrentHealth = 0;
            IsDead = true;
        }
        EventManager.Instance.UpdateEvent("Health_Hit",
            new DamageTextInfo {
                finalDamage = damage,
                isCritical = isCritical,
                position = position,
            });
    }
    private void ResetColor() {
        spriteRd.color = Color.white;
    }
    public void ResetToOriginState() {
        IsDead = false;
        CurrentHealth = BasicHealthSet;
        EventManager.Instance.UpdateEvent("Health_Revive", null);
    }
}
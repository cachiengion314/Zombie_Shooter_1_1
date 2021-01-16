using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCanvas : MonoBehaviour {
    [SerializeField] private Transform damageTextPrefab;
    private float scaleFactor = 1f;

    private void Start() {
        EventManager.Instance.ListenTo("Health_Hit", OnDamageTextUI);
        EventManager.Instance.ListenTo("Health_GainHealth", OnGainHealthTextUI);
        EventManager.Instance.ListenTo("Health_MaxHealth", OnGainMaxHealth);
    }
    private void OnGainMaxHealth(object data) {
        var info = (Health.DamageTextInfo) data;
        TextMeshProUGUI text = CreateDamageText(info.position, info.finalDamage, Color.cyan, Color.red);
        text.SetText("MAX HP");
    }
    private void OnGainHealthTextUI(object data) {
        var info = (Health.DamageTextInfo) data;
        CreateDamageText(info.position, info.finalDamage, Color.green, Color.red);
    }
    private void OnDamageTextUI(object data) {
        Health.DamageTextInfo damageInfo = (Health.DamageTextInfo) data;
        CreateDamageText(damageInfo.position, damageInfo.finalDamage, Color.yellow, Color.red, damageInfo.isCritical);
    }
    public TextMeshProUGUI CreateDamageText(Vector3 position, float damage, Color normal, Color intensive, bool isCritical = false) {
        Transform damageTextTransform = Instantiate(
            damageTextPrefab, position, Quaternion.identity);
        damageTextTransform.SetParent(transform, true);
        damageTextTransform.localScale = Vector3.one * scaleFactor;

        TextMeshProUGUI damageText = damageTextTransform.GetComponent<TextMeshProUGUI>();
        damageText.SetText(Mathf.FloorToInt(damage).ToString());

        if (!isCritical) {
            damageText.fontSize = 30;
            damageText.color = normal;
        } else {
            damageText.fontSize = 40;
            damageText.color = intensive;
        }
        return damageText;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private Slider reloadSlide;
    private Slider heathSlider;
    // Enemies Killed Text
    private TextMeshProUGUI enemiesKilledText;
    // GemText
    private TextMeshProUGUI gemsText;
    // PlayerHealth Update
    private float playerCurrentHealth;
    private float playerBasicHealth;
    private float playerHealthPercent;
    private bool isPlayerDead;
    private bool isNeedToUpdateHealth;
    // ReloadUI Update
    private float startFireTime;
    private float currentTime;
    private float fireRate;
    private bool isNeedToUpdateReload;
    // Slot
    private Image slot0;
    private Image slot1;
    private Image slot2;

    private Color defaultColor;
    private Color selectedColor;

    private Image pistol;
    private Image shotgun;
    private Image rifts;
    private Image sniper;
    private Image aug;
    private Image dualBerretas;
    private List<Image> allSlots;
    private List<Image> allWeapons;

    private void Awake() {
        reloadSlide = transform.Find("ReloadSlide").GetComponent<Slider>();
        heathSlider = transform.Find("HeathSlide").GetComponent<Slider>();
        slot0 = transform.Find("Slot0").GetComponent<Image>();
        slot1 = transform.Find("Slot1").GetComponent<Image>();
        slot2 = transform.Find("Slot2").GetComponent<Image>();

        pistol = transform.Find("Pistol").GetComponent<Image>();
        shotgun = transform.Find("ShotGun").GetComponent<Image>();
        rifts = transform.Find("Rifts").GetComponent<Image>();
        sniper = transform.Find("Sniper").GetComponent<Image>();
        aug = transform.Find("AUG").GetComponent<Image>();
        dualBerretas = transform.Find("DualBerretas").GetComponent<Image>();
        gemsText = transform.Find("GemsText").GetComponent<TextMeshProUGUI>();
        enemiesKilledText = transform.Find("EnemiesKilledText").GetComponent<TextMeshProUGUI>();

        gemsText.SetText("X" + GameManager.instance.GemsCount);
        pistol.gameObject.SetActive(false);
        shotgun.gameObject.SetActive(false);
        rifts.gameObject.SetActive(false);
        sniper.gameObject.SetActive(false);
        aug.gameObject.SetActive(false);
        dualBerretas.gameObject.SetActive(false);
        allWeapons = new List<Image> {
            pistol,
            shotgun,
            rifts,
            sniper,
            aug,
            dualBerretas
        };
        allSlots = new List<Image> {
            slot0,
            slot1,
            slot2
        };
        selectedColor = slot0.color;
        defaultColor = slot1.color;
        reloadSlide.value = 100;
        heathSlider.value = 100;
    }
    private void Start() {
        EventManager.Instance.ListenTo("HeroController_Shoot", OnUpdateReloadUI);
        EventManager.Instance.ListenTo("HeroController_PressChangingItem", OnChangingSlotUI);
        EventManager.Instance.ListenTo("HeroController_ThrowAwayWeapon", OnThrowAwayItems);
        EventManager.Instance.ListenTo("HeroController_AddItem", OnAddToItemsList);

        EventManager.Instance.ListenTo("GameManager_AddGemsUI", OnAddGemsUI);
        EventManager.Instance.ListenTo("GameManager_EnemiesKilled", OnEnemiesKilled);

        EventManager.Instance.ListenTo("Health_UpdateHealthUI", OnUpdateHealthBarUI);
    }
    private void OnEnemiesKilled(object data) {
        enemiesKilledText.SetText("Killed: " + GameManager.instance.EnemiesKilled);
    }
    private void OnAddGemsUI(object data) {
        gemsText.SetText("X" + GameManager.instance.GemsCount);
    }
    private void OnAddToItemsList(object data) {
        var onData = (HeroController.OnItemListInfo) data;
        for (int i = 0; i < onData.itemsList.Count; i++) {
            for (int gunType = 0; gunType < allWeapons.Count; gunType++) {
                if (gunType == (int) onData.itemsList[i].GunType) {
                    allWeapons[gunType].gameObject.SetActive(true);
                    allWeapons[gunType].transform.position = allSlots[i].transform.position;
                }
            }
        }
        SettingSlotColor(onData.indexOfCurrentGun);
    }
    private void OnThrowAwayItems(object data) {
        var onData = (HeroController.OnItemListInfo) data;
        for (int gunType = 0; gunType < allWeapons.Count; gunType++) {
            allWeapons[gunType].gameObject.SetActive(false);
        }
        for (int i = 0; i < onData.itemsList.Count; i++) {
            for (int gunType = 0; gunType < allWeapons.Count; gunType++) {
                if (gunType == (int) onData.itemsList[i].GunType) {
                    allWeapons[gunType].gameObject.SetActive(true);
                    allWeapons[gunType].transform.position = allSlots[i].transform.position;
                }
            }
        }
        SettingSlotColor(onData.indexOfCurrentGun);
    }
    private void OnUpdateHealthBarUI(object data) {
        Health.HealthInfo info = (Health.HealthInfo) data;
        playerCurrentHealth = info.currentHealth;
        playerBasicHealth = info.basicHealthSet;
        isPlayerDead = info.isDead;
        isNeedToUpdateHealth = true;
    }
    private void OnUpdateReloadUI(object data) {
        HeroController.OnShootRaycastEvent onShoot = (HeroController.OnShootRaycastEvent) data;
        startFireTime = Time.time;
        fireRate = onShoot.currentGun.FireRate;
        isNeedToUpdateReload = true;
    }
    private void FixedUpdate() {
        FireRateUI();
        HealthBarUI();
    }
    private void FireRateUI() {
        if (!isNeedToUpdateReload) {
            return;
        }
        currentTime = Time.time - startFireTime;
        reloadSlide.value = currentTime / fireRate * 100;
        if (reloadSlide.value >= 100) {
            isNeedToUpdateReload = false;
        }
    }
    private void HealthBarUI() {
        if (!isNeedToUpdateHealth) return;

        playerHealthPercent = playerCurrentHealth / playerBasicHealth * 100;
        if (Mathf.Abs(heathSlider.value - playerHealthPercent) <= .65f) {
            isNeedToUpdateHealth = false;
        } else {
            if (heathSlider.value > playerHealthPercent) {
                heathSlider.value -= Time.fixedDeltaTime * 45;
            } else {
                heathSlider.value += Time.fixedDeltaTime * 45;
            }
        }
    }
    // Select items
    private void OnChangingSlotUI(object data) {
        var onData = (HeroController.OnItemListInfo) data;
        SettingSlotColor(onData.indexOfCurrentGun);
    }
    private void SettingSlotColor(int indexOfCurrentGun) {
        for (int i = 0; i < allSlots.Count; i++) {
            if (i == indexOfCurrentGun) {
                allSlots[indexOfCurrentGun].color = selectedColor;
            } else {
                allSlots[i].color = defaultColor;
            }
        }
    }
}
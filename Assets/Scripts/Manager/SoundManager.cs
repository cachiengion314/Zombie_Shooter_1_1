using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;
    [HideInInspector] public AudioSource[] audioSources;

    public AudioClip shootSfx;
    public AudioClip playerRunSfx;
    public AudioClip playerWalkSfx;
    public AudioClip hitSfx;
    public AudioClip hitSmokeSfx;
    public AudioClip gameOverSfx;
    public AudioClip eatSfx;
    public AudioClip pickGunSfx;
    public AudioClip pickGemSfx;
    public AudioClip deadSfx;
    public AudioClip changeItemsSfx;
    public AudioClip flashLightSfx;
    public AudioClip throwAwaySfx;

    private void Awake() {
        instance = this;
        audioSources = GetComponents<AudioSource>();
    }
    private void Start() {
        EventManager.Instance.ListenTo("GunsMesh_PlayerPickItems", OnGunsMesh_PlayerPickItemsSfx);

        EventManager.Instance.ListenTo("HeroController_PressShoot", OnShootingSfx);
        EventManager.Instance.ListenTo("HeroController_PlayerStanding", OnPlayerStandingSfx);
        EventManager.Instance.ListenTo("HeroController_PressChangingItem", OnPressChangeItemSfx);
        EventManager.Instance.ListenTo("HeroController_ThrowAwayWeapon", OnThrowAwayWeaponSfx);
        EventManager.Instance.ListenTo("HeroController_Walking", OnWalkingSfx);
        EventManager.Instance.ListenTo("HeroController_Running", OnRunningSfx);
        EventManager.Instance.ListenTo("HeroController_WayOutEvent_EndLevel", OnEndLevelSfx);
        EventManager.Instance.ListenTo("HeroController_PressFlashLight", OnHeroController_PressFlashLightSfx);

        EventManager.Instance.ListenTo("WayOutEvent_DenyPlayerEnterText", OnDenyPlayerEnterSfx);

        EventManager.Instance.ListenTo("CollidedEvent_EnterEvent", OnCollidedEvent_EnterEventSfx);
        EventManager.Instance.ListenTo("CollidedEvent_ExitEvent", OnCollidedEvent_ExitEventSfx);

        EventManager.Instance.ListenTo("Health_PlayerIsDead", OnHealth_PlayerIsDeadSfx);
        EventManager.Instance.ListenTo("Health_Hit", OnHealth_HitSfx);
        EventManager.Instance.ListenTo("Health_GainHealth", OnHealth_GainHealthSfx);

        EventManager.Instance.ListenTo("GameManager_AddGemsUI", OnGameManager_AddGemsUISfx);
        EventManager.Instance.ListenTo("GameManager_5GemsMission", OnGameManager_5GemsMissionSfx);
        EventManager.Instance.ListenTo("GameManager_6GemsMission", OnGameManager_6GemsMissionSfx);

        EventManager.Instance.ListenTo("BulletRaycast_HitSmokeSFx", OnBulletRaycast_HitSmokeSFx);

        EventManager.Instance.ListenTo("Health_TransformIsDead", OnHealth_TransformIsDeadSfx);
    }
    private void OnHeroController_PressFlashLightSfx(object data) {
        audioSources[0].pitch = 1f;
        Utility.PlaySoundFx(audioSources[0], flashLightSfx, .5f, false, true);
    }
    // Delay OnHealth_TransformIsDeadSfx

    private void OnHealth_TransformIsDeadSfx(object data) {
        Invoke("Health_TransformIsDeadDelay", .1f);
    }
    private void Health_TransformIsDeadDelay() {
        Utility.PlaySoundFx(audioSources[3], deadSfx, .1f, false, true);
    }
    // Delay OnBulletRaycast_HitSmokeSFx
    private void OnBulletRaycast_HitSmokeSFx(object data) {
        Invoke("OnBulletRaycast_HitSmokeSFxDelay", .3f);
    }
    private void OnBulletRaycast_HitSmokeSFxDelay() {

    }
    //
    private void OnGameManager_6GemsMissionSfx(object data) {

    }
    private void OnGameManager_5GemsMissionSfx(object data) {

    }
    private void OnGameManager_AddGemsUISfx(object data) {
        Utility.PlaySoundFx(audioSources[2], pickGemSfx, .5f, false, false);
    }
    private void OnHealth_GainHealthSfx(object data) {
        Utility.PlaySoundFx(audioSources[2], eatSfx, .5f, false, false);
    }
    // Delay OnHealth_HitSfx
    private bool isCritical;
    private void OnHealth_HitSfx(object data) {
        var info = (Health.DamageTextInfo) data;
        isCritical = info.isCritical;
        Invoke("OnHealth_HitSfxDelay", .2f);
    }
    private void OnHealth_HitSfxDelay() {
        if (isCritical) {
            Utility.PlaySoundFx(audioSources[2], hitSfx, .3f, false, false);
        } else {
            Utility.PlaySoundFx(audioSources[2], hitSfx, .2f, false, false);
        }
    }
    //
    private void OnHealth_PlayerIsDeadSfx(object data) {
        Utility.PlaySoundFx(audioSources[2], gameOverSfx, .5f, false, false);
    }
    private void OnCollidedEvent_ExitEventSfx(object data) {

    }
    private void OnCollidedEvent_EnterEventSfx(object data) {
        Utility.PlaySoundFx(audioSources[1], flashLightSfx, .5f, false, true);
    }
    private void OnDenyPlayerEnterSfx(object data) {
        Utility.PlaySoundFx(audioSources[1], gameOverSfx, .5f, false, true);
    }
    private void OnEndLevelSfx(object data) {
        Utility.PlaySoundFx(audioSources[1], gameOverSfx, .3f, false, true);
    }

    private void OnRunningSfx(object data) {
        Utility.PlaySoundFx(audioSources[1], playerWalkSfx, .3f, false, true);
    }

    private void OnWalkingSfx(object data) {
        Utility.PlaySoundFx(audioSources[1], playerWalkSfx, .1f, false, true);
    }
    private void OnThrowAwayWeaponSfx(object data) {
        audioSources[0].pitch = 1f;
        Utility.PlaySoundFx(audioSources[0], throwAwaySfx, .3f, false, false);
    }
    private void OnGunsMesh_PlayerPickItemsSfx(object data) {
        Utility.PlaySoundFx(audioSources[2], pickGunSfx, .5f, false, false);
    }
    private void OnPressChangeItemSfx(object data) {
        audioSources[0].pitch = 1f;
        Utility.PlaySoundFx(audioSources[0], changeItemsSfx, .3f, false);
    }
    private void OnShootingSfx(object data) {
        var onShoot = (HeroController.OnShootRaycastEvent) data;
        if (onShoot.currentGun.GunType.ToString() == "Knife") return;

        float damage = onShoot.currentGun.BulletDamage;
        float volume = damage / 20f;
        if (volume > 1f) {
            volume = 1f;
            audioSources[0].pitch = Random.Range(.7f, .9f);
        } else {
            audioSources[0].pitch = 1f;
            volume = .5f;
        }
        Utility.PlaySoundFx(audioSources[0], shootSfx, volume, false);
    }
    private void OnPlayerStandingSfx(object data) {
        audioSources[1].Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroController : MonoBehaviour {
    public class OnItemListInfo {
        public GunType gunType;
        public Vector3 playerPosition;
        public int indexOfCurrentGun;
        public List<Gun> itemsList;
    }
    public class OnShootRaycastEvent {
        public Gun currentGun;
        public Vector3 heroDirection;
        public Vector3 lineRendererPosition;
        public float shootRange;
        public LayerMask layerMaskRaycast;
    }
    public LayerMask layerMaskRaycast;
    [HideInInspector] public Vector3 moveVelocity;
    [HideInInspector] public float aimAngle;
    [HideInInspector] public float nextFire;
    [HideInInspector] public float shootRange;
    [HideInInspector] public float fireRate;

    [HideInInspector] public float slowdownFactor;
    [HideInInspector] public bool isFlashOn;
    [HideInInspector] public bool isShooted;
    [HideInInspector] public List<Gun> itemsList;
    public float heroMovingSpeed;
    public float heroRunningSpeed;
    public float heroSpinSpeed;

    private int indexOfCurrentGun;
    [HideInInspector] public HeroWeapon we;
    [HideInInspector] public HeroComponents he;
    [HideInInspector] public State state;

    [HideInInspector] public float intervalTime;
    [HideInInspector] public bool isNeedToUpdateBasicLight;
    [HideInInspector] public float intervalTimeRunAnimation;

    public enum State {
        Running,
        Moving,
    }
    private void OnEnable() {
        itemsList = new List<Gun>();
        he = GetComponent<HeroComponents>();
        we = GetComponent<HeroWeapon>();
    }
    private void Start() {
        indexOfCurrentGun = 0;
        he.currentGun = GameManager.instance.knife;
        we.BasicGunStatsUpdate(he.currentGun);
        we.LaserEffect(he.currentGun);
        we.BasicLight(he.currentGun);

        for (int i = 0; i < GameManager.instance.MAX_SLOTS; i++) {
            itemsList.Add(GameManager.instance.knife);

        }
        EventManager.Instance.ListenTo("GunsMesh_PlayerPickItems", OnPlayerPickItem);
    }
    private void Update() {
        switch (state) {
        case State.Moving:
            BasicControlAssign();
            break;
        case State.Running:
            BasicControlAssign();
            break;
        }
    }
    private void FixedUpdate() {
        switch (state) {
        case State.Moving:
            he.rigidbody2DCom.velocity = moveVelocity * heroMovingSpeed * slowdownFactor;
            transform.eulerAngles = new Vector3(0, 0, aimAngle);
            if (moveVelocity.sqrMagnitude < .1f) {
                EventManager.Instance.UpdateEvent("HeroController_PlayerStanding", null);
            }
            BasicControlImplement();
            OnUpdateWalkingFx();
            break;
        case State.Running:
            he.rigidbody2DCom.velocity = moveVelocity * heroRunningSpeed * slowdownFactor;
            transform.eulerAngles = new Vector3(0, 0, aimAngle);
            if (moveVelocity.sqrMagnitude < .1f) {
                EventManager.Instance.UpdateEvent("HeroController_PlayerStanding", null);
            }
            BasicControlImplement();
            OnUpdateRunningFx();
            break;
        }
    }
    private void BasicControlImplement() {
        we.LaserEffect(he.currentGun);
        intervalTime += Time.fixedDeltaTime;
        if (intervalTime >= .13f && isNeedToUpdateBasicLight) {
            we.BasicLight(he.currentGun);
            isNeedToUpdateBasicLight = false;
        }
        if (isShooted && Time.time >= nextFire) {
            nextFire = Time.time + fireRate;
            intervalTime = 0;
            isNeedToUpdateBasicLight = true;

            EventManager.Instance.UpdateEvent("HeroController_PressShoot", new OnShootRaycastEvent {
                currentGun = he.currentGun,
            });

            for (int i = 0; i < he.currentGun.NumberOfRay; i++) {
                EventManager.Instance.UpdateEvent("HeroController_Shoot", new OnShootRaycastEvent {
                    currentGun = he.currentGun,
                    heroDirection = he.directions[i],
                    lineRendererPosition = he.lineRenderersTransform[i].position,
                    shootRange = shootRange,
                    layerMaskRaycast = layerMaskRaycast,
                });
            }
            we.ShootingEffect(he.currentGun);
            isShooted = false;
        }
    }
    [HideInInspector] public float moveX;
    [HideInInspector] public float moveY;
    private void BasicControlAssign() {
        moveX = 0;
        moveY = 0;
        if (Input.GetKey(KeyCode.W) || VirtualBttCanvas.isUpPress) {
            moveY = +1 * he.Direction0.y;
            moveX = +1 * he.Direction0.x;
        }
        if (Input.GetKey(KeyCode.S) || VirtualBttCanvas.isDownPress) {
            moveY = -1 * he.Direction0.y;
            moveX = -1 * he.Direction0.x;
        }
        if (Input.GetKey(KeyCode.A) || VirtualBttCanvas.isLeftPress) {
            he.Direction0 = Quaternion.Euler(0, 0, +1 * heroSpinSpeed * Time.deltaTime) * he.Direction0;
        }
        if (Input.GetKey(KeyCode.D) || VirtualBttCanvas.isRightPress) {
            he.Direction0 = Quaternion.Euler(0, 0, -1 * heroSpinSpeed * Time.deltaTime) * he.Direction0;
        }
        // Assign value
        moveVelocity = new Vector3(moveX, moveY);
        aimAngle = Mathf.Atan2(he.Direction0.y, he.Direction0.x) * 180 / Mathf.PI;
        // Esc to quit the game
        if (Input.GetKeyDown(KeyCode.Escape) || VirtualBttCanvas.isEscapeClick) {
            VirtualBttCanvas.isEscapeClick = false;
            EventManager.Instance.UpdateEvent("HeroController_WayOutEvent_EndLevel",
                new PopupEventInfo {
                    title = "Why? :(",
                    content = "Are you really want to quit the level at this early?",
                    okCallback = () => { LoadScene.LoadSceneName(SceneName.MainMenu); },
                });
        }
        if (Input.GetKey(KeyCode.L) && he.currentGun.GunType == GunType.Rifts
            || Input.GetKey(KeyCode.L) && he.currentGun.GunType == GunType.AUG
            || Input.GetKey(KeyCode.L) && he.currentGun.GunType == GunType.DualBerretas
            || VirtualBttCanvas.isShootPress && he.currentGun.GunType == GunType.Rifts
            || VirtualBttCanvas.isShootPress && he.currentGun.GunType == GunType.AUG
            || VirtualBttCanvas.isShootPress && he.currentGun.GunType == GunType.DualBerretas) {

            isShooted = true;
        } else if (Input.GetKeyDown(KeyCode.L) || VirtualBttCanvas.isShootClick) {
            VirtualBttCanvas.isShootClick = false;

            isShooted = true;
        }
        if (Input.GetKeyDown(KeyCode.J) || VirtualBttCanvas.isChangeWeaponClick) {
            VirtualBttCanvas.isChangeWeaponClick = false;
            if (indexOfCurrentGun >= GameManager.instance.MAX_SLOTS - 1) {
                indexOfCurrentGun = -1;
            }
            indexOfCurrentGun++;
            he.currentGun = itemsList[indexOfCurrentGun];
            nextFire = Time.time;
            EventManager.Instance.UpdateEvent("HeroController_PressChangingItem",
                new OnItemListInfo {
                    indexOfCurrentGun = indexOfCurrentGun,
                    itemsList = itemsList
                });
        }
        if (Input.GetKeyDown(KeyCode.K) || VirtualBttCanvas.isFlashLightClick) {
            VirtualBttCanvas.isFlashLightClick = false;
            EventManager.Instance.UpdateEvent("HeroController_PressFlashLight", null);
            if (!isFlashOn) {
                isFlashOn = true;
                state = State.Moving;
                he.flashLightTransform.gameObject.SetActive(true);
            } else {
                isFlashOn = false;
                state = State.Running;
                he.flashLightTransform.gameObject.SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.I) || VirtualBttCanvas.isThrowClick) {
            VirtualBttCanvas.isThrowClick = false;
            Gun lastWeapon = he.currentGun;
            itemsList[indexOfCurrentGun] = GameManager.instance.knife;
            he.currentGun = itemsList[indexOfCurrentGun];

            EventManager.Instance.UpdateEvent("HeroController_ThrowAwayWeapon",
               new OnItemListInfo {
                   gunType = lastWeapon.GunType,
                   playerPosition = transform.position,
                   indexOfCurrentGun = indexOfCurrentGun,
                   itemsList = itemsList
               });

        }
    }
    private void OnPlayerPickItem(object data) {
        GunType unknowGunType = (GunType) data;
        int gunType = (int) unknowGunType;

        for (int index = 0; index < itemsList.Count; index++) {
            if (itemsList[index].GunType.ToString() == "Knife") {
                he.currentGun = GameManager.instance.allWeaponsPossible[gunType];
                itemsList[index] = GameManager.instance.allWeaponsPossible[gunType];
                indexOfCurrentGun = index;

                EventManager.Instance.UpdateEvent("HeroController_AddItem",
                    new OnItemListInfo {
                        indexOfCurrentGun = indexOfCurrentGun,
                        itemsList = itemsList
                    });
                break;
            }
        }
    }
    private void OnUpdateWalkingFx() {
        intervalTimeRunAnimation += Time.deltaTime;
        if (intervalTimeRunAnimation <= .5f) {
            return;
        }
        intervalTimeRunAnimation -= .5f;

        EventManager.Instance.UpdateEvent("HeroController_Walking", null);
    }
    private void OnUpdateRunningFx() {
        intervalTimeRunAnimation += Time.deltaTime;
        if (intervalTimeRunAnimation <= .5f) {
            return;
        }
        intervalTimeRunAnimation -= .5f;

        EventManager.Instance.UpdateEvent("HeroController_Running", new OnShootRaycastEvent {
            heroDirection = moveVelocity,
            lineRendererPosition = transform.position,
        });
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [HideInInspector] public static GameManager instance;
    [HideInInspector] public InitGameManager init;
    [HideInInspector] public HeroController hc;
    private int enemiesKilled;
    public int EnemiesKilled {
        get { return enemiesKilled; }
        set {
            enemiesKilled = value;
            EventManager.Instance.UpdateEvent("GameManager_EnemiesKilled", null);
        }
    }
    public Transform playerPrefab;
    private readonly Vector3 playerOriginPosition = new Vector3(0, -37, 0);
    // Player Weapons 
    [HideInInspector] public List<Gun> allWeaponsPossible;
    [HideInInspector] public Pistol pistol;
    [HideInInspector] public Rifts rifts;
    [HideInInspector] public ShotGun shotGun;
    [HideInInspector] public Sniper sniper;
    [HideInInspector] public AUG aug;
    [HideInInspector] public DualBerretas dualBerretas;
    [HideInInspector] public Knife knife;
    // Player stats
    private int gemCount;
    [HideInInspector]
    public int GemsCount {
        get { return gemCount; }
        set {
            gemCount = value;
            EventManager.Instance.UpdateEvent("GameManager_AddGemsUI", null);
            if (gemCount == 5) {
                // GateKeeper, CollidedEvent
                EventManager.Instance.UpdateEvent("GameManager_5GemsMission", null);

            } else if (gemCount >= 6) { // TutorialText, WayOutCollider
                EventManager.Instance.UnSubscribe("GameManager_5GemsMission", null, true);
                EventManager.Instance.UpdateEvent("GameManager_6GemsMission",
                    new TextEventInfo {
                        position = playerTransform.position,
                        text = "Congratulations! You win this level! Please go back to our base \n Be careful, something dangerous is waiting",
                    });
            }
        }
    }
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public readonly int MAX_SLOTS = 3;
    [HideInInspector]
    public int WeaponsPlayerCount {
        get {
            int count = 0;
            foreach (Gun gun in hc.itemsList) {
                if (gun.GunType.ToString() != "Knife") {
                    count++;
                }
            }
            return count;
        }
    }
    private void Awake() {
        if (instance) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        Physics2D.gravity = Vector2.zero;
        init = GetComponent<InitGameManager>();

        pistol = new Pistol();
        rifts = new Rifts();
        shotGun = new ShotGun();
        sniper = new Sniper();
        aug = new AUG();
        dualBerretas = new DualBerretas();
        knife = new Knife();
        allWeaponsPossible = new List<Gun> {
            pistol,
            shotGun,
            rifts,
            sniper,
            aug,
            dualBerretas,
            knife
        };
        Quaternion quaternion = new Quaternion { eulerAngles = new Vector3(0, 0, 90) };
        playerTransform = Instantiate(playerPrefab, playerOriginPosition, quaternion);
        hc = playerTransform.GetComponent<HeroController>();
    }
    private void Start() {
        EventManager.Instance.ListenTo("GameManager_6GemsMission", OnPlayerGain6Gems);

        SpawnThings("ZombieMelee", init.data.amountOfMelee, init.data.zombieMeleePrefab,
         init.data.zombieMeleePooledList, init.data.tempMeleeVectorList, init.data.originMeleeVectorList);

        ResetRandomVectorsList(init.data. tempMeleeVectorList, init.data.originMeleeVectorList);

        SpawnThings("ZombieMeleeAdvance", init.data.amountOfMeleeAdvance, init. data.zombieMeleeAdvancePrefab,
         init.data.zombieMeleeAdvancePooledList, init.data.tempMeleeAdvanceVectorList, init.data.originMeleeAdvanceVectorList);

        ResetRandomVectorsList(init. data.tempMeleeAdvanceVectorList, init.data.originMeleeAdvanceVectorList);

        SpawnThings("PowerUpHeal", init.data.amountOfPowerUpHeal, init.data .healPowerUpPrefab, init.data. powerUpPooledList,
         init.data. tempPowerUpPosList, init.data.originPowerUpPosList, false);

        ReActivateAll();
    }
    // ReSpawn boss
    private void OnPlayerGain6Gems(object data) {
        init.data.zombieBossTransform.gameObject.SetActive(true);
        init.data.zombieBossTransform.GetComponent<Health>().ResetToOriginState();
        init.data.zombieBossTransform.position = init.data.zombieBossPos[1];
        init.data.zombieBossTransform.rotation = new Quaternion { eulerAngles = new Vector3(0, 0, 90) };
    }
    // Spawn logic
    private Vector3 GetRandomPosition(List<Vector3> tempListVectors, List<Vector3> originListVectors) {
        if (tempListVectors.Count < 1) {
            ResetRandomVectorsList(tempListVectors, originListVectors);
        }
        int randomIndex = Random.Range(0, tempListVectors.Count);
        Vector3 randomPos = tempListVectors[randomIndex];
        tempListVectors.RemoveAt(randomIndex);
        return randomPos;
    }
    private void SpawnThings(string typeName, int amount, Transform prefabs, List<Transform> transformPooledList,
        List<Vector3> tempListVectors, List<Vector3> originListVectors, bool isNeedRandomRotate = true) {
        Transform thingTransform = new GameObject(typeName).transform;
        for (int i = 0; i < amount; i++) {
            Quaternion randomQuaternion = new Quaternion {
                eulerAngles = new Vector3(0, 0, Random.Range(0, 360))
            };
            if (!isNeedRandomRotate) {
                randomQuaternion = Quaternion.identity;
            }
            Transform newThing = Instantiate(
                prefabs, GetRandomPosition(tempListVectors, originListVectors), randomQuaternion);
            transformPooledList.Add(newThing);
            newThing.SetParent(thingTransform);
        }
    }
    //
    // ReActivateZombie logic
    private void ReActivateAll() {
        InvokeRepeating("ReActivateAllZombie", 1, 15);
        InvokeRepeating("ReActivePowerUp", 1, 15);
    }
    private void ReActivePowerUp() {
        Transform heal = GetPooled(init.data.powerUpPooledList);
        if (heal) {
            heal.GetComponent<PowerUpMesh>().ReActivate();
            heal.position = GetRandomPosition(init.data.tempPowerUpPosList, init.data.originPowerUpPosList) + GunsMesh.GetRandomDirection(0);
        }
    }
    private void ReActivateAllZombie() {
        Transform zombieMelee = GetPooled(init.data.zombieMeleePooledList);
        if (zombieMelee) {
            zombieMelee.GetComponent<Health>().ResetToOriginState();
            zombieMelee.position = GetRandomPosition(init.data.tempMeleeVectorList, init.data.originMeleeVectorList);
        }
        Transform zombieMeleeAdvance = GetPooled(init.data.zombieMeleeAdvancePooledList);
        if (zombieMeleeAdvance) {
            zombieMeleeAdvance.GetComponent<Health>().ResetToOriginState();
            zombieMeleeAdvance.position = GetRandomPosition(init.data.tempMeleeAdvanceVectorList, init.data.originMeleeAdvanceVectorList);
        }
    }
    private Transform GetPooled(List<Transform> thingList) {
        foreach (Transform thing in thingList) {
            if (!thing.gameObject.activeInHierarchy) {
                return thing;
            }
        }
        return null;
    }
    private void ResetRandomVectorsList(List<Vector3> tempListVectors, List<Vector3> originListVectors) {
        tempListVectors.Clear();
        tempListVectors.AddRange(originListVectors);
    }
}
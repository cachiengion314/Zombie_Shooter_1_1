using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatas", menuName = "ConfigData/Datas", order = 2)]
public class LevelData : ScriptableObject {
    // Prefabs
    public Transform[] lightObjectPrefab;
    public Transform[] gunsPrefab;
    public Transform gunsPosTransformPrefab;
    public Transform powerUpPosPrefab;
    public Transform lightObjectPosTransformPrefab;
    public Transform meshParticlePrefab;
    public Transform zombieMeleePosPrefab;
    public Transform zombieMeleeAdvancePosPrefab;
    public Transform gridLevelPrefab;
    public Transform zombieBossPrefab;
    public Transform zombieMeleePrefab;
    public Transform zombieMeleeAdvancePrefab;
    public Transform healPowerUpPrefab;
    // gun
    [HideInInspector] public List<Vector3> gunsPosOriginVectorList;
    [HideInInspector] public Transform gunsPosTransform;
    // lightObject
    [HideInInspector] public List<Vector3> lightObjectOriginVectorList;
    [HideInInspector] public Transform lightObjectPosTransform;
    // GridLevel
    [HideInInspector] public Transform gridLevelTransform;
    // meshParticle
    [HideInInspector] public Transform meshParticleTransform;
    // zombieMelee
    [HideInInspector] public Transform zombieMeleePosTransform;
    // zombieMeleeAdvance
    [HideInInspector] public Transform zombieMeleeAdvancePosTransform;
    [HideInInspector] public List<Vector3> tempMeleeAdvanceVectorList;
    [HideInInspector] public List<Vector3> originMeleeAdvanceVectorList;
    [HideInInspector] public List<Transform> zombieMeleeAdvancePooledList;
    [HideInInspector] public int amountOfMeleeAdvance;
    public int amountOfMeleeAdvanceSet;
    // PowerUp 
    [HideInInspector] public List<Vector3> originPowerUpPosList;
    [HideInInspector] public List<Vector3> tempPowerUpPosList;
    [HideInInspector] public Transform powerUpPosTransform;
    [HideInInspector] public List<Transform> powerUpPooledList;
    [HideInInspector] public int amountOfPowerUpHeal;
    public int amountOfPowerUpHealSet;
    // Zombie Boss
    public Vector3[] zombieBossPos;
    [HideInInspector] public Transform zombieBossTransform;
    // zombieMelee
    [HideInInspector] public List<Transform> zombieMeleePooledList;
    [HideInInspector] public List<Vector3> originMeleeVectorList;
    [HideInInspector] public List<Vector3> tempMeleeVectorList;
    [HideInInspector] public int amountOfMelee;
    public int amountOfMeleeSet;
}

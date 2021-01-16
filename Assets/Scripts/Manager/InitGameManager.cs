using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGameManager : MonoBehaviour {
    [SerializeField] public LevelData data;

    private void Awake() {
        data.gridLevelTransform = Instantiate(data.gridLevelPrefab, Vector3.zero, Quaternion.identity);
        data.meshParticleTransform = Instantiate(data.meshParticlePrefab, Vector3.zero, Quaternion.identity);
        data.zombieMeleePosTransform = Instantiate(data.zombieMeleePosPrefab, Vector3.zero, Quaternion.identity);
        data.zombieMeleeAdvancePosTransform = Instantiate(data.zombieMeleeAdvancePosPrefab, Vector3.zero, Quaternion.identity);
        data.powerUpPosTransform = Instantiate(data.powerUpPosPrefab, Vector3.zero, Quaternion.identity);
        // init guns
        data.gunsPosTransform = Instantiate(data.gunsPosTransformPrefab, Vector3.zero, Quaternion.identity);
        foreach (Transform child in data.gunsPosTransform) {
            data.gunsPosOriginVectorList.Add(child.position);
        }
        for (int i = 0; i < data.gunsPrefab.Length; i++) {
            Instantiate(data.gunsPrefab[i], data.gunsPosOriginVectorList[i], Quaternion.identity);
        }
        // init lightGameObject
        data.lightObjectPosTransform = Instantiate(data.lightObjectPosTransformPrefab, Vector3.zero, Quaternion.identity);
        foreach (Transform child in data.lightObjectPosTransform) {
            data.lightObjectOriginVectorList.Add(child.position);
        }
        for (int i = 0; i < data.lightObjectPrefab.Length; i++) {
            Instantiate(data.lightObjectPrefab[i], data.lightObjectOriginVectorList[i], Quaternion.identity);
        }
        // zombie boss
        Quaternion quaternion = new Quaternion { eulerAngles = new Vector3(0, 0, -90) };
        data.zombieBossTransform = Instantiate(data.zombieBossPrefab, data.zombieBossPos[0], quaternion);

        data.zombieMeleeAdvancePooledList = new List<Transform>();
        data.zombieMeleePooledList = new List<Transform>();
        data.powerUpPooledList = new List<Transform>();
        data.originPowerUpPosList = new List<Vector3>();
        data.tempPowerUpPosList = new List<Vector3>();
        data.originMeleeVectorList = new List<Vector3>();
        data.tempMeleeVectorList = new List<Vector3>();
        data.tempMeleeAdvanceVectorList = new List<Vector3>();
        data.originMeleeAdvanceVectorList = new List<Vector3>();

        foreach (Transform child in data.powerUpPosTransform) {
            data.originPowerUpPosList.Add(child.position);
            data.tempPowerUpPosList.Add(child.position);
        }
        foreach (Transform child in data.zombieMeleePosTransform) {
            data.originMeleeVectorList.Add(child.position);
            data.tempMeleeVectorList.Add(child.position);
        }
        foreach (Transform child in data.zombieMeleeAdvancePosTransform) {
            data.originMeleeAdvanceVectorList.Add(child.position);
            data.tempMeleeAdvanceVectorList.Add(child.position);
        }
        if (data.amountOfMeleeSet <= data.originMeleeVectorList.Count
            || data.amountOfMeleeAdvanceSet <= data.originMeleeAdvanceVectorList.Count
            || data.amountOfPowerUpHealSet <= data.originPowerUpPosList.Count) {

            data.amountOfMelee = data.amountOfMeleeSet;
            data.amountOfMeleeAdvance = data.amountOfMeleeAdvanceSet;
            data.amountOfPowerUpHeal = data.amountOfPowerUpHealSet;

        } else {
            data.amountOfMelee = data.originMeleeVectorList.Count;
            data.amountOfMeleeAdvance = data.originMeleeAdvanceVectorList.Count;
            data.amountOfPowerUpHeal = data.originPowerUpPosList.Count;
        }
    }
}

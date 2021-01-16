using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDirt {
    public bool isRunning;
    private float intervalTime;
    public Vector3 position;
    public Vector3 direction;
    public float randomSpeed;
    public bool isMoveDone;
    public MeshParticle meshParticle;
    public int uvIndex;
    public int UvIndex {
        get { return uvIndex; }
        set {
            uvIndex = value;
            if (uvIndex > 6) {
                uvIndex = 0;
            }
        }
    }
    public int quadIndex;

    public void Update() {
        intervalTime += Time.deltaTime;
        if (intervalTime <= .1f) {
            return;
        }
        intervalTime -= .1f;
        uvIndex++;
        position += direction * Time.deltaTime * randomSpeed;
        randomSpeed -= Time.deltaTime * 3;
        if (randomSpeed <= 0 || UvIndex >= 6 || !isRunning) {
            uvIndex = 6;
            isMoveDone = true;
        }
        meshParticle.UpdateQuad(quadIndex, meshParticle.quadSize, position, 0, UvIndex);
    }
}
public class Dirts : MonoBehaviour {
    private MeshParticle meshParticle;
    private List<SingleDirt> dirtList;
    private float moveVelocityValue;
    private bool isRunning;
    private float randomSpeed;

    private void Awake() {
        meshParticle = GetComponent<MeshParticle>();
        dirtList = new List<SingleDirt>();

    }
    private void Start() {
        EventManager.Instance.ListenTo("HeroController_Running", OnRunning);
    }
    private void OnRunning(object data) {
        HeroController.OnShootRaycastEvent onShoot = (HeroController.OnShootRaycastEvent) data;
        moveVelocityValue = onShoot.heroDirection.sqrMagnitude;
        if (moveVelocityValue < .5f) {
            isRunning = false;
        } else {
            isRunning = true;
        }
        randomSpeed = Random.Range(2.7f, 3f);

        int quadIndex = meshParticle.AddPooledQuad(onShoot.lineRendererPosition, 0);
        SingleDirt dirt = new SingleDirt {
            randomSpeed = randomSpeed,
            isRunning = isRunning,
            position = onShoot.lineRendererPosition,
            direction = -onShoot.heroDirection,
            quadIndex = quadIndex,
            meshParticle = meshParticle,
        };
        dirtList.Add(dirt);
    }
    private void Update() {
        for (int i = 0; i < dirtList.Count; i++) {
            dirtList[i].Update();
            if (dirtList[i].isMoveDone) {
                dirtList.RemoveAt(i);
            }
        }
    }
    private void LateUpdate() {
        if (dirtList.Count > 0)
            meshParticle.ImplementMeshValue();
    }
}

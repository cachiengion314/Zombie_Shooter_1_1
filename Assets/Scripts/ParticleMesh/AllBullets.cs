using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBullet {
    public float randomRotateSpeed;
    private float rotation;
    public Vector3 position;
    public Vector3 direction;
    public float randomSpeed;
    public bool isMoveDone;
    public MeshParticle meshParticle;

    public int index;

    public void Update() {
        if (randomSpeed <= 0) {
            isMoveDone = true;
        }
        position += direction * Time.deltaTime * randomSpeed;
        rotation += randomRotateSpeed;
        randomSpeed -= Time.deltaTime * 3;

        meshParticle.UpdateQuad(index, meshParticle.quadSize, position, rotation);
    }
}
public class AllBullets : MonoBehaviour {
    private MeshParticle meshParticle;
    private List<SingleBullet> bulletsList;

    private float randomSpeed;

    private int randomZ;
    private int randomNegativeZ;
    private int randomIndexZArray;
    private int[] randomZValueArray;

    private int randomRotateSpeed;
    private void Awake() {
        meshParticle = GetComponent<MeshParticle>();
        bulletsList = new List<SingleBullet>();
        randomZValueArray = new int[] {
            randomZ,
            randomNegativeZ,
        };
    }
    private void Start() {
        EventManager.Instance.ListenTo("HeroController_Shoot", OnShootBulletsParticle);
    }
    private void OnShootBulletsParticle(object data) {
        HeroController.OnShootRaycastEvent onShoot = (HeroController.OnShootRaycastEvent) data;
        if (onShoot.currentGun.GunType.ToString().Equals("Knife")) { return; }

        randomNegativeZ = Random.Range(-70, -110);
        randomZ = Random.Range(+70, +110);
        randomZValueArray[0] = randomZ;
        randomZValueArray[1] = randomNegativeZ;
        randomIndexZArray = Random.Range(0, 2);

        randomRotateSpeed = Random.Range(-5, 5);
        randomSpeed = Random.Range(2.3f, 3.2f);

        int quadIndex = meshParticle.AddPooledQuad(onShoot.lineRendererPosition, 0);
        SingleBullet bullet = new SingleBullet {
            randomSpeed = randomSpeed,
            randomRotateSpeed = randomRotateSpeed,
            position = onShoot.lineRendererPosition,
            direction = Quaternion.Euler(
                0, 0, randomZValueArray[randomIndexZArray]) * onShoot.heroDirection,
            index = quadIndex,
            meshParticle = meshParticle,
        };
        bulletsList.Add(bullet);
    }
    private void Update() {
        for (int i = 0; i < bulletsList.Count; i++) {
            bulletsList[i].Update();
            if (bulletsList[i].isMoveDone) {
                bulletsList.RemoveAt(i);
            }
        }
    }
    private void LateUpdate() {
        if (bulletsList.Count > 0)
            meshParticle.ImplementMeshValue();
    }
}
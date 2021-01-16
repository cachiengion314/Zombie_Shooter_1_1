using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsMesh : MonoBehaviour {
    public GunType gunType;
    [System.Serializable]
    public struct UvPixels {
        public Vector2Int uv00Pixel;
        public Vector2Int uv11Pixel;
    }
    private struct UvNormalizeCoordinate {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private UvPixels[] uvPixels;
    private UvNormalizeCoordinate[] uvNormalizeCoordinates;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangle;
    private int quadIndex; // represent a single mesh index

    private int vIndex;
    private int vIndex0;
    private int vIndex1;
    private int vIndex2;
    private int vIndex3;

    private MeshFilter itemMeshFilter;
    private MeshRenderer itemMeshRenderer;
    private Vector3 meshPosition;
    private float intervalTimeOrigin = 3f;
    private float intervalTime;
    private float animationSpeed = .7f;
    private Vector3 quadSize = new Vector3(.35f, .35f);
    private bool isDestroyed;

    private void Awake() {
        itemMeshFilter = GetComponent<MeshFilter>();
        itemMeshRenderer = GetComponent<MeshRenderer>();

        itemMeshRenderer.sortingLayerID = SortingLayer.NameToID("Player");
        itemMeshRenderer.sortingOrder = 20;
        intervalTime = intervalTimeOrigin;
        meshPosition = Vector3.zero;

        AssignBasicQuadMeshValue();
        UpdateQuad(meshPosition, 0, quadSize);
        ImplementQuadMeshValue();
    }
    private void Start() {
        EventManager.Instance.ListenTo("HeroController_ThrowAwayWeapon", OnThrowAwayWeapon);
    }
    private void OnThrowAwayWeapon(object data) {
        var onData = (HeroController.OnItemListInfo) data;
        if ((int) onData.gunType == (int) gunType) {
            isDestroyed = false;
            gameObject.SetActive(true);
            Vector3 position = new Vector3(onData.playerPosition.x, onData.playerPosition.y, 0);
            transform.position = position + GetRandomDirection();
        }
    }
    private void FixedUpdate() {
        if (isDestroyed) {
            transform.gameObject.SetActive(false);
        }
        //if (intervalTime > intervalTimeOrigin / 2f) {
        //    meshPosition += Vector3.up * Time.fixedDeltaTime * animationSpeed;
        //    quadSize += Vector3.up * Time.fixedDeltaTime * animationSpeed;
        //} else if (intervalTime <= intervalTimeOrigin / 2f && intervalTime > 0) {
        //    meshPosition += Vector3.down * Time.fixedDeltaTime * animationSpeed;
        //    quadSize += Vector3.down * Time.fixedDeltaTime * animationSpeed;
        //} else {
        //    intervalTime = intervalTimeOrigin;
        //}
        //intervalTime -= Time.fixedDeltaTime * 3f;

        //UpdateQuad(meshPosition, 0, quadSize);
    }
    private void LateUpdate() {
        ImplementQuadMeshValue();
    }
    private void AssignBasicQuadMeshValue() {
        mesh = new Mesh();

        vertices = new Vector3[4];
        uv = new Vector2[4];
        triangle = new int[6];

        Material material = itemMeshRenderer.material;

        int textureWidth = material.mainTexture.width;
        int textureHeight = material.mainTexture.height;

        List<UvNormalizeCoordinate> uvNormalizeCoordinates = new List<UvNormalizeCoordinate>();
        foreach (UvPixels uvPixel in uvPixels) {
            UvNormalizeCoordinate uvCoordinate = new UvNormalizeCoordinate {
                uv00 = new Vector2(
                    (float) uvPixel.uv00Pixel.x / textureWidth,
                    (float) uvPixel.uv00Pixel.y / textureHeight),
                uv11 = new Vector2(
                     (float) uvPixel.uv11Pixel.x / textureWidth,
                    (float) uvPixel.uv11Pixel.y / textureHeight)
            };
            uvNormalizeCoordinates.Add(uvCoordinate);
        }
        this.uvNormalizeCoordinates = uvNormalizeCoordinates.ToArray();
    }
    private void UpdateQuad(Vector3 meshPositino, float rotation, Vector3 quadSize) {
        vIndex = quadIndex * 4;
        vIndex0 = vIndex;
        vIndex1 = vIndex + 1;
        vIndex2 = vIndex + 2;
        vIndex3 = vIndex + 3;

        vertices[vIndex0] = meshPositino + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, -quadSize.y);
        vertices[vIndex1] = meshPositino + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, +quadSize.y);
        vertices[vIndex2] = meshPositino + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, +quadSize.y);
        vertices[vIndex3] = meshPositino + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, -quadSize.y);

        uv[vIndex0] = new Vector2(0, 0);
        uv[vIndex1] = new Vector2(0, 1);
        uv[vIndex2] = new Vector2(1, 1);
        uv[vIndex3] = new Vector2(1, 0);

        int tIndex = quadIndex * 6;
        triangle[tIndex + 0] = vIndex0;
        triangle[tIndex + 1] = vIndex1;
        triangle[tIndex + 2] = vIndex2;
        triangle[tIndex + 3] = vIndex0;
        triangle[tIndex + 4] = vIndex2;
        triangle[tIndex + 5] = vIndex3;
    }

    private void ImplementQuadMeshValue() {
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangle;
        itemMeshFilter.mesh = mesh;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")
            && GameManager.instance.WeaponsPlayerCount < GameManager.instance.MAX_SLOTS
             && !isDestroyed) {

            isDestroyed = true;
            EventManager.Instance.UpdateEvent("GunsMesh_PlayerPickItems", gunType);
        }
    }
    public static Vector3 GetRandomDirection(float positionZ = -1) {
        float randomX = Random.Range(-10, 10);
        float randomY = Random.Range(-10, 10);
        Vector3 pos = new Vector3(randomX, randomY).normalized;
        Vector3 pos2 = pos * 1.7f;
        Vector3 pos3 = new Vector3(pos2.x, pos2.y, positionZ);
        return pos3;
    }
}
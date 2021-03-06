﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PowerUpType {
    Drumsitck, Heart
}
public class PowerUpMesh : MonoBehaviour {
    [System.Serializable]
    public struct Pixels {
        public Vector2 pixel00;
        public Vector2 pixel11;
    }
    private struct Uv {
        public Vector2 uv00;
        public Vector2 uv11;
    }
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangle;
    private MeshFilter meshFilterOfVisualObject;
    private MeshRenderer meshRenderer;
    private Material material;

    [SerializeField] int sortingLayerSet;
    [SerializeField] private Pixels[] pixelsArray;
    private Uv[] uvCoordsArray;

    [SerializeField] private int MaxPooledSize;
    private int pooledIndex;

    private int quadIndex;
    private int vIndex0;
    private int vIndex1;
    private int vIndex2;
    private int vIndex3;
    public Vector2 quadSize;

    public event Action OnReActivate; 

    private void Awake() {
        pooledIndex = -1;
        meshFilterOfVisualObject = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;

        meshRenderer.sortingLayerID = SortingLayer.NameToID("Player");
        meshRenderer.sortingOrder = sortingLayerSet;

        AssignBasicMeshValue();
        UpdateQuad(0, quadSize, Vector3.zero, 0); 
        ImplementMeshValue();
    }
    public void AssignBasicMeshValue() {
        mesh = new Mesh();
        vertices = new Vector3[4 * 10000];
        uv = new Vector2[4 * 10000];
        triangle = new int[6 * 10000];

        int width = material.mainTexture.width;
        int height = material.mainTexture.height;

        List<Uv> uvBulletList = new List<Uv>();
        foreach (Pixels pixels in pixelsArray) {
            Uv uvBulletCoords = new Uv {
                uv00 = new Vector2(pixels.pixel00.x / width, pixels.pixel00.y / height),
                uv11 = new Vector2(pixels.pixel11.x / width, pixels.pixel11.y / height)
            };
            uvBulletList.Add(uvBulletCoords);
        }
        uvCoordsArray = uvBulletList.ToArray();
    }
    public int AddPooledQuad(Vector3 position, float rotation) {
        GetPooledIndex();
        UpdateQuad(pooledIndex, quadSize, position, rotation);

        return pooledIndex;
    }
    public int GetPooledIndex() {
        if (pooledIndex >= MaxPooledSize - 1) {
            pooledIndex = -1;
        }
        return pooledIndex++;
    }
    public void UpdateQuad(int quadIndex, Vector3 quadSize, Vector3 position, float rotation, int uvIndex = 0) {
        vIndex3 = (quadIndex + 1) * 4 - 1;
        vIndex2 = (quadIndex + 1) * 4 - 2;
        vIndex1 = (quadIndex + 1) * 4 - 3;
        vIndex0 = (quadIndex + 1) * 4 - 4;

        vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, -quadSize.y);
        vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, +quadSize.y);
        vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, +quadSize.y);
        vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, -quadSize.y);

        uv[vIndex0] = uvCoordsArray[uvIndex].uv00;
        uv[vIndex1] = new Vector2(uvCoordsArray[uvIndex].uv00.x, uvCoordsArray[uvIndex].uv11.y);
        uv[vIndex2] = uvCoordsArray[uvIndex].uv11;
        uv[vIndex3] = new Vector2(uvCoordsArray[uvIndex].uv11.x, uvCoordsArray[uvIndex].uv00.y);

        triangle[(quadIndex + 1) * 6 - 6] = vIndex0;
        triangle[(quadIndex + 1) * 6 - 5] = vIndex1;
        triangle[(quadIndex + 1) * 6 - 4] = vIndex2;

        triangle[(quadIndex + 1) * 6 - 3] = vIndex0;
        triangle[(quadIndex + 1) * 6 - 2] = vIndex2;
        triangle[(quadIndex + 1) * 6 - 1] = vIndex3;
    }
    public void ImplementMeshValue() {
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangle;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
        meshFilterOfVisualObject.mesh = mesh;
    }
    public void ReActivate() {
        OnReActivate?.Invoke(); 
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public Vector3 worldPosition;
    public Vector2Int gridPosition;
    public bool walkable;
    public Node(Vector3 worldPosition, Vector2Int gridPosition) {
        this.worldPosition = worldPosition;
        this.gridPosition = gridPosition;
    }
    public int hCost;
    public int gCost;
    public Node parrent;
    public int FCost {
        get {
            return hCost + gCost;
        }
    }
}
public class InitGridWorld : MonoBehaviour {
    [HideInInspector] public static InitGridWorld ins;
    [HideInInspector] public Node[,] nodes;
    public LayerMask obstaclesLayer;
    public Vector2 gridWorld;
    private int gridSizeX;
    private int gridSizeY;
    private float NodeDiameter {
        get {
            return nodeRadius * 2;
        }
    }
    public float nodeRadius;
    private Vector3 worldPosition00;

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(Vector2.zero, new Vector3(gridWorld.x, gridWorld.y, 0));
        if (nodes != null) {
            foreach (Node node in nodes) {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (2 * nodeRadius - .05f));
            }
        }
    }
    private void Awake() {
        ins = this;
        worldPosition00 = transform.position + Vector3.left * gridWorld.x / 2 + Vector3.down * gridWorld.y / 2;
        gridSizeX = Mathf.RoundToInt(gridWorld.x / NodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorld.y / NodeDiameter);
    }
    private void Start() {
        InstantiatedGridWorld();
    }
    public Vector2Int FromWorldToGridPosition(Vector3 worldPosition) {
        Vector3 offsetDirection = -worldPosition00;
        Vector3 offsetPosition = worldPosition + offsetDirection;

        int x = Mathf.FloorToInt(offsetPosition.x / NodeDiameter);
        int y = Mathf.FloorToInt(offsetPosition.y / NodeDiameter);
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        return new Vector2Int(x, y);
    }
    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbourNodes = new List<Node>();
        for (int x = -1; x < 2; x++) {
            for (int y = -1; y < 2; y++) {
                if (x == 0 && y == 0) {
                    continue;
                }
                Vector2Int neighbourGridPos = node.gridPosition + new Vector2Int(x, y);
                Node neighbourNode = InitGridWorld.ins.nodes[neighbourGridPos.x, neighbourGridPos.y];
                if (neighbourGridPos.x >= 0 || neighbourGridPos.y >= 0
                    || neighbourGridPos.x < gridSizeX || neighbourGridPos.y < gridSizeY) {

                    neighbourNodes.Add(neighbourNode);
                }
            }
        }


        return neighbourNodes;
    }
    private void InstantiatedGridWorld() {
        nodes = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {

                nodes[x, y] = new Node(
                    worldPosition00 + Vector3.right * (2 * x + 1) * nodeRadius + Vector3.up * (2 * y + 1) * nodeRadius,
                    new Vector2Int(x, y));
                Collider2D col2D = Physics2D.OverlapCircle(nodes[x, y].worldPosition, nodeRadius, obstaclesLayer);
                if (!col2D) {
                    nodes[x, y].walkable = true;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour {


    public void PathFindingAlgorithm(Vector3 startWorldPos, Vector3 targetWorldPos) {
        List<Node> openNodes = new List<Node>();
        HashSet<Node> closeNodes = new HashSet<Node>();

        Vector2Int startGridPos = InitGridWorld.ins.FromWorldToGridPosition(startWorldPos);
        Node startNode = InitGridWorld.ins.nodes[startGridPos.x, startGridPos.y];

        Vector2Int targetGridPos = InitGridWorld.ins.FromWorldToGridPosition(targetWorldPos);
        Node targetNode = InitGridWorld.ins.nodes[targetGridPos.x, targetGridPos.y];

        openNodes.Add(startNode);

        while (openNodes.Count > 0) {
            Node currentNode = openNodes[0];
            for (int i = 0; i < openNodes.Count; i++) {
                if (currentNode.FCost > openNodes[i].FCost
                    || currentNode.FCost == openNodes[i].FCost && currentNode.hCost > openNodes[i].hCost) {

                    currentNode = openNodes[i];
                }
            }
            if (currentNode == targetNode) {
                ReTracePath(startNode, targetNode);
                return;
            }
            openNodes.Remove(currentNode);
            closeNodes.Add(currentNode);

            List<Node> neighbourNodes = InitGridWorld.ins.GetNeighbours(currentNode);
            for (int i = 0; i < neighbourNodes.Count; i++) {
                if (!neighbourNodes[i].walkable || closeNodes.Contains(neighbourNodes[i])) {
                    continue;
                }
                int newGCostToNeighbour = currentNode.gCost + GetDistane(currentNode, neighbourNodes[i]);
                if(newGCostToNeighbour < neighbourNodes[i].gCost
                    || !openNodes.Contains(neighbourNodes[i])) {

                    neighbourNodes[i].gCost = newGCostToNeighbour;
                    neighbourNodes[i].hCost = GetDistane(neighbourNodes[i], targetNode);
                    neighbourNodes[i].parrent = currentNode;

                    if (!openNodes.Contains(neighbourNodes[i])) {
                        openNodes.Add(neighbourNodes[i]);
                    }
                }
            }
        }
    }
    private int GetDistane(Node startNode, Node endNode) {



        return 0;
    }
    private void ReTracePath(Node startNode, Node endNode) {

    }
}

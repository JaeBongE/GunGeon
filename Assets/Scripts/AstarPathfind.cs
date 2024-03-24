using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathfind
{
    public AstarGrid grid;

    public AstarPathfind(AstarGrid grid)
    {
        this.grid = grid;
    }

    private float Heurisitc(AstarNode a, AstarNode b, bool diagonal = false)
    {
        //맨해튼 거리
        var dx = Mathf.Abs(a.xPos - b.xPos);
        var dy = Mathf.Abs(a.yPos - b.yPos);

        if (!diagonal) return 1 * (dx + dy);
        //체비쇼프 거리
        return Mathf.Max(Mathf.Abs(a.xPos - b.xPos), Mathf.Abs(a.yPos - b.yPos));
    }

    //public List<AstarNode> CreathPath(AstarNode start, AstarNode end, bool diagonal = false)
    //{
    //    if (start == null || end == null) return null;
    //    grid.ResetNode();
        
    //    List<AstarNode> openSet = new List<AstarNode>();
    //    List<AstarNode> closeSet = new List<AstarNode>();

    //    AstarNode startNode = start;
    //    AstarNode endNode = end;
    //    startNode.gCost = 0f;
    //    startNode.hCost = Heurisitc(start, end);
    //    openSet.Add(startNode);

    //    while (openSet.Count > 0)
    //    {
    //        //openSet 내의 노드 중 가장 거리가 짧은 노드를 찾는다.
    //        int shortest = 0;
    //        for (int iNum = 1; iNum < openSet.Count; iNum++)
    //        {
    //            if (openSet[iNum].fCost < openSet[shortest].fCost)
    //            {
    //                shortest = iNum;
    //            }
    //        }
    //        AstarNode currentNode = openSet[shortest];

    //        //목적지 도착
    //        if (currentNode == endNode)
    //        {
    //            //경로만들어서 반환
    //            List<AstarNode> path = new List<AstarNode>();
    //            path.Add(endNode);
    //            var tempNode = endNode;
    //            while (tempNode.parent != null)
    //            {
    //                path.Add(tempNode.parent);
    //                tempNode = tempNode.parent;
    //            }
    //            path.Reverse();
    //            return path;
    //        }

    //        //리스트를 업데이트 한다.
    //        openSet.Remove(currentNode);
    //        closeSet.Add(currentNode);

    //        //다음노드를 방문한다.
    //        //var neighbors = grid
    //    }
    //}
}

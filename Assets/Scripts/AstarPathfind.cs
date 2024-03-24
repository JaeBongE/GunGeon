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
        //����ư �Ÿ�
        var dx = Mathf.Abs(a.xPos - b.xPos);
        var dy = Mathf.Abs(a.yPos - b.yPos);

        if (!diagonal) return 1 * (dx + dy);
        //ü����� �Ÿ�
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
    //        //openSet ���� ��� �� ���� �Ÿ��� ª�� ��带 ã�´�.
    //        int shortest = 0;
    //        for (int iNum = 1; iNum < openSet.Count; iNum++)
    //        {
    //            if (openSet[iNum].fCost < openSet[shortest].fCost)
    //            {
    //                shortest = iNum;
    //            }
    //        }
    //        AstarNode currentNode = openSet[shortest];

    //        //������ ����
    //        if (currentNode == endNode)
    //        {
    //            //��θ��� ��ȯ
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

    //        //����Ʈ�� ������Ʈ �Ѵ�.
    //        openSet.Remove(currentNode);
    //        closeSet.Add(currentNode);

    //        //������带 �湮�Ѵ�.
    //        //var neighbors = grid
    //    }
    //}
}

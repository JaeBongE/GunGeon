using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarGrid : MonoBehaviour
{
    [SerializeField] Tilemap walkableMap;

    private AstarNode[,] grid; // [y,x] 그리드
    private AstarPathfind pathfinder;


    void Start()
    {
        CreateGrid();
        pathfinder = new AstarPathfind(this);
    }

    void Update()
    {
        
    }

    private void CreateGrid()
    {
        walkableMap.CompressBounds();
        BoundsInt bounds = walkableMap.cellBounds;
        grid = new AstarNode[bounds.size.y, bounds.size.x];
        for (int y = bounds.yMin, iNum = 0; iNum < bounds.size.y; y++, iNum++)
        {
            for (int x = bounds.xMin, jNum = 0;  jNum < bounds.size.x; x++, jNum++)
            {
                AstarNode node = new AstarNode();
                node.yIndex = iNum;
                node.xIndex = jNum;
                node.gCost = int.MaxValue;
                node.parent = null;
                node.yPos = walkableMap.CellToWorld(new Vector3Int(x, y)).y;
                node.xPos = walkableMap.CellToWorld(new Vector3Int(x, y)).x;
                //walkableMap에 타일이 있으면 이동 가능한 노드, 타일이 없으면 이동 불가능한 노드이다.
                if (walkableMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    node.isWarkable = true;
                    grid[iNum, jNum] = node;
                }
                else
                {
                    node.isWarkable = false;
                    grid[iNum, jNum] = node;
                }
            }
        }
    }

    public void ResetNode()
    {
        foreach (AstarNode node in grid)
        {
            node.Reset();
        }
    }
}

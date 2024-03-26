using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarGrid : MonoBehaviour
{
    Camera mainCam;

    [SerializeField] Tilemap walkableMap;
    [Header("씬에 그리드를 표시")]
    [SerializeField] bool ShowTestGrid;
    [Header("대각선 탐색")]
    [SerializeField] bool Diagonal;

    private AstarNode[,] grid; // [y,x] 그리드
    private AstarPathfind pathfinder;

    private AstarNode startNode;
    private AstarNode endNode;
    private GameObject objEnemy;


    void Start()
    {
        CreateGrid();
        pathfinder = new AstarPathfind(this);
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            startNode = GetNodeFromWorld(pos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            endNode = GetNodeFromWorld(pos);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PathFind(Diagonal);
        }
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

    public AstarNode GetNodeFromWorld(Vector3 _worldPos)
    {
        //월드 좌표로 해당 좌표의 AstarNode 인스턴스를 얻는다.
        Vector3Int cellPos = walkableMap.WorldToCell(_worldPos);
        int y = cellPos.y + Mathf.Abs(walkableMap.cellBounds.yMin);
        int x = cellPos.x + Mathf.Abs(walkableMap.cellBounds.xMin);

        AstarNode node = grid[y, x];
        return node;
    }

    public List<AstarNode> GetNeighborNodes(AstarNode _node, bool diagonal = false)
    {
        List<AstarNode> neighbors = new List<AstarNode>();
        int height = grid.GetUpperBound(0);
        int width = grid.GetUpperBound(1);

        int y = _node.yIndex;
        int x = _node.xIndex;
        //상하
        if (y < height)
        {
            neighbors.Add(grid[y + 1, x]);
        }
        if (y > 0)
        {
            neighbors.Add(grid[y - 1, x]);
        }
        //좌우
        if(x < width)
        {
            neighbors.Add(grid[y, x + 1]);
        }
        if (x > 0)
        {
            neighbors.Add(grid[y, x - 1]);
        }

        if (!diagonal) return neighbors;

        //대각선
        if (x > 0 && y > 0)
        {
            neighbors.Add(grid[y - 1, x - 1]);
        }
        if (x < width && y > 0)
        {
            neighbors.Add(grid[y - 1, x + 1]);
        }
        if (x > 0 && y < height)
        {
            neighbors.Add(grid[y + 1, x - 1]);
        }
        if (x < width && y < height)
        {
            neighbors.Add(grid[y + 1, x + 1]);
        }

        return neighbors;
    }

    private void OnDrawGizmos()
    {
        if (grid != null && ShowTestGrid == true)
        {
            foreach (var node in grid)
            {
                Gizmos.color = Color.red;
                Vector3Int cellPos = walkableMap.WorldToCell(new Vector3(node.xPos, node.yPos));
                Vector3 drawPos = walkableMap.GetCellCenterWorld(cellPos);
                drawPos -= walkableMap.cellGap / 2;
                Vector3 drawsize = walkableMap.cellSize;
                Gizmos.DrawWireCube(drawPos, drawsize);
            }
        }
    }

    public void PathFind(bool diagonal)
    {
        List<AstarNode> path = pathfinder.CreathPath(startNode, endNode, diagonal);
        if (path != null)
        {
            for (int iNum = 0; iNum < path.Count -1; iNum++)
            {
                Vector3Int startCellPos = walkableMap.WorldToCell(new Vector3(path[iNum].xPos, path[iNum].yPos));
                Vector3 startCenterPos = walkableMap.GetCellCenterLocal(startCellPos);
                startCenterPos -= walkableMap.cellGap / 2;

                Vector3Int endCellPos = walkableMap.WorldToCell(new Vector3(path[iNum + 1].xPos, path[iNum + 1].yPos));
                Vector3 endCenterPos = walkableMap.GetCellCenterLocal(endCellPos);
                endCenterPos -= walkableMap.cellGap / 2;

                Debug.DrawLine(new Vector3(path[iNum].xPos, path[iNum].yPos), new Vector3(path[iNum + 1].xPos, path[iNum + 1].yPos), Color.black, 2f);
                Debug.DrawLine(startCellPos, endCenterPos, Color.white, 2f);
            }
        }
    }

}

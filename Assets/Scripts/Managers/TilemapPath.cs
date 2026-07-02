using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPath : MonoBehaviour
{
    [SerializeField] Tilemap pathTilemap;
    [SerializeField] TileBase startTile;

    [Header("Hex Settings")]
    [SerializeField] bool pointTop = true; 
    // true  = Hexagon Point Top
    // false = Hexagon Flat Top
    [SerializeField] Vector3 worldOffset = Vector3.zero;
    // np. jeśli przeciwnik nadal jest za nisko, ustaw np. (0, 0.2f, 0)

    public static Vector3[] Points { get; private set; }

    void Awake()
    {
        Points = BuildPath();
    }

    Vector3[] BuildPath()
    {
        Vector3Int start = FindStartCell();
        List<Vector3Int> cells = TraceFrom(start);

        Vector3[] worldPoints = new Vector3[cells.Count];

        for (int i = 0; i < cells.Count; i++)
        {
            worldPoints[i] = pathTilemap.GetCellCenterWorld(cells[i]) + worldOffset;
        }

        return worldPoints;
    }

    Vector3Int FindStartCell()
    {
        foreach (Vector3Int cell in pathTilemap.cellBounds.allPositionsWithin)
        {
            if (pathTilemap.GetTile(cell) == startTile)
                return cell;
        }

        Debug.LogError("TilemapPath: nie znalazłem kafelka startowego!");
        return Vector3Int.zero;
    }

    List<Vector3Int> TraceFrom(Vector3Int start)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        Vector3Int current = start;

        path.Add(current);
        visited.Add(current);

        bool moved = true;

        while (moved)
        {
            moved = false;

            foreach (Vector3Int dir in GetHexDirs(current))
            {
                Vector3Int next = current + dir;

                if (!visited.Contains(next) && pathTilemap.HasTile(next))
                {
                    current = next;
                    path.Add(current);
                    visited.Add(current);
                    moved = true;
                    break;
                }
            }
        }

        return path;
    }

    Vector3Int[] GetHexDirs(Vector3Int cell)
    {
        if (pointTop)
        {
            // Hexagon Point Top — zależne od parzystości Y
            bool oddRow = Mathf.Abs(cell.y) % 2 == 1;

            if (oddRow)
            {
                return new Vector3Int[]
                {
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(1, 1, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(0, -1, 0)
                };
            }
            else
            {
                return new Vector3Int[]
                {
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(-1, -1, 0)
                };
            }
        }
        else
        {
            // Hexagon Flat Top — zależne od parzystości X
            bool oddColumn = Mathf.Abs(cell.x) % 2 == 1;

            if (oddColumn)
            {
                return new Vector3Int[]
                {
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(1, 1, 0),
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(-1, 0, 0)
                };
            }
            else
            {
                return new Vector3Int[]
                {
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(-1, -1, 0)
                };
            }
        }
    }
}
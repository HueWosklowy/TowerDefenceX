using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPath : MonoBehaviour
{
    [SerializeField] Tilemap pathTilemap;   // tilemapa, na której narysowana jest ścieżka
    [SerializeField] TileBase startTile;     // wyróżniony kafelek = początek trasy

    public static Vector3[] Points { get; private set; }   // środki kafelków w kolejności

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
            worldPoints[i] = pathTilemap.GetCellCenterWorld(cells[i]);

        return worldPoints;
    }

    Vector3Int FindStartCell()
    {
        foreach (Vector3Int cell in pathTilemap.cellBounds.allPositionsWithin)
        {
            if (pathTilemap.GetTile(cell) == startTile)
                return cell;
        }
        Debug.LogError("TilemapPath: nie znalazłem kafelka startowego (startTile)!");
        return Vector3Int.zero;
    }

    List<Vector3Int> TraceFrom(Vector3Int start)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        Vector3Int current = start;
        path.Add(current);
        visited.Add(current);

        // Sąsiedzi w 4 kierunkach (bez skosów)
        Vector3Int[] dirs = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };

        bool moved = true;
        while (moved)
        {
            moved = false;
            foreach (Vector3Int dir in dirs)
            {
                Vector3Int next = current + dir;
                if (!visited.Contains(next) && pathTilemap.HasTile(next))
                {
                    current = next;
                    path.Add(current);
                    visited.Add(current);
                    moved = true;
                    break;   // podążamy jedną ścieżką
                }
            }
        }
        return path;
    }
}
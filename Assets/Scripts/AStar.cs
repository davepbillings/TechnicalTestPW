using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    static List<Tile> path = new List<Tile>();
    static List<Tile> neighbors = new List<Tile>();
    static List<Tile> openSet = new List<Tile>();
    static List<Tile> closedSet = new List<Tile>();

    //find path using a* method (f value comparisons to determine optimal route)
    public static List<Tile> FindPath(Tile[,] grid, Tile start, Tile goal)
    {
        Debug.Log("starting path finding");
        openSet.Clear();
        closedSet.Clear();

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            Tile current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].f < current.f || (openSet[i].f == current.f && openSet[i].h < current.h))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            //when goal is reached, reverse path
            if (current == goal)
            {
                return ReconstructPath(goal);
            }

            //compare neighbor tiles distance scores
            foreach (Tile neighbor in GetNeighbors(grid, current))
            {
                if (closedSet.Contains(neighbor) || !neighbor.isWalkable)
                {
                    continue;
                }

                double tentativeGScore = current.g + CalculateDistance(current, neighbor);

                if (!openSet.Contains(neighbor) || tentativeGScore < neighbor.g)
                {
                    neighbor.parent = current;
                    neighbor.g = tentativeGScore;
                    neighbor.h = CalculateDistance(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        Debug.Log("No Viable Path");
        return null; // No path found
    }

    //distance calculation
    private static double CalculateDistance(Tile a, Tile b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    //assign list of neighbor tiles
    private static List<Tile> GetNeighbors(Tile[,] grid, Tile tile)
    {
        neighbors.Clear();
        int x = tile.x;
        int y = tile.y;

        if (x > 0) neighbors.Add(grid[x - 1, y]);
        if (x < grid.GetLength(0) - 1) neighbors.Add(grid[x + 1, y]);
        if (y > 0) neighbors.Add(grid[x, y - 1]);
        if (y < grid.GetLength(1) - 1) neighbors.Add(grid[x, y + 1]);

        return neighbors;
    }

    //retrace path back to start
    private static List<Tile> ReconstructPath(Tile current)
    {
        path.Clear();
        while (current != null)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }
}

using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;


public static class PathFinder
{
    private const float PerpendicularDistance = 1f;
    private const float DiagonalDistance = 1.4f;

    // public static Cell DeikstraSearch(Cell entry, Cell exit)
    // {
    //     Dictionary<int, Cell> visited = new Dictionary<int, Cell>();
    //     Queue<Cell> toVisit = new Queue<Cell>();
    //     Dictionary<int, Cell> toVisitDictionary = new Dictionary<int, Cell>();
    //
    //     toVisit.Enqueue(entry);
    //     toVisitDictionary.Add(entry.GetHashCode(), entry);
    //
    //     while (toVisit.Count > 0)
    //     {
    //         Cell current = toVisit.Dequeue();
    //         current = toVisitDictionary[current.GetHashCode()];
    //         toVisitDictionary.Remove(current.GetHashCode());
    //
    //         if (current.Equals(exit))
    //         {
    //             return current;
    //         }
    //
    //         visited.Add(current.GetHashCode(), current);
    //         List<Cell> neighbours = GetNeighbours(current);
    //         foreach (Cell neighbour in neighbours)
    //         {
    //             if (!visited.ContainsKey(neighbour.GetHashCode()) &&
    //                 !toVisitDictionary.ContainsKey(neighbour.GetHashCode()))
    //             {
    //                 toVisit.Enqueue(neighbour);
    //                 toVisitDictionary.Add(neighbour.GetHashCode(), neighbour);
    //             }
    //             else if (visited.ContainsKey(neighbour.GetHashCode())
    //                      && visited[neighbour.GetHashCode()].Distance > neighbour.Distance)
    //             {
    //                 visited.Remove(neighbour.GetHashCode());
    //
    //                 toVisit.Enqueue(neighbour);
    //                 toVisitDictionary.Add(neighbour.GetHashCode(), neighbour);
    //             }
    //             else if (toVisitDictionary.ContainsKey(neighbour.GetHashCode())
    //                      && toVisitDictionary[neighbour.GetHashCode()].Distance > neighbour.Distance)
    //             {
    //                 toVisitDictionary[neighbour.GetHashCode()] = neighbour;
    //             }
    //         }
    //     }
    //
    //     return null;
    // }
    

    public static Cell SearchInDepth(Cell entry, Cell target)
    {
        Dictionary<int, Cell> visited = new Dictionary<int, Cell>();
        Stack<Cell> toVisit = new Stack<Cell>();

        entry.DistanceLeft = (target.Position - entry.Position).magnitude;
        toVisit.Push(entry);
        visualise(target, VisualAction.Target);
        visualise(entry, VisualAction.ToVisit);
        
        while (toVisit.Count > 0)
        {
            Cell current = toVisit.Pop();
            visualise(current, VisualAction.Visiting);
            if (current.Equals(target))
            {
                return current;
            }

            visited.Add(current.GetHashCode(), current);
            List<Cell> neighbours = GetNeighbours(current);
            foreach (Cell neighbour in neighbours)
            {
                if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisit.Contains(neighbour))
                {
                    neighbour.DistanceLeft = (target.Position - neighbour.Position).magnitude;
                    toVisit.Push(neighbour);
                    visualise(neighbour, VisualAction.ToVisit);
                }
            }
            
            visualise(current, VisualAction.Visited);
        }

        return null;
    }

    private static Cell SearchInWidth(Cell entry, Cell target)
    {
        Dictionary<int, Cell> visited = new Dictionary<int, Cell>();
        Queue<Cell> toVisit = new Queue<Cell>();

        entry.DistanceLeft = (target.Position - entry.Position).magnitude;
        toVisit.Enqueue(entry);
        visualise(target, VisualAction.Target);
        visualise(entry, VisualAction.ToVisit);

        while (toVisit.Count > 0)
        {
            Cell current = toVisit.Dequeue();
            visualise(current, VisualAction.Visiting);

            if (current.Equals(target))
            {
                return current;
            }

            visited.Add(current.GetHashCode(), current);
            List<Cell> neighbours = GetNeighbours(current);
            foreach (Cell neighbour in neighbours)
            {
                if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisit.Contains(neighbour))
                {
                    neighbour.DistanceLeft = (target.Position - neighbour.Position).magnitude;
                    toVisit.Enqueue(neighbour);
                    visualise(neighbour, VisualAction.ToVisit);
                }
            }
            visualise(current, VisualAction.Visited);
        }

        return null;
    }
    
    private static Cell SearchDirected(Cell entry, Cell target)
    {
        Dictionary<int, Cell> visited = new Dictionary<int, Cell>();
        SortedSet<Cell> toVisit = new SortedSet<Cell>(new CellComparer());
        Dictionary<int, Cell> toVisitDic = new Dictionary<int, Cell>();
        
        entry.DistanceLeft = (target.Position - entry.Position).magnitude;
        toVisit.Add(entry);
        toVisitDic.Add(entry.GetHashCode(), entry);
        visualise(target, VisualAction.Target);
        visualise(entry, VisualAction.ToVisit);

        while (toVisit.Count > 0)
        {
            Cell current = toVisit.Min;
            visited.Add(current.GetHashCode(), current);
            toVisit.Remove(current);
            toVisitDic.Remove(current.GetHashCode());
            visualise(current, VisualAction.Visiting);

            if (current.Equals(target))
            {
                return current;
            }
            List<Cell> neighbours = GetNeighbours(current);
            foreach (Cell neighbour in neighbours)
            {
                if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisitDic.ContainsKey(neighbour.GetHashCode()))
                {
                    neighbour.DistanceLeft = (target.Position - neighbour.Position).magnitude;
                    toVisit.Add(neighbour);
                    toVisitDic.Add(neighbour.GetHashCode(), neighbour);
                    visualise(neighbour, VisualAction.ToVisit);
                }
            }
            visualise(current, VisualAction.Visited);
        }

        return null;
    }

    private static List<Cell> GetNeighbours(Cell cell, bool addDiagonal = false)
    {
        List<Cell> neighbours = new List<Cell>();

        for (int x = -1; x < 2; x += 2)
        {
            Cell horizontalNeighbour = cell.GetNeighbour(x, 0);
            AddIfFreeToMove(horizontalNeighbour, PerpendicularDistance);

            for (int y = -1; y < 2; y += 2)
            {
                Cell verticalNeighbour = cell.GetNeighbour(0, y);

                if (x == -1)
                    AddIfFreeToMove(verticalNeighbour, PerpendicularDistance);

                if (addDiagonal && horizontalNeighbour.IsFreeToMove() && verticalNeighbour.IsFreeToMove())
                {
                    Cell diagonalNeighbour = cell.GetNeighbour(x, y);
                    AddIfFreeToMove(diagonalNeighbour, DiagonalDistance);
                }
            }
        }

        return neighbours;

        void AddIfFreeToMove(Cell newCell, float distance)
        {
            if (newCell.IsFreeToMove())
                neighbours.Add(newCell.SetParent(cell).SetDistance(cell.Distance + distance));
        }
    }


    public static Cell Search(Cell entry, Cell target, SearchType searchType = SearchType.Width)
    {
        WayHighlighter.New();
        return searchType switch
        {
            SearchType.Width => SearchInWidth(entry, target),
            SearchType.Directed => SearchDirected(entry, target),
            _ => SearchInDepth(entry, target)
        };
    }

    private static void visualise(Cell item, VisualAction action)
    {
        if (SceneController.Instance.isVisualiseSearch)
        {
            WayHighlighter.Queue.Enqueue(new KeyValuePair<VisualAction, Cell>(action, item));
        }
    }

}
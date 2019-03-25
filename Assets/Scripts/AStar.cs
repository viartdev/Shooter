using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x; 
        this.Y = y;
    }

    public static bool operator ==(Point p1, Point p2)
    {
        return p1.X == p2.X && p1.Y == p2.Y;
    }

    public static bool operator !=(Point p1, Point p2)
    {
        return p1.X != p2.X || p1.Y != p2.Y;
    }
}

public class Cell
    {

        public Point Position { get; set; }
        public Cell PreviousCell { get; set; }

        public int PathLengthFromStart { get; set; }    
        public int FromCellToFinishLength { get; set; }

        public int FullPathLength
        {
            get { return this.PathLengthFromStart + this.FromCellToFinishLength; }
        }
    }


 static class AStar
 {

     private static int StartRoomID;
     private static int EndRoomID;

    private static  List<Point> GetPathToCell(Cell cell)
    {
        List<Point> result = new List<Point>();
        var activCell = cell;
        while (activCell != null)
        {
            result.Add(activCell.Position);
            activCell = activCell.PreviousCell;
        }

        result.Reverse();
        return result;
    }

    private static int CalculatePathLength(Point start, Point end)
    {
        int deltaX = Math.Abs(start.X - end.X);
        int deltaY = Math.Abs(start.Y - end.Y);
        return deltaX + deltaY;
    }

    public static List<Point> CalculatePathCellsList(int[,] field, Point start, Point end)
    {
        var closedCells = new List<Cell>();
        var activeCells = new List<Cell>();

        Cell startCell = new Cell()
        {
            Position = start,
            PreviousCell = null,
            PathLengthFromStart = 0,
            FromCellToFinishLength = CalculatePathLength(start, end)
        };
        activeCells.Add(startCell);

        while (activeCells.Count > 0)
        {
            var activeCell = activeCells.OrderBy(cell => cell.FullPathLength).First();
            
            if (activeCell.Position == end)
                return GetPathToCell(activeCell);
            activeCells.Remove(activeCell);
            closedCells.Add(activeCell);

            foreach (var nearCell in GetNearCells(activeCell, end, field))
            {
                if (closedCells.Count(cell => cell.Position == nearCell.Position) > 0)
                    continue;
                var openCell = activeCells.FirstOrDefault(cell =>
                    cell.Position == nearCell.Position);
                if (openCell == null)
                    activeCells.Add(nearCell);
                else if (openCell.PathLengthFromStart > nearCell.PathLengthFromStart)
                {

                    openCell.PreviousCell = activeCell;
                    openCell.PathLengthFromStart = nearCell.PathLengthFromStart;
                }
            }
        }

        return null;
    }

    private static List<Cell> GetNearCells(Cell cell,Point end, int[,] field)
    {
        List<Cell> result = new List<Cell>();

        Point[] NearPoint = new Point[4];
        NearPoint[0] = new Point(cell.Position.X + 1, cell.Position.Y);
        NearPoint[1] = new Point(cell.Position.X - 1, cell.Position.Y);
        NearPoint[2] = new Point(cell.Position.X, cell.Position.Y + 1);
        NearPoint[3] = new Point(cell.Position.X, cell.Position.Y - 1);

        foreach (var point in NearPoint)
        {
            if (point.X < 0 || point.X >= field.GetLength(0))
                continue;
            if (point.Y < 0 || point.Y >= field.GetLength(1))
                continue;
            if (field[point.X, point.Y] == 0 || field[point.X, point.Y] == StartRoomID || field[point.X, point.Y] == EndRoomID)
            {

                var nearCell = new Cell()
                {
                    Position = point,
                    PreviousCell = cell,
                    PathLengthFromStart = cell.PathLengthFromStart + 1,
                    FromCellToFinishLength = CalculatePathLength(point, end)
                };
                result.Add(nearCell);
            }
        }

        return result;
    }


    private static List<Vector3> CalculatePathVectorList(int[,] field, Vector3 start, Vector3 end)
    {
        Point startPoint = new Point((int)start.x, (int)start.z);
        Point endPoint = new Point((int)end.x, (int)end.z);
        List<Point> findedPath = CalculatePathCellsList(field, startPoint, endPoint);
        List<Vector3> findedPathVector3 = new List<Vector3>();
        foreach (var point in findedPath)
        {
            Vector3 vector = new Vector3(point.X, 0.0f, point.Y);
            findedPathVector3.Add(vector);
        }

        return findedPathVector3;
    }
    public static List<Vector2> CalculatePathVector2List(int[,] field, Vector2 start, Vector2 end,int startRoomID, int endRoomID)
    {
        StartRoomID = startRoomID;
        EndRoomID = endRoomID;
        Point startPoint = new Point((int)start.x, (int)start.y);
        Point endPoint = new Point((int)end.x, (int)end.y);
        List<Point> findedPath = CalculatePathCellsList(field, startPoint, endPoint);
        List<Vector2> findedPathVector2 = new List<Vector2>();
        if (findedPath == null)
            return null;
        foreach (var point in findedPath)
        {
            Vector2 vector = new Vector2(point.X,point.Y);
            findedPathVector2.Add(vector);
        }

        return findedPathVector2;
    }

    public static Vector3 GetNextCellVector(int[,] field, Vector3 start, Vector3 end)
    {
        Point startPoint = new Point((int) start.x, (int) start.z);
        Point endPoint = new Point((int) end.x, (int) end.z);
        List<Point> pointList = CalculatePathCellsList(field, startPoint, endPoint);
        return new Vector3(pointList[1].X, 0.0f, pointList[1].Y);
    }
 }

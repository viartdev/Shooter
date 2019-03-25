using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

class Room
{
    private int MinRoomSize = 5;
    private int MaxRoomSize = 10;
    private int NeighboursCount = 3;

    public Vector2 RoomSize { get; set; }
    public Vector2 RoomPosition { get; set; }
    public int ID { get; set; }

    public List<Room> Neighbours;

    public Room(int[,] Field)
    {
        Neighbours = new List<Room>();
        GenerateRoomSize();
        GenerateRoomPosition(Field);
    }



    private void GenerateRoomSize()
    {
        int xSize = Random.Range(MinRoomSize, MaxRoomSize);
        int ySize = Random.Range(MinRoomSize, MaxRoomSize);
        if (xSize % 2 == 0)
        {
            xSize += 1;
        }

        if (ySize % 2 == 0)
        {
            ySize += 1;
        }

        RoomSize = new Vector2(xSize, ySize);
    }

    private void GenerateRoomPosition(int[,] Field)
    {
        bool CanBePlaced = false;

        while (!CanBePlaced)
        {
            bool otherRoom = false;
            int xPos = (int) Random.Range(RoomSize.x / 2 + 1, Field.GetLength(0) - (RoomSize.x / 2 + 1));
            int yPos = (int) Random.Range(RoomSize.y / 2 + 1, Field.GetLength(0) - (RoomSize.y / 2 + 1));

            for (int x = (int) (xPos - RoomSize.x / 2); x < (int) (xPos + RoomSize.x / 2); x++)
            {
                for (int y = (int) (yPos - RoomSize.y / 2); y < (int) (yPos + RoomSize.y / 2); y++)
                {
                    if (Field[x, y] > 0)
                    {
                        otherRoom = true;
                        break;
                    }
                }

                if (otherRoom)
                {
                    break;
                }
            }

            if (!otherRoom)
            {
                CanBePlaced = true;
                RoomPosition = new Vector2(xPos, yPos);
            }
        }
    }

    private bool NeighbourFinded(int val, List<Room> roms)
    {
        if (val != 0 && val != ID)
        {
            if (!IsExistNeighbour(val))
            {
                Room neightbour = GetRoomByID(val, roms);
                Neighbours.Add(neightbour);
                return true;
            }

        }

        return false;
    }

    public void FindNeighbours(int[,] Field, List<Room> roms)
    {
        int FindedNeighbours = 0;
        int x = (int) RoomPosition.x;
        int y = (int) RoomPosition.y;
        int lenght = 1;

        while (FindedNeighbours < NeighboursCount)
        {
            for (int i = x - lenght; i < x + lenght; i++)
            {
                if (i < 0 || i > Field.GetLength(0) - 1)
                    continue;
                if (y - lenght >= 0)
                {
                    if (NeighbourFinded(Field[i, y - lenght], roms))
                        FindedNeighbours += 1;
                }

                if (y + lenght <= Field.GetLength(1) - 1)
                {
                    if (NeighbourFinded(Field[i, y + lenght], roms))
                        FindedNeighbours += 1;
                }
            }

            for (int i = y - lenght; i < y + lenght; i++)
            {
                if (i < 0 || i > Field.GetLength(1) - 1)
                    continue;
                if (x - lenght >= 0)
                {
                    if (NeighbourFinded(Field[x - lenght, i], roms))
                        FindedNeighbours += 1;
                }

                if (x + lenght <= Field.GetLength(0) - 1)
                {
                    if (NeighbourFinded(Field[x + lenght, i], roms))
                        FindedNeighbours += 1;
                }

            }

            Debug.Log(lenght);
            Debug.Log(FindedNeighbours);
            lenght++;
        }

    }

    private bool IsExistNeighbour(int neighbourID)
    {
        foreach (var neighbour in Neighbours)
        {
            if (neighbourID == neighbour.ID)
            {
                return true;
            }
        }

        return false;
    }

    private Room GetRoomByID(int roomID, List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            if (room.ID == roomID)
            {
                return room;
            }
        }

        return null;
    }
}






public class BoardManager : NetworkBehaviour
{
    public GameObject EarthTile;
    public GameObject FloorTile;

    private List<Vector2> roomIDPath = new List<Vector2>();
    private Vector2 FieldSize = new Vector2(70, 70);
    private int RoomsCount = 15;
    private int[,] Field;
    private List<Room> Rooms;


    void Start()
    {
        Random.InitState(42);
        InitField();
        CreateRooms();
        InitNeighbours();
        CreateGraph();
        CreateWalls();
        LayoutTiles();
    }



    private void InitField()
    {
        Field = new int[(int) FieldSize.x, (int) FieldSize.y];
        for (int i = 0; i < Field.GetLength(0); i++)
        {
            for (int j = 0; j < Field.GetLength(1); j++)
            {
                Field[i, j] = 0;
            }
        }
    }

    private void CreateRooms()
    {
        Rooms = new List<Room>();
        for (int i = 0; i < RoomsCount; i++)
        {
            Room room = new Room(Field);
            room.ID = i + 1;
            Rooms.Add(room);
            ChangeField(room);
        }
    }

    private void ChangeField(Room activeRoom)
    {
        Vector2 RoomPosition = activeRoom.RoomPosition;
        Vector2 RoomSize = activeRoom.RoomSize;

        for (int i = (int) (RoomPosition.x - RoomSize.x / 2); i < (int) (RoomPosition.x + RoomSize.x / 2); i++)
        {
            for (int j = (int) (RoomPosition.y - RoomSize.y / 2); j < (int) (RoomPosition.y + RoomSize.y / 2); j++)
            {
                Field[i, j] = activeRoom.ID;
            }
        }
    }

    private void LayoutTiles()
    {
        for (int i = 0; i < Field.GetLength(0); i++)
        {
            for (int j = 0; j < Field.GetLength(1); j++)
            {
                if (Field[i, j] > 0 || Field[i, j] == -1)
                {
                    Instantiate(FloorTile, new Vector2(i, j), Quaternion.identity);
                }
                else if (Field[i, j] == -2)
                {
                    Instantiate(EarthTile, new Vector2(i, j), Quaternion.identity);
                }
            }
        }
    }

    private void InitNeighbours()
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            Rooms[i].FindNeighbours(Field, Rooms);
        }
    }

    private void CreateGraph()
    {
        foreach (var room in Rooms)
        {
            for (int i = 0; i < room.Neighbours.Count; i++)
            {
                CreatePath(room, room.Neighbours[i]);
            }
        }
    }

    private void CreatePath(Room startRoom, Room endRoom)
    {
        int startRoomID = startRoom.ID;
        int endRoomID = endRoom.ID;

        foreach (var path in roomIDPath)
        {
            if ((startRoomID == path.x && endRoomID == path.y) || (startRoomID == path.y && endRoomID == path.x))
            {
                return;
            }

            foreach (var neighbour in endRoom.Neighbours)
            {
                if ((startRoomID == path.y && neighbour.ID == path.x) ||
                    (startRoomID == path.x && neighbour.ID == path.y))
                {
                    return;
                }
            }
        }

        Vector2 startPoint = startRoom.RoomPosition;
        Vector2 endPoint = endRoom.RoomPosition;
        List<Vector2> findedPath = AStar.CalculatePathVector2List(Field, startPoint, endPoint, startRoomID, endRoomID);
        if (findedPath == null)
        {
            return;
        }

        roomIDPath.Add(new Vector2(startRoomID, endRoomID));
        FillPathField(findedPath);

    }

    private void FillPathField(List<Vector2> path)
    {
        foreach (var point in path)
        {
            if (Field[(int) point.x, (int) point.y] == 0)
            {
                Field[(int) point.x, (int) point.y] = -1;
            }

        }
    }

    private void CreateWalls()
    {
        for (int i = 0; i < Field.GetLength(0); i++)
        {
            for (int j = 0; j < Field.GetLength(1); j++)
            {
                if (Field[i, j] > 0 || Field[i, j] == -1)
                {
                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            if (k < 0 || k >= Field.GetLength(0) || l < 0 || l >= Field.GetLength(1))
                                continue;
                            if (Field[k, l] == 0)
                            {
                                Field[k, l] = -2;
                            }
                        }
                    }
                }
            }
        }
    }

    public Vector3 GetRandomPosition()
    {
        int number = Random.Range(0, Rooms.Count);
        Vector2 position = Rooms[number].RoomPosition;
        return position;
    }




}
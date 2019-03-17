using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class BoardManager : NetworkBehaviour
{
    public GameObject[] FloorPrefabs;
    public GameObject WallPrefab;

    private int fieldSizeX = 16;
    private int fieldSizeY = 16;

    private Transform boardHolder;


    public void SetupSceen()
    {
        boardHolder = new GameObject("Board").transform;
        LayoutRoom();
    }


    // Start is called before the first frame update
    void Start()
    {
        SetupSceen();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void LayoutRoom()
    {

        int floorId = FloorPrefabs.Length;

        for (int x = 0; x < fieldSizeX; x++)
        {
            for (int y = 0; y < fieldSizeY; y++)
            {
                GameObject instance;
                Vector3 position = new Vector3(x, y);

                if (x == 0 || y == 0 || x == fieldSizeX - 1 || y == fieldSizeY - 1)
                    instance = Instantiate(WallPrefab, position, Quaternion.identity);
                else
                {
                    floorId = Random.Range(0, FloorPrefabs.Length);
                    instance = Instantiate(FloorPrefabs[floorId], position, Quaternion.identity);
                }

                instance.transform.SetParent(boardHolder);
            }
        }
    }

}
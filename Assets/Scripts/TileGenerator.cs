using UnityEngine;
using UnityEngine.UI;

public class TileGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameMaster master;

    GridLayoutGroup group;

    void Awake()
    {
        int size = PlayerPrefs.GetInt("Size", 3);

        group = GetComponent<GridLayoutGroup>();

        group.constraintCount = size;
        group.cellSize = Vector2.one * Mathf.Min(Screen.width / size, Screen.height / size);

        master.tiles = new Tile[size, size];

        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                Tile tile = Instantiate(tilePrefab, transform.position, transform.rotation, transform).GetComponent<Tile>();

                tile.x = x;
                tile.y = y;

                master.tiles[x, y] = tile;
            }
        }

        Destroy(this);
    }
}

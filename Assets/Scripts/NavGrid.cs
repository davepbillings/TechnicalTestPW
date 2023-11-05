using UnityEngine;
using System.Collections.Generic;

public class NavGrid : MonoBehaviour
{
    private Tile[,] tiles;
    private GameObject tileHolder;
    private Material whiteTile, blackTile;
    private int gridHeight = 25, gridWidth = 25;

    [SerializeField]
    private TMPro.TMP_InputField widthField;
    [SerializeField]
    private TMPro.TMP_InputField heightField;

    private void Start()
    {
        Create();
    }

    public Tile[,] GetTiles()
    {
        return tiles;
    }
    /// <summary>
    /// read and validate input fields for custom grid size before setting variables
    /// </summary>
    public void SetWidthAndHeight()
    {
        if (CheckInputField(widthField.text) && CheckInputField(heightField.text))
        {
            gridWidth = System.Convert.ToInt32(widthField.text.ToString());
            gridHeight = System.Convert.ToInt32(heightField.text.ToString());
        }
    }
    /// <summary>
    /// utility to check for no non-numerical characters in string
    /// </summary>
    private bool CheckInputField(string fieldValue)
    {
        foreach (char c in fieldValue)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// remove old grid and create new one, checking for custom size
    /// </summary>
    public void RandomizeGrid()
    {
        Destroy();
        SetWidthAndHeight();
        Create();
    }
    /// <summary>
    /// create initial grid, establish materials 
    /// </summary>
    public void Create()
    {
        tileHolder = new GameObject("grid holder");
        DefaultTileMaterials();
        SpawnGrid();
    }

    public void Destroy()
    {
        GameObject.Destroy(tileHolder);
    }

    /// <summary>
    /// default black and white materials for tiles
    /// </summary>
    private void DefaultTileMaterials()
    {
        if (!whiteTile)
        {
            whiteTile = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            whiteTile.color = Color.white;
        }
        if (!blackTile)
        {
            blackTile = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            blackTile.color = Color.black;
        }
    }
    
    /// <summary>
    /// reconstruct the tiles in the list to clear f,g,h data
    /// </summary>
    public void RefreshTiles()
    {
        for (int a = 0; a < gridWidth; a++)
        {
            for (int b = 0; b < gridHeight; b++)
            {
                tiles[a, b] = new Tile(a, b, tiles[a, b].isWalkable);
            }
        }
    }

    /// <summary>
    /// spawn grid data
    /// </summary>
    private void SpawnGrid()
    {
        tiles = new Tile[gridWidth, gridHeight];

        for (int a = 0; a < gridWidth; a++)
        {
            for (int b = 0; b < gridHeight; b++)
            {
                tiles[a, b] = new Tile(a,b,true);
            }
        }
        GenerateBlockers();
    }

    /// <summary>
    /// generate random obsticals inside the grid, ignoring the outer ring
    /// </summary>
    private void GenerateBlockers()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.x > 0 & tile.x < gridWidth - 1 && tile.y > 0 && tile.y < gridHeight - 1)
            {
                tile.isWalkable=(Random.value < 0.5f);
            }
            PlaceTile(tile.x, tile.y, (tile.x.ToString() + " " + tile.y.ToString()), tile.isWalkable);
        }
    }

    /// <summary>
    /// place gameobjects for each tile in the grid and assign correct visual material
    /// </summary>
    private void PlaceTile(int x, int z, string name, bool walkable)
    {
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tile.name = name;
        if (walkable)
        {
            tile.GetComponent<Renderer>().sharedMaterial = whiteTile;
        }
        if (!walkable)
        {
            tile.GetComponent<Renderer>().sharedMaterial = blackTile;
        }
        tile.transform.localEulerAngles = new Vector3(90, 0, 0);
        tile.transform.position = new Vector3(x, 0, z);
        tile.transform.parent = tileHolder.transform;
    }

}

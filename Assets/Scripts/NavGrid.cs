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
                tiles[a, b].isWalkable = false;
            }
        }
        GenerateBlockers();
    }

    /// <summary>
    /// generate random obsticals inside the grid, ignoring the outer ring
    /// </summary>
    private void GenerateBlockers()
    {
        CarvePath(1, 1);
        foreach (Tile tile in tiles)
        {
            if (tile.x == 0 || tile.x == gridWidth - 1 || tile.y == 0 || tile.y == gridHeight - 1)
            {
                tile.isWalkable = true;//(Random.value < 0.5f);
            }
            
            
            PlaceTile(tile.x, tile.y, (tile.x.ToString() + " " + tile.y.ToString()), tile.isWalkable);
        }
        
        
    }
    private void CarvePath(int x, int y)
    {
        tiles[x, y].isWalkable = true;

        // Define the order in which we'll explore neighboring cells
        int[] directions = { 1, 2, 3, 4 };
        ShuffleArray(directions);

        foreach (int dir in directions)
        {
            int dx = 0, dy = 0;
            switch (dir)
            {
                case 1: // Up
                    dy = -2;
                    break;
                case 2: // Right
                    dx = 2;
                    break;
                case 3: // Down
                    dy = 2;
                    break;
                case 4: // Left
                    dx = -2;
                    break;
            }

            int newX = x + dx;
            int newY = y + dy;

            if (IsInBounds(newX, newY) && tiles[newX, newY].isWalkable == false)
            {
                tiles[x + dx / 2, y + dy / 2].isWalkable = true;
                CarvePath(newX, newY);
            }
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x > 0 && x < gridWidth - 1 && y > 0 && y < gridHeight - 1;
    }

    private void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i+1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
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

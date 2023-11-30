using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
   
    [SerializeField]
    private Player player;
    [SerializeField]
    private Material red;

    private void Start()
    {
        _currentPathIndex = 0;
    }

    public void IsEnemy(Vector2 target)
    {
        _grid.RefreshTiles();
        tileGrid = _grid.GetTiles();
        currentPath = AStar.FindPath(tileGrid, tileGrid[(int)transform.position.x, (int)transform.position.z], tileGrid[(int)target.x, (int)target.y]);
       //_currentPathIndex = 0;
       if (currentPath != null)
            {
                pathSmoothed = SmoothPointGenerator.CreateSmoothPath(SmoothPointGenerator.ConvertTileListToVector(currentPath), 3);
            }
        
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform == player.transform)
        {
            Debug.Log("collided with player");
            player.Died();
        }
    }


    void Update()
    {

        IsEnemy(new Vector2(player.transform.position.x, player.transform.position.z));
        Traverse();
        DebugMoving();
    }
   

    //visual debug for moving, could be modified to trigger character animations etc.
    private void DebugMoving()
    {
        if (moving && gameObject.GetComponent<MeshRenderer>().sharedMaterial.color != Color.red)
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
        }
        if (!moving && gameObject.GetComponent<MeshRenderer>().sharedMaterial.color != Color.grey)
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.grey;
        }
    }
    
    
}

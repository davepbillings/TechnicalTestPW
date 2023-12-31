using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public List<Tile> currentPath = new List<Tile>();
    public int _currentPathIndex = 0;
    public List<Vector2> pathSmoothed = new List<Vector2>();
    public Tile[,] tileGrid;
    public bool moving;
    public bool useSmoothing = true;
    public bool isEnemy = false;

    public bool isKilled = false;

    [SerializeField]
    public NavGrid _grid;
    [SerializeField]
    public float _speed = 10.0f;

    

    public void Died()
    {
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        
        CheckInput();
        Traverse();
        DebugMoving();
    }

    public void ToggleSmoothing()
    {
        useSmoothing = !useSmoothing;
    }

    //bring player back to spawn position and orientation
    public void ResetPlayerPosition()
    {
        pathSmoothed = null;
        currentPath = null;
        transform.position = new Vector3(0, 1, 0);
        moving = false;
        transform.eulerAngles = Vector3.zero;
    }

    //visual debug for moving, could be modified to trigger character animations etc.
    public void DebugMoving()
    {
        if (moving && gameObject.GetComponent<MeshRenderer>().sharedMaterial.color != Color.green)
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.green;
        }
        if (!moving && gameObject.GetComponent<MeshRenderer>().sharedMaterial.color != Color.grey)
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.grey;
        }
    }

    public void CheckInput()
    {
        // Check Input
        if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                _grid.RefreshTiles();
                tileGrid = _grid.GetTiles();
                currentPath = AStar.FindPath(tileGrid, tileGrid[(int)transform.position.x, (int)transform.position.z], tileGrid[(int)hitInfo.transform.position.x, (int)hitInfo.transform.position.z]);
                _currentPathIndex = 0;
                if(currentPath!= null)
                {
                    pathSmoothed = SmoothPointGenerator.CreateSmoothPath(SmoothPointGenerator.ConvertTileListToVector(currentPath),3);
                }
            }
        }
    }

    public void Traverse()
    {
        // because both smooth and unsmooth paths are generated can swap between both while moving
        if(!useSmoothing)
        {
            if (currentPath != null)
            {
                if (_currentPathIndex < currentPath.Count)
                {
                    moving = true;
                    var currentNode = currentPath[_currentPathIndex];

                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentNode.x, 1, currentNode.y), _speed * Time.deltaTime);
                    transform.LookAt(new Vector3(currentNode.x, 1, currentNode.y));

                    if (transform.position == new Vector3((int)currentNode.x, 1, (int)currentNode.y))
                    {
                        _currentPathIndex++;
                        Debug.Log("moving to next node");
                    }
                }
                if (_currentPathIndex >= currentPath.Count)
                {
                    moving = false;
                    currentPath = null;
                }
            }
        }
        if (useSmoothing)
        {
            if (pathSmoothed != null)
            {
                if (_currentPathIndex < pathSmoothed.Count)
                {
                    moving = true;
                    var currentNode = pathSmoothed[_currentPathIndex];

                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentNode.x, 1, currentNode.y), _speed * Time.deltaTime);
                    transform.LookAt(new Vector3(currentNode.x, 1, currentNode.y));

                    if (transform.position == new Vector3(currentNode.x, 1, currentNode.y))
                    {
                        _currentPathIndex++;
                        Debug.Log("moving to next node");
                    }
                }
                if (_currentPathIndex >= pathSmoothed.Count)
                {
                    moving = false;
                    pathSmoothed = null;
                }
            }
        }
    }
}

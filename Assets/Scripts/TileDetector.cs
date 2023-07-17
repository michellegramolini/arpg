using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;

public class TileDetector : MonoBehaviour
{
    [Header("Tilemap")]
    // Drag/assign in editor
    [SerializeField]
    private Tilemap _heightMap;
    [SerializeField]
    private Tilemap _terrainMap;

    // TODO: this should be tiles? //List<CustomProperty>
    private Dictionary<string, SuperTile> _heightTiles = new()
    {
        { "current", null },
        { "left", null },
        { "right", null },
        { "up", null },
        { "down", null },
        { "up-right", null },
        { "up-left", null },
        { "down-right", null },
        { "down-left", null }
    };

    private readonly Dictionary<string, Vector3Int> _adjacentPositions = new()
    {
        { "left", Vector3Int.left },
        { "right", Vector3Int.right },
        { "up", Vector3Int.up },
        { "down", Vector3Int.down },
        { "up-right", Vector3Int.up + Vector3Int.right },
        { "up-left", Vector3Int.up + Vector3Int.left },
        { "down-right", Vector3Int.down + Vector3Int.right },
        { "down-left", Vector3Int.down + Vector3Int.left }
    };

    private void Awake()
    {
        _heightMap = GameObject.FindWithTag("height_map").GetComponent<Tilemap>();
        _terrainMap = GameObject.FindWithTag("terrain_map").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectStandingTile();
        DetectAdjacentTiles();
    }

    // Detect tile
    void DetectStandingTile()
    {
        // get the tile grid position from whereever this object is 
        Vector3Int heightMapGridPosition = _heightMap.WorldToCell(transform.position);
        // TODO:?
        Vector3Int terrainMapGridPosition = _terrainMap.WorldToCell(transform.position);

        if (_heightMap.HasTile(heightMapGridPosition))
        {
            _heightTiles["current"] = _heightMap.GetTile<SuperTile>(heightMapGridPosition);
        }
        else
        {
            _heightTiles["current"] = null;
        }
    }

    void DetectAdjacentTiles()
    {
        // initialize grid position
        Vector3Int gridPosition = _heightMap.WorldToCell(transform.position);

        // if there's no tile there for some reason, return
        if (!_heightMap.HasTile(gridPosition))
        {
            Debug.LogWarning($"No tile at position: {gridPosition}");
            return;
        }

        // iterate through adjacent position vectors
        foreach (KeyValuePair<string, Vector3Int> adjacentPosition in _adjacentPositions)
        {
            Vector3Int position = gridPosition + adjacentPosition.Value;

            // if there is a tile there, add it to _heightTiles
            if (_heightMap.HasTile(position))
            {
                _heightTiles[adjacentPosition.Key] = _heightMap.GetTile<SuperTile>(position);

                // Debug
                //_heightMap.SetColor(position, Color.green);
            }
            else
            {
                _heightTiles[adjacentPosition.Key] = null;
            }
        }
    }

    // retrieve value off the CustomProperty with m_Value
    public CustomProperty GetTileProp(string tileKey, string propName)
    {
        if (tileKey == null || propName == null)
        {
            return null;
        }

        // TODO: swich based on propName
        bool hasTileProps = _heightTiles.TryGetValue(tileKey, out SuperTile tile);

        if (hasTileProps && tile != null)
        {
            foreach (CustomProperty prop in tile.m_CustomProperties)
            {
                if (prop.m_Name == propName)
                {
                    return prop;
                }
            }
        }

        return null;
    }

    public CustomProperty GetTilePropFromSuperTile(SuperTile superTile, string propName)
    {
        if (superTile == null)
        {
            return null;
        }

        foreach (CustomProperty prop in superTile.m_CustomProperties)
        {
            if (prop.m_Name == propName)
            {
                return prop;
            }
        }

        return null;
    }

    // TODO: either add paramater or create different function
    public SuperTile GetHeightTile(Vector3 position)
    {
        // get the tile grid position from whereever this object is
        Vector3Int gridPosition = _heightMap.WorldToCell(position);
        if (_heightMap.HasTile(gridPosition))
        {
            return _heightMap.GetTile<SuperTile>(gridPosition);
        }

        return null;
    }

    public SuperTile GetTerrainTile(Vector3 position)
    {
        // get the tile grid position from whereever this object is
        Vector3Int gridPosition = _terrainMap.WorldToCell(position);
        if (_terrainMap.HasTile(gridPosition))
        {
            return _terrainMap.GetTile<SuperTile>(gridPosition);
        }

        return null;
    }

}

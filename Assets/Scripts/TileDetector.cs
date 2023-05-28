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
    private Tilemap _tilemap;

    // TODO: add diagonal
    private Dictionary<string, List<CustomProperty>> _tileProperties = new()
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
        Vector3Int gridPosition = _tilemap.WorldToCell(transform.position);

        if (_tilemap.HasTile(gridPosition))
        {
            // set the current standing tile
            _tileProperties["current"] = _tilemap.GetTile<SuperTile>(gridPosition).m_CustomProperties;
        }
        else
        {
            // set the current standing tile
            // TODO: should be an empty list?
            _tileProperties["current"] = null;
        }


        //Debug.Log(currentTile.m_CustomProperties[0].m_Name.ToString() + ": " + currentTile.m_CustomProperties[0].m_Value.ToString());
    }

    void DetectAdjacentTiles()
    {
        // initialize grid position
        Vector3Int gridPosition = _tilemap.WorldToCell(transform.position);

        // if there's no tile there for some reason, return
        if (!_tilemap.HasTile(gridPosition))
        {
            Debug.LogWarning($"No tile at position: {gridPosition}");
            return;
        }

        // iterate through adjacent position vectors
        foreach (KeyValuePair<string, Vector3Int> adjacentPosition in _adjacentPositions)
        {
            Vector3Int position = gridPosition + adjacentPosition.Value;

            // if there is a tile there, add it to _tiles
            if (_tilemap.HasTile(position))
            {
                _tileProperties[adjacentPosition.Key] = _tilemap.GetTile<SuperTile>(position).m_CustomProperties;

                // Debug
                //_tilemap.SetColor(position, Color.green);
            }
            else
            {
                _tileProperties[adjacentPosition.Key] = null;
            }
        }
    }

    // retrieve value off the CustomProperty with m_Value
    public CustomProperty GetTileProp(string tileKey, string propName)
    {
        bool hasTileProps = _tileProperties.TryGetValue(tileKey, out List<CustomProperty> tileProps);

        if (hasTileProps && tileProps != null)
        {
            foreach (CustomProperty prop in tileProps)
            {
                if (prop.m_Name == propName)
                {
                    return prop;
                }
            }
        }

        return null;
    }
}

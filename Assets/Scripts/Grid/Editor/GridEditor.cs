using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    private Grid m_Grid;

    private void OnEnable()
    {
        m_Grid = (Grid)target;
    }
    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.modifiers == EventModifiers.Control)
        {
            // Stop selection
            GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

            // Validations
            if (m_Grid.SelectedTile >= m_Grid.AvailableTiles.Length)
            {
                throw new GridException("Selected tile is out of bounds!");
            }

            // Calculs de position
            Ray t_Rayon = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector2Int t_GridPos = m_Grid.WorldToGrid(t_Rayon.origin);
            Vector3 t_CellCenter = m_Grid.GridToWorld(t_GridPos);

            // Delete old tile
            Tile[] t_Tiles = m_Grid.GetComponentsInChildren<Tile>();
            foreach (Tile t_Tile in t_Tiles)
            {
                if (t_Tile.transform.position == t_CellCenter)
                {
                    Undo.DestroyObjectImmediate(t_Tile.gameObject);
                    break;
                }
            }

            // Spawner nouvelle tuile
            GameObject t_TilePrefab = m_Grid.AvailableTiles[m_Grid.SelectedTile];
            GameObject t_NewTile = (GameObject)PrefabUtility.InstantiatePrefab(t_TilePrefab, m_Grid.transform);
            Undo.RegisterCreatedObjectUndo(t_NewTile, "Tile created");
            t_NewTile.transform.position = t_CellCenter;

            // Dimensions de la tuile
            Sprite t_Sprite = t_NewTile.GetComponent<SpriteRenderer>().sprite;
            float t_Scale = m_Grid.CellSize / t_Sprite.bounds.size.x;
            t_NewTile.transform.localScale = new Vector3(t_Scale, t_Scale, t_Scale);
        }
    }

}


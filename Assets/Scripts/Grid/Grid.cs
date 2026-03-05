using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public uint RowCount = 10;
    public uint ColumnCount = 10;
    public float CellSize = 1;
    public GameObject[] AvailableTiles;
    public int SelectedTile;

    [SerializeField]
    private Color m_GridColor = Color.cyan;

    private List<Tile> m_Tiles;

    private void Awake()
    {
        m_Tiles = GetComponentsInChildren<Tile>().ToList();
        foreach (var t_Tile in m_Tiles)
        {
            Vector2Int t_GridPos = WorldToGrid(t_Tile.transform.position);
            t_Tile.x = (uint)t_GridPos.x;
            t_Tile.y = (uint)t_GridPos.y;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Debug.Log("OnDrawGizmosSelected");
        Gizmos.color = m_GridColor;
        //Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 1, 1));

        for (int i = 0; i < ColumnCount + 1; i++)
        {
            float t_Length = RowCount * CellSize;
            Vector3 t_From = new Vector3(i * CellSize, 0, 0) + transform.position;
            Vector3 t_To = new Vector3(i * CellSize, t_Length, 0) + transform.position;

            Gizmos.DrawLine(t_From, t_To);
        }
        for (int i = 0; i < RowCount + 1; i++)
        {
            float t_Length = ColumnCount * CellSize;
            Vector3 t_From = new Vector3(0, i * CellSize, 0) + transform.position;
            Vector3 t_To = new Vector3(t_Length, i * CellSize, 0) + transform.position;

            Gizmos.DrawLine(t_From, t_To);
        }



    }
    public Vector3 GridToWorld(Vector2Int a_GridPos)
    {


        if (a_GridPos.x >= ColumnCount || a_GridPos.x < 0)
        {
            throw new GridException("X value is out of grid!");
        }
        else if (a_GridPos.y >= RowCount || a_GridPos.y < 0)
        {
            throw new GridException("Y value is out of grid!");
        }

        float t_PosX = (a_GridPos.x + 0.5f) * CellSize;
        float t_PosY = (a_GridPos.y + 0.5f) * CellSize;

        return new Vector3(t_PosX, t_PosY, 0) + transform.position;
    }

    public Vector2Int WorldToGrid(Vector3 a_WorldPos)
    {
        //Raméne la grille à (0, 0, 0)
        a_WorldPos -= transform.position;

        // Division des position par Cellsize
        a_WorldPos /= CellSize;

        int t_PosX = Mathf.FloorToInt(a_WorldPos.x);
        int t_PosY = Mathf.FloorToInt(a_WorldPos.y);

        if (t_PosX >= ColumnCount || t_PosX < 0)
        {
            throw new GridException("X value is out of grid!");
        }
        else if (t_PosY >= RowCount || t_PosY < 0)
        {
            throw new GridException("Y value is out of grid!");
        }

        return new Vector2Int(t_PosX, t_PosY);
    }

    public Tile GetTile(Vector2Int a_Pos)
    {
        return m_Tiles.FirstOrDefault(t => t.x == a_Pos.x && t.y == a_Pos.y);
    }
}

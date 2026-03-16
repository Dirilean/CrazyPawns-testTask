using System;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    private int m_floorLayer;
    
    [SerializeField] private float m_checkboardQuadSize = 1.5f;
    [SerializeField] private Transform m_chessBoardTransform;

    private GameObject[] m_board;

    private void Start()
    {
        m_floorLayer = LayerMask.NameToLayer("Floor");
    }

    public void CreateBoard(int _checkerboardSize, Color _blackCellColor, Color _whiteCellColor)
    {
        DeleteBoard();

        //Вычисляем середину доски с учетом что пивот quad в его центре 
        float centerCheckBoardSize =
            -_checkerboardSize * 0.5f * m_checkboardQuadSize +
            (m_checkboardQuadSize * 0.5f);
        Vector3 offset = new Vector3(centerCheckBoardSize, 0, centerCheckBoardSize);

        m_board = new GameObject[_checkerboardSize * _checkerboardSize];
        for (int row = 0; row < _checkerboardSize; row++)
        {
            for (int col = 0; col < _checkerboardSize; col++)
            {
                m_board[row * _checkerboardSize + col] =
                    CreateFloorQuad(row, col, offset, _blackCellColor, _whiteCellColor);
            }
        }
    }

    public void DeleteBoard()
    {
        if (m_board == null) return;

        for (int i = m_board.Length - 1; i >= 0; i--)
        {
            Destroy(m_board[i]);
        }

        m_board = null;
    }

    private GameObject CreateFloorQuad(int _row, int _col, Vector3 _offset, Color _blackCellColor,
        Color _whiteCellColor)
    {
        var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.SetParent(m_chessBoardTransform);
        quad.layer = m_floorLayer;
        quad.transform.localScale = Vector3.one * 1.5f;
        quad.transform.localRotation = Quaternion.Euler(90, 0, 0);

        quad.transform.localPosition =
            new Vector3(_row * m_checkboardQuadSize, 0, _col * m_checkboardQuadSize) +
            _offset;

        bool isEvenRow = _row % 2 == 0;
        bool isEvenCol = _col % 2 == 0;
        bool isWhiteCell = isEvenRow ^ isEvenCol;
        quad.GetComponent<Renderer>().material.color =
            isWhiteCell ? _whiteCellColor : _blackCellColor;

        return quad;
    }
}
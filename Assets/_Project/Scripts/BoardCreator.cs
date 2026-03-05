using UnityEngine;

namespace Runtime
{
    public class BoardCreator : MonoBehaviour
    {
        private const float CHECKBOARD_QUAD_SIZE = 1.5f;

        [SerializeField] private Transform m_chessBoardTransform;

        private GameObject[] m_board;

        public void CreateBoard(int CheckerboardSize, Color BlackCellColor, Color WhiteCellColor)
        {
            DeleteBoard();

            float halfCheckBoardSize = -CheckerboardSize / 2f * CHECKBOARD_QUAD_SIZE;
            Vector3 offset = new Vector3(halfCheckBoardSize, 0, halfCheckBoardSize);

            m_board = new GameObject[CheckerboardSize * CheckerboardSize];
            for (int row = 0; row < CheckerboardSize; row++)
            {
                for (int col = 0; col < CheckerboardSize; col++)
                {
                    m_board[row * CheckerboardSize + col] =
                        CreateQuad(row, col, offset, BlackCellColor, WhiteCellColor);
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

        private GameObject CreateQuad(int _row, int _col, Vector3 _offset, Color _BlackCellColor, Color _WhiteCellColor)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.SetParent(m_chessBoardTransform);
            quad.transform.localScale = Vector3.one * 1.5f;
            quad.transform.localRotation = Quaternion.Euler(90, 0, 0);

            quad.transform.localPosition =
                new Vector3(_row * CHECKBOARD_QUAD_SIZE, 0, _col * CHECKBOARD_QUAD_SIZE) + _offset;

            bool isEvenRow = _row % 2 == 0;
            bool isEvenCol = _col % 2 == 0;
            bool isWhiteCell = isEvenRow ^ isEvenCol;
            quad.GetComponent<Renderer>().material.color =
                isWhiteCell ? _WhiteCellColor : _BlackCellColor;

            return quad;
        }
    }
}

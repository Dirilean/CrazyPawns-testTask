using CrazyPawn;
using UnityEngine;

namespace Runtime
{
    public class BoardCreator : MonoBehaviour
    {
        private const float CHECKBOARD_QUAD_SIZE = 1.5f;

        [SerializeField] private CrazyPawnSettings m_settings;
        [SerializeField] private Transform m_chessBoardTransform;

        private GameObject[] m_board;

        void Start()
        {
            CreateBoard();
        }

        public void CreateBoard()
        {
            DeleteBoard();

            float halfCheckBoardSize = -m_settings.CheckerboardSize / 2f * CHECKBOARD_QUAD_SIZE;
            Vector3 offset = new Vector3(halfCheckBoardSize, 0, halfCheckBoardSize);

            m_board = new GameObject[m_settings.CheckerboardSize * m_settings.CheckerboardSize];
            for (int row = 0; row < m_settings.CheckerboardSize; row++)
            {
                for (int col = 0; col < m_settings.CheckerboardSize; col++)
                {
                    m_board[row + col] = CreateQuad(row, col, offset);
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

        private GameObject CreateQuad(int _row, int _col, Vector3 _offset)
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
                isWhiteCell ? m_settings.WhiteCellColor : m_settings.BlackCellColor;

            return quad;
        }
    }
}

using CrazyPawn;
using EditorAttributes;
using UnityEngine;

namespace Runtime
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private CrazyPawnSettings m_settings;
        [SerializeField] private BoardCreator m_boardCreator;

        private void Start()
        {
            CreateBorder();
        }

        [Button("Create border")]
        public void CreateBorder()
        {
            m_boardCreator.CreateBoard(m_settings.CheckerboardSize, m_settings.BlackCellColor,
                m_settings.WhiteCellColor);
        }
    }
}
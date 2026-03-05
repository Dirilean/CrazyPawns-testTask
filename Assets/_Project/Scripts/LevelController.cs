using CrazyPawn;
using EditorAttributes;
using UnityEngine;

namespace Runtime
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private CrazyPawnSettings m_settings;
        [SerializeField] private BoardCreator m_boardCreator;
        [SerializeField] private PawnsSpawner m_pawnsSpawner;

        private void Start()
        {
            CreateBorder();
            CreatePawns();
        }

        [Button("Create border")]
        public void CreateBorder()
        {
            m_boardCreator.CreateBoard(m_settings.CheckerboardSize, m_settings.BlackCellColor,
                m_settings.WhiteCellColor);
        }

        [Button("Create pawns")]
        public void CreatePawns()
        {
            m_pawnsSpawner.CreatePawns(m_settings.InitialZoneRadius, m_settings.InitialPawnCount);
        }
    }
}
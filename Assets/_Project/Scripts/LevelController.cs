using CrazyPawn;
using EditorAttributes;
using Pawn;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CrazyPawnSettings m_settings;
    [SerializeField] private BoardCreator m_boardCreator;
    [SerializeField] private PawnsManager m_pawnsSpawner;

    private void Start()
    {
        CreateBorder();
        CreatePawns();
    }

    [Button("Create border")]
    private void CreateBorder()
    {
        m_boardCreator.CreateBoard(m_settings.CheckerboardSize, m_settings.BlackCellColor,
            m_settings.WhiteCellColor);
    }

    [Button("Create pawns")]
    private void CreatePawns()
    {
        m_pawnsSpawner.CreatePawns(m_settings.InitialZoneRadius, m_settings.InitialPawnCount, m_settings.ActiveConnectorMaterial, m_settings.DeleteMaterial);
    }
}
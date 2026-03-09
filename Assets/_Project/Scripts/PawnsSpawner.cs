using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class PawnsSpawner : MonoBehaviour
    {
        [SerializeField] private Pawn m_pawnPrefab;

        private Pawn[] m_pawns;
        private float m_lastRadius;

        public void CreatePawns(float _radius, int _count, Material _activeMat, Material _deleteMat)
        {
            m_lastRadius = _radius;
            DeletePawns();

            float maxRandomSquareDistance = _radius * _radius;
            m_pawns = new Pawn[_count];
            for (int i = 0; i < _count; i++)
            {
                float maxRandomDisanceSqrt = Random.Range(0f, maxRandomSquareDistance);
                float xSquarePos = Random.Range(0, 1f) * maxRandomDisanceSqrt;
                float xPos = Mathf.Sqrt(xSquarePos);
                float zPos = Mathf.Sqrt(maxRandomDisanceSqrt - xSquarePos);

                Vector2 quarter = GetRandomCircleQuarter();
                Vector3 randomPosInRadius = new Vector3(xPos * quarter.x, 0, zPos * quarter.y);

                m_pawns[i] = Instantiate(m_pawnPrefab, randomPosInRadius, Quaternion.identity, transform);
                m_pawns[i].Init(_activeMat, _deleteMat);
            }
        }

        public void DeletePawns()
        {
            if (m_pawns == null) return;

            for (int i = m_pawns.Length - 1; i >= 0; i--)
            {
                if (m_pawns[i] != null && m_pawns[i].gameObject != null)
                    Destroy(m_pawns[i].gameObject);
            }

            m_pawns = null;
        }

        private Vector2 GetRandomCircleQuarter()
        {
            int randomQuarter = Random.Range(0, 4);
            switch (randomQuarter)
            {
                case 0: return new Vector2(1, 1);
                case 1: return new Vector2(1, -1);
                case 2: return new Vector2(-1, -1);
                case 3: return new Vector2(-1, 1);
            }

            return Vector3.one;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_lastRadius == 0) return;

            Handles.DrawWireDisc(transform.position, transform.up, m_lastRadius);
        }
#endif
    }
}

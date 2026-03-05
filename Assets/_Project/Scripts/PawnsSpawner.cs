using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class PawnsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject pawnPrefab;

        private GameObject[] pawns;
        private float lastRadius;

        public void CreatePawns(float _radius, int _count)
        {
            lastRadius = _radius;
            DeletePawns();

            float maxRandomSquareDistance = _radius * _radius;
            pawns = new GameObject[_count];
            for (int i = 0; i < _count; i++)
            {
                float maxRandomDisanceSqrt = Random.Range(0f, maxRandomSquareDistance);
                float xSquarePos = Random.Range(0, 1f) * maxRandomDisanceSqrt;
                float xPos = Mathf.Sqrt(xSquarePos);
                float zPos = Mathf.Sqrt(maxRandomDisanceSqrt - xSquarePos);

                Vector2 quarter = GetRandomCircleQuarter();
                Vector3 randomPosInRadius = new Vector3(xPos * quarter.x, 0, zPos * quarter.y);

                pawns[i] = Instantiate(pawnPrefab, randomPosInRadius, Quaternion.identity, transform);
            }
        }

        public void DeletePawns()
        {
            if (pawns == null) return;

            for (int i = pawns.Length - 1; i >= 0; i--)
            {
                Destroy(pawns[i]);
            }

            pawns = null;
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
            if (lastRadius == 0) return;

            Handles.DrawWireDisc(transform.position, transform.up, lastRadius);
        }
#endif
    }
}

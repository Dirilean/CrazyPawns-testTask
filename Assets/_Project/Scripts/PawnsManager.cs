using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class PawnsManager : MonoBehaviour
    {
        [SerializeField] private Pawn m_pawnPrefab;
        [SerializeField] private ConnectionLine m_connectionLinePrefab;

        private List<Pawn> m_pawns = new();

        //Последний назначенный радиус для спавна пешек (сохраняем для отрисовки гизмо)
        private float m_lastRadius;

        //Текущая линия которая сейчас коннектится
        private ConnectionLine m_connectingLine;

        public void CreatePawns(float _radius, int _count, Material _activeMat, Material _deleteMat)
        {
            m_lastRadius = _radius;
            DeletePawns();

            float maxRandomSquareDistance = _radius * _radius;
            m_pawns.Clear();
            for (int i = 0; i < _count; i++)
            {
                float maxRandomDisanceSqrt = Random.Range(0f, maxRandomSquareDistance);
                float xSquarePos = Random.Range(0, 1f) * maxRandomDisanceSqrt;
                float xPos = Mathf.Sqrt(xSquarePos);
                float zPos = Mathf.Sqrt(maxRandomDisanceSqrt - xSquarePos);

                Vector2 quarter = GetRandomCircleQuarter();
                Vector3 randomPosInRadius = new Vector3(xPos * quarter.x, 0, zPos * quarter.y);

                m_pawns.Add(Instantiate(m_pawnPrefab, randomPosInRadius, Quaternion.identity, transform));
                m_pawns[i].Init(_activeMat, _deleteMat);
                m_pawns[i].OnDestroyAction += DeletePawnFromList;
                m_pawns[i].OnConnectorClickAction += ClickConnector;
            }
        }

        public void DeletePawns()
        {
            if (m_pawns == null) return;

            for (int i = m_pawns.Count - 1; i >= 0; i--)
            {
                if (m_pawns[i] != null && m_pawns[i].gameObject != null)
                    Destroy(m_pawns[i].gameObject);
            }

            m_pawns.Clear();
        }

        private void DeletePawnFromList(Pawn _pawn)
        {
            m_pawns.Remove(_pawn);
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

        public void ClickConnector(Pawn _pawn, PawnConnector _connector)
        {
            //Начинаем коннект
            if (m_connectingLine == null)
            {
                //Выделяем все коннекторы пешек кроме текущей
                for (int i = 0; i < m_pawns.Count; i++)
                {
                    if (m_pawns[i] == _pawn) continue;
                    m_pawns[i].SetActiveState();
                }

                _pawn.SetNormalState();

                m_connectingLine = Instantiate(m_connectionLinePrefab, transform);
                m_connectingLine.SetStart(_connector);
            }
            else //Заканчиваем коннект
            {
                if (_connector.m_allowConnect)
                {
                    //Снимаем выделение
                    for (int i = 0; i < m_pawns.Count; i++)
                    {
                        m_pawns[i].SetNormalState();
                    }

                    //Заканчиваем коннект
                    m_connectingLine.SetEnd(_connector);
                }
                else
                {
                    //Удаляем так как никуда не приконнектились
                    Destroy(m_connectingLine.gameObject);
                    m_connectingLine = null;
                }
            }
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

using System.Collections.Generic;
using Clickable;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class PawnsManager : MonoBehaviour, IClickDown, IClickDragEnd
    {
        [SerializeField] private Pawn m_pawnPrefab;
        [SerializeField] private ConnectionLine m_connectionLinePrefab;

        private List<Pawn> m_pawns = new();

        //Последний назначенный радиус для спавна пешек (сохраняем для отрисовки гизмо)
        private float m_lastRadius;

        //Текущая линия которая сейчас коннектится
        private ConnectionLine m_connectingLine;

        private List<ConnectionLine> m_connectingLines = new();

        //Ждем окончания коннекта линии (стартовая точка уже есть)
        private bool isWaitToEndConnecting = false;
        
        public void CreatePawns(float _radius, int _count, Material _activeMat, Material _deleteMat)
        {
            m_lastRadius = _radius;
            DeletePawnsAndConnectionsLines();

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
                m_pawns[i].m_onDestroyAction += DeletePawn;
                m_pawns[i].m_onConnectorClickAction += ClickConnector;
            }
        }

        public void DeletePawnsAndConnectionsLines()
        {
            if (m_pawns == null) return;

            for (int i = m_pawns.Count - 1; i >= 0; i--)
            {
                if (m_pawns[i] != null && m_pawns[i].gameObject != null)
                    Destroy(m_pawns[i].gameObject);
            }

            m_pawns.Clear();

            for (int i = m_connectingLines.Count - 1; i >= 0; i--)
            {
                if (m_connectingLines[i] != null && m_connectingLines[i].gameObject != null)
                    Destroy(m_connectingLines[i].gameObject);
            }

            m_connectingLines.Clear();
        }

        private void DeletePawn(Pawn _pawn)
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

                isWaitToEndConnecting = true;
            }
            else if (_connector!=null && _connector.m_allowConnect) //Заканчиваем коннект
            {
                SetPawnsNormalState();

                //Заканчиваем коннект
                m_connectingLine.SetEnd(_connector);

                m_connectingLines.Add(m_connectingLine);
                m_connectingLine = null;
                isWaitToEndConnecting = false;
            }
        }

        private void TryResetUnsuccesfulConnecting()
        {
            if (!isWaitToEndConnecting) return;
            
            isWaitToEndConnecting = false;
            SetPawnsNormalState();
            DeleteNotConnectionLine();
        }

        private void SetPawnsNormalState()
        {
            for (int i = 0; i < m_pawns.Count; i++)
            {
                m_pawns[i].SetNormalState();
            }
        }

        private void DeleteNotConnectionLine()
        {
            if (m_connectingLine != null)
                Destroy(m_connectingLine.gameObject);

            m_connectingLine = null;
            isWaitToEndConnecting = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_lastRadius == 0) return;

            Handles.DrawWireDisc(transform.position, transform.up, m_lastRadius);
        }
#endif
        
        //Действия по мисклику
        public void OnClickDown()
        {
            TryResetUnsuccesfulConnecting();
        }

        //Действия по мисклику
        public void OnClickDragEnd()
        {
            TryResetUnsuccesfulConnecting();
        }
    }
}

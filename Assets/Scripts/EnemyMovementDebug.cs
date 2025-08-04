using UnityEditor;
using UnityEngine;

public class EnemyMovementDebug : MonoBehaviour
{
    [SerializeField] private bool _showDebugInfo = true;
    [SerializeField] private bool _showRaycast = false;

    private FollowTargetStrategy _strategy;

    private void Start()
    {
        // Récupérer la stratégie (adapter selon ton implémentation)
        _strategy = GetComponent<EnemyGO>()?.MovementStrategy as FollowTargetStrategy;
    }

    private void OnDrawGizmos()
    {
        if (!_showRaycast || _strategy == null) return;

        // Dessiner les raycast de détection
        Vector2 currentPos = transform.position;

        if (_strategy.Target != null)
        {
            Vector2 targetPos = _strategy.Target.position;
            Vector2 dirToTarget = (targetPos - currentPos).normalized;

            // Rayon vers la cible
            Gizmos.color = Color.green;
            Gizmos.DrawLine(currentPos, targetPos);

            // Rayons de détection d'obstacles
            Gizmos.color = Color.yellow;
            float baseAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;

            for (int i = 0; i < 7; i++) // Adapter selon _raycastCount
            {
                float angle = baseAngle + (i - 3) * 15f; // 15° entre chaque rayon
                Vector2 rayDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector2 rayEnd = currentPos + rayDir * 3f; // Adapter selon _raycastDistance

                Gizmos.DrawLine(currentPos, rayEnd);
            }
        }
    }

    private void OnGUI()
    {
        if (!_showDebugInfo) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.BeginVertical("box");

        GUILayout.Label("Enemy Movement Debug", EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).label);

        if (EnemyMovementManager.Instance != null)
        {
            EnemyMovementManager.Instance.GetDebugInfo(out int total, out int queued, out int current);

            GUILayout.Label($"Total Enemies: {total}");
            GUILayout.Label($"Queued Calculations: {queued}");
            GUILayout.Label($"Current Frame Calcs: {current}");
            GUILayout.Label($"FPS: {(int)(1f / Time.unscaledDeltaTime)}");
        }

        if (_strategy != null)
        {
            GUILayout.Space(10);
            GUILayout.Label("This Enemy:", EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).label);
            GUILayout.Label($"Has Target: {_strategy.Target != null}");

            if (_strategy.Target != null)
            {
                float distance = Vector2.Distance(transform.position, _strategy.Target.position);
                GUILayout.Label($"Distance to Target: {distance:F1}");
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

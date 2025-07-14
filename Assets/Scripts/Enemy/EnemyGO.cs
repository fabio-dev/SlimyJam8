using Assets.Scripts.Domain;
using UnityEngine;

public class EnemyGO : MonoBehaviour
{
	public Enemy Enemy { get; private set; }

	public void SetEnemy(Enemy enemy)
	{
		Enemy = enemy;
	}
}

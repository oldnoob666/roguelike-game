using UnityEngine;

public class EnemyPatrolState : EnemyStateBase
{
    private Vector2 patrolPoint;
    private float waitUntil;

    public EnemyPatrolState(EnemyController enemy) : base(enemy) { }

    public override void OnEnter()
    {
        patrolPoint = enemy.HomePosition;
        waitUntil = Time.time;
    }

    public override void LogicUpdate()
    {
        if (enemy.FoundPlayer)
        {
            enemy.SwitchState(EnemyController.EnemyState.Chase);
            return;
        }

        if (Vector2.Distance(enemy.Body.position, patrolPoint) < 0.15f && Time.time >= waitUntil)
        {
            patrolPoint = enemy.HomePosition + Random.insideUnitCircle * enemy.PatrolRadius;
            waitUntil = Time.time + enemy.WaitTime;
        }
    }

    public override void PhysicsUpdate()
    {
        enemy.MoveTowards(patrolPoint, enemy.PatrolSpeed, 0.15f, Time.time < waitUntil);
    }

    public override void OnExit()
    {
        enemy.SetAnimationBool("walk", false);
    }
}

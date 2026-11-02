public class EnemyChaseState : EnemyStateBase
{
    public EnemyChaseState(EnemyController enemy) : base(enemy) { }

    public override void LogicUpdate()
    {
        // 离开检测范围后，仍会向最后看到玩家的位置追击一段时间。
        if (!enemy.FoundPlayer && !enemy.RemembersPlayer)
            enemy.SwitchState(EnemyController.EnemyState.Patrol);
    }

    public override void PhysicsUpdate()
    {
        enemy.MoveTowards(enemy.LastSeenPosition, enemy.ChaseSpeed, enemy.StoppingDistance);
    }

    public override void OnExit()
    {
        enemy.SetAnimationBool("run", false);
    }
}

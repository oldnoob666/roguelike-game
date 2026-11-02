// 普通 C# 类，不是组件。每个具体状态只实现自己需要的阶段。
public abstract class EnemyStateBase
{
    protected readonly EnemyController enemy;

    protected EnemyStateBase(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    // 进入状态时执行一次。
    public virtual void OnEnter() { }

    // 由 EnemyController.Update 每帧调用。
    public virtual void LogicUpdate() { }

    // 由 EnemyController.FixedUpdate 在物理更新时调用。
    public virtual void PhysicsUpdate() { }

    // 切换到其他状态前执行一次。
    public virtual void OnExit() { }
}

using System;
using UnityEngine;

// 玩家和敌人共用的血量组件；只负责扣血和发出通知，不负责移动或动画。
public class CharacterHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 100;//最大血量100
    [SerializeField, Min(0f)] private float invulnerableDuration = 0.5f;//无敌时间

    public int CurrentHealth { get; private set; }//当前血量，外部只读（get）不能改数（private set）
    // 补充：MaxHealth 是只读属性；=> maxHealth 表示读取时返回 maxHealth，外部不能通过该属性赋值。
    public int MaxHealth => maxHealth;//？？
    public bool IsDead => CurrentHealth <= 0;//做一个判断，如果当前血量小于等于0，返回true，否则返回false

    public event Action<Vector2> Damaged;//受伤事件，传递一个Vector2参数，表示受伤的方向
    public event Action Died;//死亡事件

    // 补充：保存下一次允许受伤的时间点，不是剩余无敌秒数。
    private float nextDamageTime;//用来判断无敌时间的数（不对）

    private void Awake()
    {
        CurrentHealth = maxHealth;//初始化当前血量为最大血量
    }

    // hitDirection 是从攻击来源指向受击者的方向。返回值表示这次是否成功扣血。
    public bool TakeDamage(int damage, Vector2 hitDirection, bool ignoreInvulnerability = false)
    {
        // 补充：ignoreInvulnerability是bool，表示已死亡、伤害不大于0，或本次不忽略无敌且仍在无敌时间内时，拒绝伤害；ignoreInvulnerability 默认 false，传 true 只跳过无敌判断。
        if (IsDead || damage <= 0 || (!ignoreInvulnerability && Time.time < nextDamageTime))//当死亡，伤害数字小于等于0，无敌，？（ignoreInvulnerability 是什么？）时，不能造成伤害
            return false;

        // 补充：先扣除伤害，再取结果与0中的较大值，保证扣血后的血量不低于0。
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);//当前血量<0时，选择0
        // 补充：当前时间加无敌持续时间，得到无敌结束的时间点；即使本次忽略无敌，扣血后也会更新它。
        nextDamageTime = Time.time + invulnerableDuration;//计算无敌

        if (IsDead)
            Died?.Invoke();//发送死亡事件
        else
            Damaged?.Invoke(hitDirection.normalized);//附带Vector2的受伤事件

        return true;//返回造成伤害成功
    }
}

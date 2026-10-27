using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(CharacterHealth))]//？？
public class PlayerHealth : MonoBehaviour//挂载在player身上，负责死亡与ui管理
{
    private CharacterHealth health;
    [SerializeField] private TMP_Text healthText;

    public bool IsAlive => health != null && !health.IsDead;//**每次读取该属性时判断：如果health不为空或者health里的Isdead布尔值为false，返回true

    private void Awake()
    {
        health = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        health.Died += OnDead;//把Ondead方法绑定到Died事件，死亡时触发
    }

    private void OnDisable()
    {
        health.Died -= OnDead;//退订
    }

    private void OnDead()
    {
        
        var player = GetComponent<Player>();
        if (player != null) player.enabled = false;//死亡后关闭移动组件

        var shooting = GetComponent<SimplePlayerShooting>();
        if (shooting != null) shooting.enabled = false;//关闭射击组件

        var body = GetComponent<Rigidbody2D>();
        if (body != null) body.velocity = Vector2.zero;//设置纵向，横向速度为0，立刻停止移动
    }

    private void Update()
    {

        if (healthText != null)
        {
            healthText.text = $"HP :{health.CurrentHealth} /{ health.MaxHealth}";
        }
        if(health.CurrentHealth < 30) healthText.color = Color.red;

        if (health.IsDead && Input.GetKeyDown(KeyCode.R))//玩家已经死亡并且按下R
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//等价于Scene currentScene = SceneManager.GetActiveScene();
                                                                             //int sceneIndex = currentScene.buildIndex;
                                                                             //SceneManager.LoadScene(sceneIndex);
    }

    private void OnGUI()//将角色血量在ui上简易显示
    {
        
        if (health.IsDead)
            GUI.Box(new Rect(Screen.width / 2f - 130, Screen.height / 2f - 25, 260, 50),
                "Game Over\nPress R to restart");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int coins;
    private int life;
    private int deathCount;
    private int newHp = 10;
    public GameObject PlayerPrefab;
    private GameObject player;

    private static GameManager instance;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null) instance = this;


        this.coins = 0;
        this.life = 5;

        Init();
    }

    void FixedUpdate()
    {
        if(coins >= 100)
        {
            coins -= 100;
            life += 1;
        }

        if (player == null) PlayerKilled();
    }

    void Init()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "EndingScene") return;
        player = Instantiate(PlayerPrefab, this.transform.position, this.transform.rotation);
        player.name = "Player";
        player.GetComponent<PlayerManager>().SetHp(this.newHp);
    }

    void PlayerKilled()
    {
        if (this.life > 0)
        {
            life -= 1;
            GameObject.Find("Main Camera").transform.position = new Vector3(0, 0, -10);
            Init();
        }

        //SceneChange("EndingScene");
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        this.newHp =  PlayerManager.Instance.GetHp();
    }

    public void AddCoins(int coins) { this.coins += coins; }
    public int GetCoins() { return this.coins; }

    public void AddLife(int life) { this.life += life; }
    public int GetLife() { return this.life; }

    public static GameManager Instance
    {
        get { return instance; }
    }

    public int GetDeathCount() { return this.deathCount; }
    public void AddDeathCount() { this.deathCount += 1; }
}

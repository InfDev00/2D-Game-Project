using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int coins;
    private int life;
    private int deathCount;
    public GameObject PlayerPrefab;
    private GameObject player;

    private static GameManager instance;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null) instance = this;


        this.coins = 0;
        this.life = 0;

        Init();
    }

    void FixedUpdate()
    {
        if(coins >= 100)
        {
            coins -= 100;
            life += 1;
        }

        if(player==null)Init();
    }

    void Init()
    {
        player = Instantiate(PlayerPrefab, this.transform.position, this.transform.rotation);
        player.name = "Player";
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

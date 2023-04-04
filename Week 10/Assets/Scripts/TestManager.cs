using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public delegate void EnemyLoaded();
    public EnemyLoaded enemyLoaded;

    public static TestManager instance;

    bool isLoaded = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Random.value < 0.01f && isLoaded == false)
        {
            LoadData();
        }
    }

    void LoadData()
    {
        isLoaded = true;

        TextAsset txtAsset = Resources.Load<TextAsset>("enemyData/enemyData1");
        string textContent = txtAsset.text;
        print(textContent);

        string[] data = textContent.Split("##");
        EnemyBase enemy = new EnemyBase();
        enemy.name = data[0];
        enemy.hp = int.Parse(data[1]);
        enemy.atk = int.Parse(data[2]);
        enemy.def = int.Parse(data[3]);

        print("name: " + enemy.name);
        print("hp: " + enemy.hp);
        print("atk: " + enemy.atk);
        print("def: " + enemy.def);

        enemyLoaded?.Invoke();  // first check if it is not null;
    }

    public class EnemyBase
    {
        public string name;
        public int hp;
        public int atk;
        public int def;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    float gateTimer = 0;
    float gateTimerTotal = 1;

    public static GameManager instance;

    [SerializeField]
    Transform playerTarget;
    [SerializeField]
    Transform playerGroup;
    [SerializeField]
    playerUnit playerUnit_Prefab;
    [SerializeField]
    GateController gate_Prefab;
    [SerializeField]
    TextMeshPro tx_PlayerNum;

    [HideInInspector]
    public string[] abilityName = { "+", "-", "x", "÷" };

    public struct Ability
    {
        public int abilityID;
        public int abilityPower;
        public Ability(int _abilityID, int _abilityPower)
        {
            abilityID = _abilityID;
            abilityPower = _abilityPower;
        }
    }


    int visualCount = 0;

    private int _playerNum;
    private int PlayerNum
    {
        get
        {
            return _playerNum;
        }
        set
        {
            _playerNum = Mathf.Clamp(value, 0, 500);
        }
    }

    List<playerUnit> playerUnitList = new List<playerUnit>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateUnit(10);
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        playerTarget.Translate(Vector3.right * horizontal * 10 * Time.deltaTime);

        if (playerTarget.position.x < -5)
        {
            playerTarget.position = new Vector3(-5, playerTarget.position.y, playerTarget.position.z);
        }
        else if (playerTarget.position.x > 5)
        {
            playerTarget.position = new Vector3(5, playerTarget.position.y, playerTarget.position.z);
        }

        if (gateTimer > gateTimerTotal)
        {
            gateTimer = 0;
            gateTimerTotal = 0.5f + Random.value * 3;

            GateController gate = Instantiate(gate_Prefab);

            gate.Position = new Vector3(0, 0, 25);
        }
        else
        {
            gateTimer += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        tx_PlayerNum.text = PlayerNum.ToString();
    }

    void CreateUnit(int num)
    {
        PlayerNum += num;
        visualCount += num;

        for (int i = 0; i < num; i++)
        {
            playerUnit unit = Instantiate(playerUnit_Prefab, playerGroup);
            unit.Position = playerTarget.position + new Vector3(Random.value, Random.value, Random.value);
            playerUnitList.Add(unit);
        }
    }

    void RemoveUnit(int num, bool visual) {
        if (!visual) {
            PlayerNum -= num;
            if (PlayerNum < 1)
            {
                PlayerNum = 1;
            }
        }
        visualCount -= num;

        if (visualCount < 1)
        {
            visualCount = 1;
        }

        for (int i = 0; i < num; i++)
        {
            if (playerUnitList.Count > 0)
            {
                playerUnit unit = playerUnitList[Random.Range(0, playerUnitList.Count)];

                playerUnitList.Remove(unit);
                Destroy(unit.gameObject);
            }
            else
            {
                break;
            }
        }
    }

    public void ApplyGateAbility(Ability ability)
    {
        if (ability.abilityID == 0)
        {
            CreateUnit(ability.abilityPower);
        }
        else if (ability.abilityID == 1)
        {
            RemoveUnit(ability.abilityPower, false);
        }
        else if (ability.abilityID == 2)
        {
            CreateUnit(PlayerNum * (ability.abilityPower - 1));
        }
        else if (ability.abilityID == 3)
        {
            RemoveUnit(PlayerNum / ability.abilityPower * ability.abilityPower - 1, false);
        }

        if (PlayerNum > 500)
        {
            PlayerNum = 500;
        }

        if (visualCount > 100) {
            RemoveUnit(visualCount - 100, true);
            visualCount = 100;
        }
    }
}

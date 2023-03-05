using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour
{
    [SerializeField]
    float gateSpeed = -10;
    [SerializeField]
    TextMeshPro[] tx_Gates;

    bool isGateTriggered = false;

    GameManager.Ability[] gateAbilities;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    void Start()
    {
        gateAbilities = new GameManager.Ability[]
        {
            new GameManager.Ability(Random.Range(0, 4), Random.Range(1, 10)),
            new GameManager.Ability(Random.Range(0, 4), Random.Range(1, 10)),
        };

        for (int i = 0; i < tx_Gates.Length; i++)
        {
            tx_Gates[i].text = GameManager.instance.abilityName[gateAbilities[i].abilityID] + gateAbilities[i].abilityPower;
        }
    }

    void Update()
    {
        Move();
        if (Position.z < -20) {
            Remove();
        }
    }

    public void GateTriggered(int gateID)
    {
        if (!isGateTriggered)
        {
            isGateTriggered = true;

            GameManager.instance.ApplyGateAbility(gateAbilities[gateID]);
        }
    }

    public void Move()
    {
        transform.Translate(Vector3.forward * gateSpeed * Time.deltaTime);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}

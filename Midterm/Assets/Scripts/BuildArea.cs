using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildArea : MonoBehaviour
{
    public enum AreaState {Empty, Built}
    AreaState areaState = AreaState.Empty;

    [SerializeField]
    GameObject emptyTower;
    
    GameObject currentTower;

    [HideInInspector]
    public TowerBase currentTowerBase;

    Transform buildTransform;

    [SerializeField]
    GameObject BuildTextGroup;
    [SerializeField]
    TextMeshProUGUI NameText;
    [SerializeField]
    TextMeshProUGUI BuildOneButton;
    [SerializeField]
    TextMeshProUGUI BuildTwoButton;
    [SerializeField]
    TextMeshProUGUI SellButton;

    void Start()
    {
        Setup();
    }

    void Setup() {
        buildTransform = transform.Find("Build Transform").transform;

        if (areaState == AreaState.Empty)
        {
            BuildTower(emptyTower);
        }

        BuildTextGroup.SetActive(false);
    }

    void OnMouseDown()
    {
        SelectThisArea();
    }

    void SelectThisArea()
    {
        BuildTextGroup.SetActive(true);

        GameManager.instance.currentBuildArea = this;

        if (currentTower != null)
        {
            NameText.text = currentTowerBase.towerName;

            if (currentTowerBase.upgradeTower[0] != null)
            {
                TowerBase firstTower = currentTowerBase.upgradeTower[0].GetComponent<TowerBase>();
                BuildOneButton.text = firstTower.name + " ( cost " + firstTower.cost + " )";
            }
            else
            {
                BuildOneButton.text = "";
            }
            if (currentTowerBase.upgradeTower[1] != null)
            {
                TowerBase secondTower = currentTowerBase.upgradeTower[1].GetComponent<TowerBase>();
                BuildTwoButton.text = secondTower.name + " ( cost " + secondTower.cost + " )";
            }
            else
            {
                BuildTwoButton.text = "";
            }

            if (currentTowerBase.towerName == "Buildable Area")
            {
                SellButton.text = "";
            }
            else
            {
                SellButton.text = "Sell ( " + currentTowerBase.cost / 2 + " )";
            }
        }
    }
    
    public void BuildTower(GameObject tower)
    {
        if (GameManager.instance.CostMoney(tower.GetComponent<TowerBase>().cost))
        {
            RemoveTower();
            currentTower = Instantiate(tower, buildTransform.position, Quaternion.identity);
            currentTowerBase = currentTower.GetComponent<TowerBase>();

            SoundManager.instance.PlaySound(currentTowerBase.buildSound);

            BuildTextGroup.SetActive(false);
            if (tower != emptyTower) {
                areaState = AreaState.Built;
            }
        }
    }

    public void SellTower()
    {
        GameManager.instance.AddMoney(currentTowerBase.cost / 2);
        RemoveTower();
        BuildTower(emptyTower);
    }

    public void RemoveTower()
    {
        if (currentTower != null)
        {
            Destroy(currentTower.gameObject);
            currentTower = null;
            currentTowerBase = null;

            areaState = AreaState.Empty;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

	public Color hoverColor;
	public Color notEnoughMoneyColor;
	public Vector3 positionOffset;

	[HideInInspector]
	public GameObject turret;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
	public bool isUpgraded = false;

	private Renderer rend;

	private Color startColor;

	BuildManager buildManager;
	private void Start()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;

		buildManager = BuildManager.instance;
	}

	public Vector3 GetBuildPosition()
	{
		return transform.position + positionOffset;
	}


	private void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		if(turret != null)
		{
			buildManager.SelectNode(this);
			return;
		}


		if (!buildManager.CanBuild)
		{
			return;
		}

		BuildTurret(buildManager.GetTurretToBuild());

	}

	void BuildTurret(TurretBlueprint blueprint)
	{
		if (PlayerStats.Money < blueprint.cost)
		{
			Debug.Log("Not Enough muni bitc");
			return;
		}

		PlayerStats.Money -= blueprint.cost;

		turretBlueprint = blueprint;

		GameObject _turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);
	}

	public void UpgradeTurret()
	{
		if (PlayerStats.Money < turretBlueprint.upgradeCost)
		{
			Debug.Log("Not Enough muni bitc to upgrade");
			return;
		}

		PlayerStats.Money -= turretBlueprint.upgradeCost;

		//get rid of the old turret
		Destroy(turret);

		//building an upgraded turret
		GameObject _turret = Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;



		GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity); //todo: add upgrade effect!
		Destroy(effect, 5f);

		isUpgraded = true;
		
	}

	public void SellTurret()
	{
		PlayerStats.Money += turretBlueprint.GetSellAmount();


		//spawn a cool effect:
		GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Destroy(turret);
		turretBlueprint = null;
	}

	private void OnMouseEnter()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		if (!buildManager.CanBuild)
		{
			return;
		}

		if(buildManager.HasMoney)
		{
			rend.material.color = hoverColor;

		} else
		{
			rend.material.color = notEnoughMoneyColor;

		}

	}
	private void OnMouseExit()
	{
		rend.material.color = startColor;
	}
}

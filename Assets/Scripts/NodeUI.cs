using UnityEngine;
using UnityEngine.UI;
public class NodeUI : MonoBehaviour
{
	public GameObject nodeUICanvas;
	public Text upgradeCost;
	private Node target;
	public Button upgradeButton;

	public Text sellAmount;


	public void SetTarget(Node target)
	{
		this.target = target;

		transform.position = target.GetBuildPosition();

		if(!target.isUpgraded)
		{
			upgradeCost.text = "$"+ target.turretBlueprint.upgradeCost;
			upgradeButton.interactable = true;

		} else
		{
			upgradeCost.text = "DONE";
			upgradeButton.interactable = false;
		}

		sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

		nodeUICanvas.SetActive(true);
	}

	public void Hide()
	{
		nodeUICanvas.SetActive(false);
	}


	public void Upgrade()
	{
		target.UpgradeTurret();
		BuildManager.instance.DeselectNode();
	}

	public void Sell()
	{
		target.SellTurret();
		BuildManager.instance.DeselectNode();
		target.isUpgraded = false;
	}
}

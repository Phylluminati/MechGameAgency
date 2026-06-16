using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	[SerializeField]
	private Image _hullPointBar;
	private float _targetH;
    private float _targetA;
    private float _targetS;
	[SerializeField]
	private int _reduceSpeed;

	[SerializeField]
	private Image _shieldPointBar;

    [SerializeField]
    private Image _armorPointBar;
	void Start()
	{
		//fix method with proper currentHP this one is currently a dud
		/*UpdateHull(this.GetComponent<UnitTracker>().);
		if (_shieldPointBar != null)
		{
			UpdateShield(this.GetComponent<UnitTracker>().);
		}
        if(_armorPointBar != null)
        {

        }*/
	}
    public void UpdateArmor(float currentAP)
    {
        int maxArmor = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[1];
        _targetA = currentAP / (float)maxArmor;
		//_armorPointBar.fillAmount = currentAP / (float)maxArmor;
    }

	public void UpdateHull(float currentHealth)
	{
		int maxHull = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[0];
		_targetH = currentHealth / (float)maxHull;
		//_hullPointBar.fillAmount = currentHealth / (float)maxHull;
	}

    public void UpdateShield(float currentS)
	{
		int maxShield = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[1];
		//_shieldPointBar.fillAmount = currentMP / maxShield;
        _targetS = currentS / (float)maxShield;
		//_shieldPointBar.fillAmount = currentS / (float)maxShield;

	}

	void Update()
	{
		_hullPointBar.fillAmount = Mathf.MoveTowards(_hullPointBar.fillAmount, _targetH, _reduceSpeed * Time.deltaTime);
        _shieldPointBar.fillAmount = Mathf.MoveTowards(_shieldPointBar.fillAmount, _targetS, _reduceSpeed * Time.deltaTime);
        _armorPointBar.fillAmount = Mathf.MoveTowards(_armorPointBar.fillAmount, _targetA, _reduceSpeed * Time.deltaTime);

	}

	public void FullUpdateCheck()
	{
		UpdateHull(this.GetComponent<UnitTracker>().healthBarCurrentValues[0]);
		UpdateArmor(this.GetComponent<UnitTracker>().healthBarCurrentValues[1]);
		UpdateShield(this.GetComponent<UnitTracker>().healthBarCurrentValues[2]);
		print("HP Current values" + this.GetComponent<UnitTracker>().healthBarCurrentValues[0] + " " + this.GetComponent<UnitTracker>().healthBarCurrentValues[1] + " " + this.GetComponent<UnitTracker>().healthBarCurrentValues[2]);
		//print(_targetA + _targetH + _targetS);
	}

}

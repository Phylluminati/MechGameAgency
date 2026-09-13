
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

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
	GameObject HpText;
	int HTextTarget;
	int ATextTarget;
	int STextTarget;
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
		//HpText = .transform.GetChild(2);
	}
    public void UpdateArmor(float currentAP)
    {
        int maxArmor = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[1];
        _targetA = currentAP / (float)maxArmor;
		//ATextTarget = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[1];
		ATextTarget = (int)currentAP;
		//_armorPointBar.fillAmount = currentAP / (float)maxArmor;
    }

	public void UpdateHull(float currentHealth)
	{
		int maxHull = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[0];
		_targetH = currentHealth / (float)maxHull;
		//HTextTarget = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[0];
		HTextTarget = (int)currentHealth;
		//_hullPointBar.fillAmount = currentHealth / (float)maxHull;
	}

    public void UpdateShield(float currentS)
	{
		int maxShield = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[2];
		//_shieldPointBar.fillAmount = currentMP / maxShield;
        _targetS = currentS / (float)maxShield;
		//STextTarget = this.GetComponent<UnitTracker>().unitData.healthBarMaxValues[2];
		STextTarget = (int)currentS;
		//_shieldPointBar.fillAmount = currentS / (float)maxShield;

	}

	void Update()
	{
		_hullPointBar.fillAmount = Mathf.MoveTowards(_hullPointBar.fillAmount, _targetH, _reduceSpeed * Time.deltaTime);
        _shieldPointBar.fillAmount = Mathf.MoveTowards(_shieldPointBar.fillAmount, _targetS, _reduceSpeed * Time.deltaTime);
        _armorPointBar.fillAmount = Mathf.MoveTowards(_armorPointBar.fillAmount, _targetA, _reduceSpeed * Time.deltaTime);
		//_hullPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = ToString(Mathf.MoveTowards(float.Parse(_hullPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text), (float)HTextTarget, _reduceSpeed * Time.deltaTime));
		if(float.Parse(_hullPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) > HTextTarget)
		{

			float tempNumber = float.Parse(_hullPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) - 1;
			_hullPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = tempNumber.ToString("0000");
		}
		else if(float.Parse(_hullPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) < HTextTarget)
		{
			_hullPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = HTextTarget.ToString("0000");
		}
		if(float.Parse(_armorPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) > ATextTarget)
		{

			float tempNumber = float.Parse(_armorPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) - 1;
			_armorPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = tempNumber.ToString("00000");
		}
		else if(float.Parse(_armorPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) < ATextTarget)
		{
			_armorPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = ATextTarget.ToString("0000");
		}
		if(float.Parse(_shieldPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) > STextTarget)
		{

			float tempNumber = float.Parse(_shieldPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) - 1;
			_shieldPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = tempNumber.ToString("00000");
		}
		else if(float.Parse(_shieldPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text) < STextTarget)
		{
			_shieldPointBar.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = STextTarget.ToString("0000");
		}
	}

	public void MainHealthbar(int activeHealthbar)
	{
		switch(activeHealthbar)
		{
			case 0:
				_armorPointBar.gameObject.transform.localScale = new Vector3(1,2,1);
				_shieldPointBar.gameObject.transform.localScale = new Vector3(1,1,1);
			break;	
			case 1:
				_armorPointBar.gameObject.transform.localScale = new Vector3(1,1,1);
				_shieldPointBar.gameObject.transform.localScale = new Vector3(1,2,1);
			break;
		}
	}

	public void FullUpdateCheck()
	{
		UpdateHull(this.GetComponent<UnitTracker>().healthBarCurrentValues[0]);
		UpdateArmor(this.GetComponent<UnitTracker>().healthBarCurrentValues[1]);
		UpdateShield(this.GetComponent<UnitTracker>().healthBarCurrentValues[2]);
		//print("HP Current values" + this.GetComponent<UnitTracker>().healthBarCurrentValues[0] + " " + this.GetComponent<UnitTracker>().healthBarCurrentValues[1] + " " + this.GetComponent<UnitTracker>().healthBarCurrentValues[2]);
		//print(_targetA + _targetH + _targetS);
	}

}

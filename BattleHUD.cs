using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text hpAmount;
    public Text mpAmount;
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    public Slider mpSlider;

    public void SetHUD(Unit unit)
    {
        hpAmount.text = "HP: " + unit.currentHP + "/" + unit.maxHP;
        mpAmount.text = "MP: " + unit.currentMP + "/" + unit.maxMP;
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        mpSlider.maxValue = unit.maxMP;
        mpSlider.value = unit.currentMP;
    }

    public void SetPlayerHUD(Unit unit)
    {
        hpAmount.text = "HP: " + unit.playerCurrentHP + "/" + unit.playerHP;
        mpAmount.text = "MP: " + unit.playerCurrentMP + "/" + unit.playerMP;
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.playerLvl;
        hpSlider.maxValue = unit.playerHP;
        hpSlider.value = unit.playerCurrentHP;
        mpSlider.maxValue = unit.playerMP;
        mpSlider.value = unit.playerCurrentMP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetMP(int mp)
    {
        mpSlider.value = mp;
    }
}

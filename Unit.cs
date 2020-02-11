using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    // player stats
    public int playerLvl;
    public int playerCurrentHP;
    public int playerHP;
    public int playerCurrentMP;
    public int playerMP;
    public int playerDMG;

    // data about characters
    public string unitName;
    public int unitLevel;
    public int experience;

    // regular attack dmg
    public int damage;

    // dmg spells
    public int dmgSpellCost;
    public int spellDamage;

    // heal spells
    public int healSpellCost;

    // hp
    public int maxHP;
    public int currentHP;

    // mana
    public int maxMP;
    public int currentMP;

    void Awake()
    {
        Stats();
    }

    public bool TakeDamage(int dmg)
    {
        int randomDmg = Random.Range(1, 5);
        dmg = dmg + 2 - randomDmg;

        currentHP -= dmg;

        if(currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TakeDamagePlayer(int dmg)
    {
        int randomDmg = Random.Range(1, 5);
        dmg = dmg + 2 - randomDmg;

        playerCurrentHP -= dmg;

        if (playerCurrentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TakeMana(int amount)
    {
        currentMP -= amount;

        if (currentMP < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TakeManaPlayer(int amount)
    {
        playerCurrentMP -= amount;

        if (playerCurrentMP < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Rest(int amountHP, int amountMP)
    {
        playerCurrentHP += amountHP;
        if(playerCurrentHP > playerHP)
        {
            playerCurrentHP = playerHP;
        }

        playerCurrentMP += amountMP;
        if(playerCurrentMP > playerMP)
        {
            playerCurrentMP = playerMP;
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void PlayerHeal(int amount)
    {
        playerCurrentHP += amount;
        if (playerCurrentHP > playerHP)
        {
            playerCurrentHP = playerHP;
        }
    }

    public void ExpGained(int amount)
    {
        experience += amount;

        int curLvl = (int)(0.1f * Mathf.Sqrt(experience));

        if (curLvl != playerLvl)
        {
            playerLvl = curLvl;
        }

        int xpNextLevel = 100 * (playerLvl + 1) * (playerLvl + 1);
        int differenceXp = xpNextLevel - experience;

        int totalDifference = xpNextLevel - (100 * playerLvl * playerLvl);
    }

    // We can edit our and enemy stats according to level
    public void Stats()
    {
        switch (unitLevel)
        {
            case 1:
                damage += 0;
                break;
            case 2:
                damage += 5;
                break;
            case 3:
                maxHP += 5;
                currentHP += 5;
                damage += 5;
                break;
            case 4:
                maxHP += 5;
                currentHP += 5;
                maxMP += 5;
                currentMP += 5;
                damage += 5;
                break;
            case 5:
                spellDamage += 10;
                dmgSpellCost += 5;
                break;
            case 6:
                maxHP += 5;
                currentHP += 5;
                maxMP += 5;
                currentMP += 5;
                damage += 10;
                spellDamage += 10;
                dmgSpellCost += 5;
                break;
            case 7:
                maxHP += 10;
                currentHP += 10;
                maxMP += 5;
                currentMP += 5;
                damage += 10;
                spellDamage += 10;
                dmgSpellCost += 5;
                break;
            case 8:
                maxHP += 10;
                currentHP += 10;
                maxMP += 10;
                currentMP += 10;
                damage += 10;
                spellDamage += 10;
                dmgSpellCost += 5;
                break;
            case 9:
                maxHP += 20;
                currentHP += 20;
                maxMP += 10;
                currentMP += 10;
                damage += 10;
                spellDamage += 10;
                dmgSpellCost += 5;
                break;
            case 10:
                maxHP += 20;
                currentHP += 20;
                maxMP += 20;
                currentMP += 20;
                damage += 10;
                spellDamage += 10;
                dmgSpellCost += 5;
                break;
        }      
    }

    // editing player stats according to level
    public void PlayerStats()
    {
            switch (playerLvl)
            {
                case 1:
                    playerHP = 50;
                    playerCurrentHP = 50;
                    playerMP = 40;
                    playerCurrentMP = 40;
                    playerDMG = 25;
                    spellDamage = 35;
                    break;
                case 2:
                    playerHP = 55;
                    playerCurrentHP = 55;
                    playerMP = 40;
                    playerCurrentMP = 40;
                    playerDMG = 25;
                    spellDamage = 35;
                    break;
                case 3:
                    playerHP = 55;
                    playerCurrentHP = 55;
                    playerMP = 45;
                    playerCurrentMP = 45;
                    playerDMG = 25;
                    spellDamage = 35;
                    break;
                case 4:
                    playerHP = 55;
                    playerCurrentHP = 55;
                    playerMP = 45;
                    playerCurrentMP = 45;
                    playerDMG = 25;
                    break;
                case 5:
                    playerHP = 55;
                    playerCurrentHP = 55;
                    playerMP = 45;
                    playerCurrentMP = 45;
                    playerDMG = 25;
                    spellDamage = 45;
                    break;
                case 6:
                    playerHP = 55;
                    playerCurrentHP = 55;
                    playerMP = 45;
                    playerCurrentMP = 45;
                    playerDMG = 30;
                    spellDamage = 45;
                    break;
                case 7:
                    playerHP = 60;
                    playerCurrentHP = 60;
                    playerMP = 45;
                    playerCurrentMP = 45;
                    playerDMG = 30;
                    spellDamage = 45;
                    break;
                case 8:
                    playerHP = 60;
                    playerCurrentHP = 60;
                    playerMP = 45;
                    playerCurrentMP = 45;
                    playerDMG = 30;
                    spellDamage = 40;
                    break;
                case 9:
                    playerHP = 70;
                    playerCurrentHP = 70;
                    playerMP = 50;
                    playerCurrentMP = 50;
                    playerDMG = 30;
                    spellDamage = 40;
                    break;
                case 10:
                    playerHP = 70;
                    playerCurrentHP = 70;
                    playerMP = 60;
                    playerCurrentMP = 60;
                    playerDMG = 30;
                    spellDamage = 50;
                    break;
            }
    }
}

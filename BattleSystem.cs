using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, FINISHED }

public class BattleSystem : MonoBehaviour
{
    // details about dungeon progress
    private int dungeonFloor = 1;
    private int enemiesKilled;

    // instantiate gameobject
    private GameObject enemyGO;
    public GameObject playerGO;

    // player prefab
    public GameObject playerPrefab;

    // enemies prefabs
    private GameObject enemySpawn;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;
    public GameObject enemyPrefab5;

    // level window
    public GameObject levelWindow;

    // canvas
    public GameObject advCanvas;
    public GameObject worldMapCanv;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text lvlText;
    public Text dialogueText;
    public Text floorText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    IEnumerator SetupBattle()
    {
        playerUnit = playerGO.GetComponent<Unit>();

        enemySpawn = enemyPrefab1;
        enemyGO = Instantiate(enemySpawn, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetPlayerHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator NextFloor()
    {
        state = BattleState.START;
        floorText.text = "Floor: " + dungeonFloor + "/5";

        enemyGO = Instantiate(enemySpawn, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void StartNewFloor()
    {
        switch (dungeonFloor)
        {
            case 2:
                enemySpawn = enemyPrefab2;
                StartCoroutine(NextFloor());
                break;
            case 3:
                enemySpawn = enemyPrefab3;
                StartCoroutine(NextFloor());
                break;
            case 4:
                enemySpawn = enemyPrefab4;
                StartCoroutine(NextFloor());
                break;
            case 5:
                enemySpawn = enemyPrefab5;
                StartCoroutine(NextFloor());
                break;
            default:
                return;
        }
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.playerDMG);

        enemyHUD.SetHP(enemyUnit.currentHP);
        enemyHUD.SetHUD(enemyUnit);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            dialogueText.text = "You won the battle!";

            yield return new WaitForSeconds(2f);

            EndBattle(playerUnit);
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    public void EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " turn";

        int enemyTurn = Random.Range(1, 4);

        switch (enemyTurn)
        {
            case 1:
                StartCoroutine(EnemyAttack());
                break;
            case 2:
                if (enemyUnit.currentHP < enemyUnit.maxHP && enemyUnit.currentMP >= enemyUnit.healSpellCost)
                {
                    StartCoroutine(EnemyCastHeal());
                }
                else
                {
                    EnemyTurn();
                }
                break;
            case 3:
                if (enemyUnit.currentMP >= enemyUnit.dmgSpellCost)
                {
                    StartCoroutine(EnemyCastFireBall());
                }
                else
                {
                    EnemyTurn();
                }          
                break;
            default:
                return;
        }
    }

    IEnumerator EnemyAttack()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamagePlayer(enemyUnit.damage);

        yield return new WaitForSeconds(1f);

        playerHUD.SetHP(playerUnit.playerCurrentHP);
        playerHUD.SetPlayerHUD(playerUnit);

        if (isDead)
        {
            state = BattleState.LOST;
            dialogueText.text = "You were defeated.";

            yield return new WaitForSeconds(2f);

            EndBattle(playerUnit);
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle(Unit unit)
    {
        //Unit unit = new Unit();
        if(state == BattleState.WON)
        {
            // we re heading further into dungeon where we encounter next enemies
            Destroy(enemyGO);
            dungeonFloor++;
            enemiesKilled++;
            

            if (enemiesKilled < 5)
            {
                StartNewFloor();
            }    
        }
        else if (state == BattleState.LOST)
        {
            // we re going back to the map where we choose what arena we want to fight
            advCanvas.SetActive(false);
            worldMapCanv.SetActive(true);
            unit.PlayerStats();
        }

        if (enemiesKilled == 5)
        {
            dialogueText.text = "You completed dungeon!";
            unit.ExpGained(500);
            unit.PlayerStats();

            lvlText.text = "Lvl: " + unit.playerLvl;
            levelWindow.SetActive(true);
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    public void RestartLvl()
    {
        Destroy(enemyGO);
        enemySpawn = enemyPrefab1;
        dungeonFloor = 1;
        enemiesKilled = 0;
        floorText.text = "Floor: " + dungeonFloor + "/5";
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        levelWindow.SetActive(false);
    }

    IEnumerator PlayerRest()
    {
        int restHP = Random.Range(4, 7);
        int restMP = Random.Range(18, 21);

        playerUnit.Rest(restHP, restMP);

        playerHUD.SetHP(playerUnit.playerCurrentHP);
        playerHUD.SetMP(playerUnit.playerCurrentMP);
        playerHUD.SetPlayerHUD(playerUnit);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator PlayerCastFireBall()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.spellDamage);
        bool noMana = playerUnit.TakeManaPlayer(playerUnit.dmgSpellCost);

        if (noMana)
        {
            dialogueText.text = "Not enough mana!";
            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }
        else
        {
            yield return new WaitForSeconds(1f);

            enemyHUD.SetHP(enemyUnit.currentHP);
            playerHUD.SetMP(playerUnit.playerCurrentMP);
            enemyHUD.SetHUD(enemyUnit);
            playerHUD.SetPlayerHUD(playerUnit);
            dialogueText.text = "You casted fire ball spell!";

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.WON;
                dialogueText.text = "You won the battle!";

                yield return new WaitForSeconds(2f);

                EndBattle(playerUnit);
            }
            else
            {
                state = BattleState.ENEMYTURN;
                EnemyTurn();
            }
        }    
    }

    IEnumerator EnemyCastFireBall()
    {
        bool isDead = playerUnit.TakeDamagePlayer(enemyUnit.spellDamage);
        bool noMana = enemyUnit.TakeMana(enemyUnit.dmgSpellCost);

        if (noMana)
        {
            EnemyTurn();
        }
        else
        {
            yield return new WaitForSeconds(1f);

            playerHUD.SetHP(playerUnit.playerCurrentHP);
            enemyHUD.SetMP(enemyUnit.currentMP);
            playerHUD.SetPlayerHUD(playerUnit);
            enemyHUD.SetHUD(enemyUnit);
            dialogueText.text = enemyUnit.unitName + " casted fire ball spell!";

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.LOST;
                dialogueText.text = "You were defeated.";

                yield return new WaitForSeconds(2f);

                EndBattle(playerUnit);
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
    }

    IEnumerator PlayerCastHeal()
    {
        bool noMana = playerUnit.TakeManaPlayer(playerUnit.healSpellCost);

        if (noMana)
        {
            dialogueText.text = "Not enough mana!";
            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }
        else
        {
            int healAmount = Random.Range(30, 41);

            playerUnit.PlayerHeal(healAmount);

            playerHUD.SetHP(playerUnit.playerCurrentHP);
            playerHUD.SetMP(playerUnit.playerCurrentMP);
            playerHUD.SetPlayerHUD(playerUnit);
            dialogueText.text = "You casted healing spell!";

            yield return new WaitForSeconds(2f);

            
            state = BattleState.ENEMYTURN;
            EnemyTurn();
            
        }
    }

    IEnumerator EnemyCastHeal()
    {
        bool noMana = enemyUnit.TakeMana(enemyUnit.healSpellCost);

        if (noMana)
        {
            EnemyTurn();
        }
        else
        {
            int healAmount = Random.Range(30, 41);

            enemyUnit.Heal(healAmount);

            enemyHUD.SetHP(enemyUnit.currentHP);
            enemyHUD.SetMP(enemyUnit.currentMP);
            enemyHUD.SetHUD(enemyUnit);
            dialogueText.text = enemyUnit.unitName + " had casted healing spell!";

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator PlayerRunsAway()
    {
        dialogueText.text = "You flee away from the battleground!";

        yield return new WaitForSeconds(3f);

        advCanvas.SetActive(false);
        worldMapCanv.SetActive(true);
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnWaitButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerRest());
    }

    public void OnFireBallButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerCastFireBall());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerCastHeal());
    }

    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerRunsAway());
    }
}

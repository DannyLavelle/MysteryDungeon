using UnityEngine.InputSystem;
using UnityEngine;
using System;
using TMPro;
public class PlayerInput : MonoBehaviour
{
    private Camera cam;
    private PlayerController player;
    public LayerMask floorLayerMask;
    public TMP_Text text;
    int ability;
    private void Start()
    {


        cam = Camera.main;
        player = GetComponent<PlayerController>();

        InputHandler inputHandler =GetComponent<InputHandler>();
        inputHandler.MoveEvent += CharacterMove;
        inputHandler.BasicAttackEvent += Attack;
        inputHandler.AbilitySwitchEvent += AbilitySwitch;
        inputHandler.ItemSwitchEvent += ItemUse;


        AbilitySwitch(1);

    }


    private void Update()
    {
        
    }
    private void GetDistanceWithRay(Vector2 mousePos, out int distance, out Vector2Int gridTarget)
    {
        distance = 0; // Initialize the output variable
        gridTarget = new Vector2Int(); // Initialize the output variable

        Ray ray = cam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, floorLayerMask))
        {
            gridTarget = new Vector2Int(
                Mathf.RoundToInt(hit.point.x),
                Mathf.RoundToInt(hit.point.z)
            );

            distance = Mathf.Max(
                Mathf.Abs(gridTarget.x - player.GridPosition.x),
                Mathf.Abs(gridTarget.y - player.GridPosition.y)
            );
        }
    }


    private void CharacterMove(Vector2 mousePos)
    {
        GetDistanceWithRay(mousePos, out int distance, out var gridTarget);
        CheckInRangeThenExecute((d, r) => d > r, distance,0, new MoveAction(player, gridTarget)); // "greater than"
    }

    private void Attack(Vector2 mousePos)
    {
        GetDistanceWithRay(mousePos, out int distance, out var gridTarget);
        AttackAction action = null;
        switch(ability)
        {
            case 1:
            action = new AttackAction(player,gridTarget);
            break;
            case 2:
            action = new StrongAttackAction(player, gridTarget);
            break;
            case 3:
            action = new MineAttackAction(player, gridTarget);
            break;
            case 4:
            action = new ThrowAttackAction(player, gridTarget);
            break;
            default:
            Debug.LogWarning("Didn't set ability or ability is not added");
            
            break;
        }

        if (action == null) return;
        
        CheckInRangeThenExecute((d, r) => d == r, distance,1, action); // "equal to"
    }
    private void AbilitySwitch(int abilityID)
    {
        ability = abilityID;
        string abilityName = "";
        switch(abilityID)
        {
            case 1:
            abilityName = "Weak Slash";
            break;
            case 2:
            abilityName = "Strong Slash";
            break;
            case 3:
            abilityName = "Place Mine";
            break;
            case 4:
            abilityName = "Throw Weapon";
            break;
        }

        text.text = ("Current Ability: " + abilityName);
    }

    private void CheckInRangeThenExecute(Func<int, int, bool> condition, int distance,int range, GameAction action)
    {
        if (condition(distance, range)) // Uses the provided comparison function
        {
            TurnManager.Instance.EnqueueAction(action);
        }
        else
        {
            Debug.Log($"Action not possible—distance {distance} did not meet the required condition.");
        }
    }

    private void ItemUse(int index)
    {
        Inventory inv = GetComponent<Inventory>();
        inv.UseItem(index);
    }



}






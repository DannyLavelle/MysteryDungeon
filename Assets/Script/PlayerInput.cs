using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera cam;
    private PlayerController player;

    private void Start()
    {


        cam = Camera.main;
        player = GetComponent<PlayerController>();

        InputHandler inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.MoveEvent += CharacterMove;
    }

    private void Update()
    {
        
    }

    private void CharacterMove(Vector2 mousePos)
    {
        Ray ray = cam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("Floor"))
        {

           
            var gridTarget = new Vector2Int(
                Mathf.RoundToInt(hit.point.x),
                Mathf.RoundToInt(hit.point.z)
            );

            // optional: check reachability
            int distance = Mathf.Max(
                Mathf.Abs(gridTarget.x - player.GridPosition.x),
                Mathf.Abs(gridTarget.y - player.GridPosition.y)
            );
            if (distance > 0)
            {
                var move = new MoveAction(player, gridTarget);
                TurnManager.Instance.EnqueueAction(move);
            }
        }
    }
}

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
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("1");
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("Floor"))
            {
                
                // assume your Floor tiles have colliders and tag="Floor",
                // and world pos (x,0,z) maps to grid (x,z)
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
}

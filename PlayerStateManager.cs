using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(PlayerController))]
public class PlayerStateManager : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
        
    private PlayerController playerController;

    // Player's state list
    // These states will be initialized initially on Start() or Awake() with player's components
    // These states can't be modified during "Play": Add or Delete
    // Player can only change state within these states
    public PlayerGroundedState GroundedState { get; private set; }
    public PlayerAirborneState AirborneState { get; private set; }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        // Init states of player
        GroundedState = new PlayerGroundedState(playerController, this);
        AirborneState = new PlayerAirborneState(playerController, this); 

        ChangeState(GroundedState); // Set default state
    }

    private void Update()
    {
        CurrentState?.UpdateState(); // Execute state update
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState?.EnterState();

        // Debug.Log("Change to new state: " + newState.GetType().Name);
    }
}

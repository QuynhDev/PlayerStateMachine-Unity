using UnityEditor;
using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    // Call constructor method of parent class
    public PlayerAirborneState(PlayerController playerController, PlayerStateManager stateManager) : base(playerController, stateManager) { }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // Allows movement while in AirborneState
        playerController.ExecuteMove(GameInput.Instance.GetMovementVectorNormalized(), playerController.MoveSpeed); 
        playerController.ApplyGravity();

        if (playerController.Grounded) // If is grounded -> Change state to [GroundedState]
        {
            stateManager.ChangeState(stateManager.GroundedState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}

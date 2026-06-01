using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    // Call constructor method of parent class
    public PlayerGroundedState(PlayerController playerController, PlayerStateManager stateManager) : base(playerController, stateManager) { }

    private void GameInput_OnJumpPerformed(object sender, System.EventArgs e) 
    {
        playerController.ExecuteJump();
        stateManager.ChangeState(stateManager.AirborneState);
    }

    public override void EnterState()
    {
        base.EnterState();

        GameInput.Instance.OnJumpPerformed += GameInput_OnJumpPerformed; 
    }


    public override void UpdateState()
    {
        base.UpdateState(); 

        float targetSpeed = GameInput.Instance.IsSprintPressed() ? playerController.SprintSpeed : playerController.MoveSpeed;

        playerController.ExecuteMove(GameInput.Instance.GetMovementVectorNormalized(), targetSpeed);
        playerController.ApplyGravity();

        if (!playerController.Grounded) // If is not grounded -> Change state to [AirborneState]
        {
            stateManager.ChangeState(stateManager.AirborneState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        GameInput.Instance.OnJumpPerformed -= GameInput_OnJumpPerformed;
    }
}

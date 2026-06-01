using UnityEngine;

public abstract class PlayerState // Patern for all player state: Grounded, Airborne,...
{
    protected PlayerController playerController;
    protected PlayerStateManager stateManager;

    public PlayerState(PlayerController playerController, PlayerStateManager stateManager)
    {
        this.playerController = playerController;
        this.stateManager = stateManager;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
}

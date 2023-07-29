public interface IDefineMovement
{
    public void OnMoveLeft();
    public void OnMoveRight();

    public void ProcessSidewaysMovement(MovementLane lane);

    public void ProcessForwardMovement(float speed);
}
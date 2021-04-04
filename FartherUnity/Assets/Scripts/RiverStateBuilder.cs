public class RiverStateBuilder
{
    private readonly bool[] connections;
    public bool ConnectsUpRight { get { return connections[0]; } set { connections[0] = value; } }
    public bool ConnectsRight { get { return connections[1]; } set { connections[1] = value; } }
    public bool ConnectsDownRight { get { return connections[2]; } set { connections[2] = value; } }
    public bool ConnectsDownLeft { get { return connections[3]; } set { connections[3] = value; } }
    public bool ConnectsLeft { get { return connections[4]; } set { connections[4] = value; } }
    public bool ConnectsUpLeft { get { return connections[5]; } set { connections[5] = value; } }

    public RiverState ToState()
    {
        return new RiverState(ConnectsUpRight, ConnectsRight, ConnectsDownRight, ConnectsDownLeft, ConnectsLeft, ConnectsUpLeft);
    }
}

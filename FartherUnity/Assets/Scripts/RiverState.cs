using System.Linq;

public class RiverState
{
    public bool Exists
    {
        get
        {
            return connections.Any();
        }
    }
    private readonly bool[] connections;
    public bool ConnectsUpRight { get { return connections[0]; } }
    public bool ConnectsRight { get { return connections[1]; } }
    public bool ConnectsDownRight { get { return connections[2]; } }
    public bool ConnectsDownLeft { get { return connections[3]; } }
    public bool ConnectsLeft { get { return connections[4]; } }
    public bool ConnectsUpLeft { get { return connections[5]; } }

    public RiverState(bool connectsUpRight = false, 
        bool connectsRight = false, 
        bool connectsDownRight = false,
        bool connectsDownLeft = false,
        bool connectsLeft = false,
        bool connectsUpLeft = false)
    {
        this.connections = new bool[] { connectsUpRight, connectsRight, connectsDownRight, connectsDownLeft, connectsLeft, connectsUpLeft };
    }

}

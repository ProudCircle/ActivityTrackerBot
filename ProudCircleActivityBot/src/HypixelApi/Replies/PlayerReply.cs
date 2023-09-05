namespace ProudCircleActivityBot.HypixelApi.Replies; 

public class PlayerReply {
    public PlayerType PlayerType { get; private set; }
}

public enum PlayerType {
    None = 0,
    Player = 1,
    NickedPlayer = 2
}
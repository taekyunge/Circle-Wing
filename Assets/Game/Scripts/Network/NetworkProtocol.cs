public enum NetworkProtocol : byte
{
    SERVER_ENTER_PLAYER = 0,
    SERVER_DEATH_PLAYER,
    SERVER_FINISH_GAME,

    START_COUNT = 100,
    FINISH_COUNT,
    FINISH_PLAYER,
    DEATH_PLAYER,
    END_GAME,

    // Photon2 Protocols
    LEAVE = 254,
}
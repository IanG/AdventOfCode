using System.Net.Security;

namespace Day10.Data;

public enum TileType
{
    None = 0,
    StartingPoint = 83,   // S
    Vertical = 124,       // |
    Horizontal = 45,      // -
    NorthEastBend = 76,   // L
    NorthWestBend = 74,   // J
    SouthWestBend = 55,   // 7
    SouthEastBend = 70,   // F
    Ground = 46           // .
}
namespace PrisonBreak.Tests
{
    internal class Common
    {
        internal static string[] smallMazeLines =
        {
            "C__", "__X", "E_X"
        };

        internal static string[] mediumMazeLines =
        {
            "_X__X_X_X", "CXX______", "____XX__X", "XX__X_X__",
            "XXXXXEX__", "X______X_", "__XXXXX__", "________X"
        };

        internal static string[] largeMazeLines =
        {
            "X_XXXXXXXXX__", "__X___E______", "_XXXXXXXXXXX_",
            "____X___X____", "XXXXXXX______", "_____________", "_XXXX________",
            "X_CX__XXXXXX_", "____XXXXX____", "_XX________X_",
        };

        internal static string[] mazeSymbols =
        {
            "X_", "CE"
        };

        internal static string[] noSolutionMazeLines =
        {
            "C__", "X_X", "EXX"
        };
    }
}
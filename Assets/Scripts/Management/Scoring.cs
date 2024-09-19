using Game.Management;
using System;

public static class Scoring
{
    // For now simply klondike, but it is easy to add different rules
    public static IScoreResolver Resolver = new KlondikeScoreResolver();

    public static void HandleScoreChanged(int scoreChange) => OnScoreChanged?.Invoke(scoreChange);

    public static event Action<int> OnScoreChanged;
}
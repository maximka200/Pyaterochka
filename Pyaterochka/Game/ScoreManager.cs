using System.IO;

public static class ScoreManager
{
    private const string FilePath = "highscore.txt";

    public static int LoadHighScore()
    {
        if (File.Exists(FilePath) && int.TryParse(File.ReadAllText(FilePath), out int score))
            return score;
        return 0;
    }

    public static void SaveHighScore(int score)
    {
        File.WriteAllText(FilePath, score.ToString());
    }
}
namespace Winkeladvokat
{
    using System.Windows.Media;

    public class ScoreResult
    {
        public ScoreResult(Color color, int score)
        {
            this.Color = color;
            this.Score = score;
        }

        public Color Color { get; private set; }

        public int Score { get; private set; }
    }
}
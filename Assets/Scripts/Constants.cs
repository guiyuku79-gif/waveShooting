using System.Collections.Generic;
public static class Constants
{
    public const int Division = 100; //媒質の点の個数
    public const float Width = 12; //媒質の幅
    public const float WaveSpeed = 1f; //波の速さ

    public const float LaneHeight = 4f;

    public static readonly IReadOnlyList<float> laneYs = new float[]
    {
    4.5f,0f,-4.5f
    };

    public enum WaveType
    {
        Player,
        Enemy
    }

}
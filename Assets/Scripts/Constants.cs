using System.Collections.Generic;
public static class Constants
{
    public const int Division = 150; //媒質の点の個数
    public const float Width = 16; //媒質の幅
    public const float WaveSpeed = 0.8f; //波の速さ

    public const float PlayerX = 2f; //媒質の端から見たプレイヤーのX座標

    public const int PlayerDivisionX = (int)(PlayerX * Division / Width); //Mediumから見たプレイヤーの位置

    public const float ShortestWaveLength = 0.5f;

    public const float LaneHeight = 4f; //レーンの高さ

    public const float ScreenHeight = 14f; //プレイヤーの動ける範囲

    public static readonly IReadOnlyList<float> laneYs = new float[]
    {
    4.5f,0f,-4.5f
    };

    public enum EnemyAction
    {
        Wait,
        Wave,
        WaveStart,
        Move
    }

}
interface IDefineObstacle
{
    public FactionType ObstacleFaction { get; }
    public int Damage { get; }

    public int InflictDamage();
}
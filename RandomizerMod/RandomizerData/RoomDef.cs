namespace RandomizerMod.RandomizerData
{
    public record RoomDef
    {
        public string SceneName { get; init; }
        public string MapArea { get; init; }
        public string TitledArea { get; init; }

        public virtual bool Equals(RoomDef other) => ReferenceEquals(this, other) ||
            (other is not null && this.EqualityContract == other.EqualityContract && this.SceneName == other.SceneName &&
            this.MapArea == other.MapArea && this.TitledArea == other.TitledArea);

        public override int GetHashCode() => HashCode.Combine(EqualityContract.GetHashCode(),
            SceneName?.GetHashCode(), MapArea?.GetHashCode(), TitledArea?.GetHashCode());
    }
}

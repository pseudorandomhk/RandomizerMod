using RandomizerCore.Logic;
using RandomizerMod.RC;
using System.Collections;

namespace RandomizerMod.RandomizerData
{
    public record CostDef(string Term, int Amount)
    {
        public virtual LogicCost ToLogicCost(LogicManager lm)
        {
            return Term switch
            {
                "GEO" => new LogicGeoCost(lm, Amount),
                _ => new SimpleCost(lm.GetTermStrict(Term), Amount),
            };
        }

        public virtual bool Equals(CostDef other) => ReferenceEquals(this, other) ||
            (other is not null && this.EqualityContract == other.EqualityContract &&
            this.Term == other.Term && this.Amount == other.Amount);

        public override int GetHashCode() => HashCode.Combine(EqualityContract.GetHashCode(),
            Term?.GetHashCode(), Amount.GetHashCode());
    }
}

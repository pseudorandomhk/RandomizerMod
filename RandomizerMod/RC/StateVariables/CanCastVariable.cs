using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;

namespace RandomizerMod.RC.StateVariables
{
    public class CanCastVariable : StateModifierWrapper<CastSpellVariable>
    {
        public override string Name { get; }
        public const string Prefix = "$CANCAST";
        protected override string InnerPrefix => CastSpellVariable.Prefix;

        protected readonly StateBool EquippedSpellTwister;

        public CanCastVariable(string name, LogicManager lm) : base(name, lm)
        {
            Name = name;
            try
            {
                EquippedSpellTwister = lm.StateManager.GetBool("CHARM33");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Exception constructing CanCastVariable", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (term.StartsWith(Prefix))
            {
                variable = new CanCastVariable(term, lm);
                return true;
            }
            variable = default;
            return false;
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            int totalCasts = InnerVariable.SpellCasts.Sum();
            foreach (LazyStateBuilder innerState in InnerVariable.ModifyState(sender, pm, state))
            {
                LazyStateBuilder innerStateCopy = innerState;
                InnerVariable.RecoverSoul((state.GetBool(EquippedSpellTwister) ? 24 : 33) * totalCasts, ref innerStateCopy);
                yield return innerStateCopy;
            }
        }

        public override IEnumerable<Term> GetTerms() => InnerVariable.GetTerms();
    }
}

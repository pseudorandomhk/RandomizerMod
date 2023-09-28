using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;

namespace RandomizerMod.RC.StateVariables
{
    /*
     * Prefix :$CANGETFLOAT
     * Required Parameters: None
     * Optional Parameters: Same as $CASTSPELL. The total number of spell casts must be 1, if specified.
    */
    public class CanGetFloatVariable : StateModifierWrapper<CastSpellVariable>
    {
        public override string Name { get; }
        public const string Prefix = "$CANGETFLOAT";
        protected override string InnerPrefix => CastSpellVariable.Prefix;

        protected readonly StateBool NoFloat;
        protected readonly StateBool Float;
        protected readonly Term Fireball;

        public CanGetFloatVariable(string name, LogicManager lm) : base(name, lm)
        {
            Name = name;
            try
            {
                NoFloat = lm.StateManager.GetBoolStrict("NOFLOAT");
                Float = lm.StateManager.GetBoolStrict("FLOAT");
                Fireball = lm.GetTermStrict("FIREBALL");

                if (InnerVariable.SpellCasts.Length != 1 || InnerVariable.SpellCasts[0] != 1)
                {
                    throw new ArgumentOutOfRangeException("CanGetFloat takes exactly one spell cast");
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error constructing CanGetFloatVariable", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (term.StartsWith(Prefix))
            {
                variable = new CanGetFloatVariable(term, lm);
                return true;
            }
            variable = default;
            return false;
        }

        public override IEnumerable<Term> GetTerms()
        {
            yield return Fireball;
            foreach (Term t in InnerVariable.GetTerms()) yield return t;
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            yield return new(state);
            if (pm.Has(Fireball, 1))
            {
                foreach (LazyStateBuilder innerState in InnerVariable.ModifyState(sender, pm, state))
                {
                    innerState.SetBool(NoFloat, false);
                    innerState.SetBool(Float, true);
                    yield return innerState;
                }
            }
        }

        public override IEnumerable<LazyStateBuilder>? ProvideState(object? sender, ProgressionManager pm)
            => Enumerable.Empty<LazyStateBuilder>();
    }

    /*
     * Prefix: $CONVERTFLOATTOPFLOAT
     * Required Parameters: None
     * Optional Parameters: None
    */
    public class PfloatConversionVariable : StateModifier
    {
        public override string Name { get; }
        public const string Prefix = "$CONVERTFLOATTOPFLOAT";

        protected readonly StateBool Float;
        protected readonly StateBool Pfloat;

        public PfloatConversionVariable(string name, LogicManager lm)
        {
            Name = name;
            try
            {
                Float = lm.StateManager.GetBoolStrict("FLOAT");
                Pfloat = lm.StateManager.GetBoolStrict("PFLOAT");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error constructing PfloatConversionVariable", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (term == Prefix)
            {
                variable = new PfloatConversionVariable(term, lm);
                return true;
            }
            variable = default;
            return false;
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            if (!state.GetBool(Float))
            {
                state.SetBool(Float, false);
                state.SetBool(Pfloat, true);
            }
            yield return state;
        }

        public override IEnumerable<LazyStateBuilder>? ProvideState(object? sender, ProgressionManager pm)
            => Enumerable.Empty<LazyStateBuilder>();

        public override IEnumerable<Term> GetTerms() => Enumerable.Empty<Term>();
    }

    /*
     * Prefix: $REMOVEPFLOAT
     * Required Parameters: None
     * Optional Parameters: Same as $CASTSPELL. The total number of spell casts must be 1, if specified.
    */
    public class RemovePfloatVariable : StateModifierWrapper<CastSpellVariable>
    {
        public override string Name { get; }
        public const string Prefix = "$REMOVEPFLOAT";
        protected override string InnerPrefix => CastSpellVariable.Prefix;

        protected readonly StateBool NoFloat;
        protected readonly StateBool Pfloat;
        protected readonly Term AnySpell;
        
        public RemovePfloatVariable(string name, LogicManager lm) : base(name, lm)
        {
            Name = name;
            try
            {
                NoFloat = lm.StateManager.GetBoolStrict("NOFLOAT");
                Pfloat = lm.StateManager.GetBoolStrict("PFLOAT");
                AnySpell = lm.GetTermStrict("ANYSPELL");

                if (InnerVariable.SpellCasts.Length != 1 || InnerVariable.SpellCasts[0] != 1)
                {
                    throw new ArgumentOutOfRangeException("RemovePfloat takes exactly one spell cast");
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Exception constructing RemovePfloatVariable", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (term.StartsWith(Prefix))
            {
                variable = new RemovePfloatVariable(term, lm);
                return true;
            }
            variable = default;
            return false;
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            if (pm.Has(AnySpell) && state.GetBool(Pfloat))
            {
                foreach (LazyStateBuilder innerState in InnerVariable.ModifyState(sender, pm, state))
                {
                    innerState.SetBool(Pfloat, false);
                    innerState.SetBool(NoFloat, true);
                    yield return innerState;
                }
            }
        }

        public override IEnumerable<Term> GetTerms()
        {
            yield return AnySpell;
            foreach (Term t in InnerVariable.GetTerms()) yield return t;
        }
    }

    /*
     * Prefix: $CANGETDIVEFLOAT
     * Required Parameters: None
     * Optional Parameters: Same as $CASTSPELL. The total number of spell casts must be 1, if specified.
    */
    public class CanGetDiveFloatVariable : StateModifierWrapper<CanCastVariable>
    {
        public override string Name { get; }
        public const string Prefix = "$CANGETDIVEFLOAT";
        protected override string InnerPrefix => CanCastVariable.Prefix;

        protected readonly StateBool NoFloat;
        protected readonly StateBool Pfloat;
        protected readonly StateBool DiveFloat;
        protected readonly Term Quake;

        public CanGetDiveFloatVariable(string name, LogicManager lm) : base(name, lm)
        {
            Name = name;
            try
            {
                NoFloat = lm.StateManager.GetBoolStrict("NOFLOAT");
                Pfloat = lm.StateManager.GetBoolStrict("PFLOAT");
                DiveFloat = lm.StateManager.GetBoolStrict("DIVEFLOAT");
                Quake = lm.GetTermStrict("QUAKE");

                if (InnerVariable.InnerVariable.SpellCasts.Length != 1 || InnerVariable.InnerVariable.SpellCasts[0] != 0)
                {
                    throw new ArgumentOutOfRangeException("CanGetDiveFloat takes exactly one spell cast");
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Exception constructing CanGetDiveFloatVariable", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (term.StartsWith(Prefix))
            {
                variable = new CanGetDiveFloatVariable(term, lm);
                return true;
            }
            variable = default;
            return false;
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            yield return new(state);
            if (pm.Has(Quake, 1))
            {
                foreach (LazyStateBuilder innerState in InnerVariable.ModifyState(sender, pm, state))
                {
                    innerState.SetBool(DiveFloat, true);
                    if (!innerState.GetBool(Pfloat)) innerState.SetBool(NoFloat, false);
                    yield return innerState;
                }
            }
        }

        public override IEnumerable<LazyStateBuilder>? ProvideState(object? sender, ProgressionManager pm)
            => Enumerable.Empty<LazyStateBuilder>();

        public override IEnumerable<Term> GetTerms()
        {
            yield return Quake;
            foreach (Term t in InnerVariable.GetTerms()) yield return t;
        }
    }

    /*
     * Prefix: $CONVERTDIVEFLOATTODIVEWALKOUT
     * Required Parameters: None
     * Optional Parameters: None
    */
    public class DiveWalkOutConversionVariable : StateModifier
    {
        public override string Name { get; }
        public const string Prefix = "$CONVERTDIVEFLOATTODIVEWALKOUT";

        protected readonly StateBool DiveFloat;
        protected readonly StateBool DiveWalkOut;
        protected readonly StateBool NoFloat;
        protected readonly StateBool Pfloat;

        public DiveWalkOutConversionVariable(string name, LogicManager lm)
        {
            Name = name;
            try
            {
                DiveFloat = lm.StateManager.GetBoolStrict("DIVEFLOAT");
                DiveWalkOut = lm.StateManager.GetBoolStrict("DIVEWALKOUT");
                NoFloat = lm.StateManager.GetBoolStrict("NOFLOAT");
                Pfloat = lm.StateManager.GetBoolStrict("PFLOAT");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Exception constructing DiveWalkOutConversionVariable", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (term == Prefix)
            {
                variable = new DiveWalkOutConversionVariable(term, lm);
                return true;
            }
            variable = default;
            return false;
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            if (state.GetBool(DiveFloat))
            {
                state.SetBool(DiveFloat, false);
                state.SetBool(DiveWalkOut, true);
                if (!state.GetBool(Pfloat)) state.SetBool(NoFloat, true);
            }
            yield return state;
        }

        public override IEnumerable<LazyStateBuilder>? ProvideState(object? sender, ProgressionManager pm)
            => Enumerable.Empty<LazyStateBuilder>();

        public override IEnumerable<Term> GetTerms() => Enumerable.Empty<Term>();
    }

    /*
     * Prefix: $REMOVEDIVEWALKOUT
     * Required Parameters: None
     * Optional Parameters: None
    */
    public class RemoveDiveWalkOutVariable : StateModifier
    {
        public override string Name { get; }
        public const string Prefix = "$REMOVEDIVEWALKOUT";

        protected readonly StateBool DiveWalkOut;

        public RemoveDiveWalkOutVariable(string name, LogicManager lm)
        {
            Name = name;
            try
            {
                DiveWalkOut = lm.StateManager.GetBoolStrict("DIVEWALKOUT");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Exception constructing RemoveDiveWalkOutVariable", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (term == Prefix)
            {
                variable = new RemoveDiveWalkOutVariable(term, lm);
                return true;
            }
            variable = default;
            return false;
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            state.SetBool(DiveWalkOut, false);
            yield return state;
        }

        public override IEnumerable<LazyStateBuilder>? ProvideState(object? sender, ProgressionManager pm)
            => Enumerable.Empty<LazyStateBuilder>();

        public override IEnumerable<Term> GetTerms() => Enumerable.Empty<Term>();
    }
}

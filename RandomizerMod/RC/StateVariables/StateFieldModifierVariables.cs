using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;

namespace RandomizerMod.RC.StateVariables
{
    internal class StateFieldModifierVariable : StateModifier
    {
        private static Dictionary<string, string> fieldModifierNames = new()
        {
            { "$NOFLOAT", "NOFLOAT" },
            { "$FLOAT", "FLOAT" },
            { "$PFLOAT", "PFLOAT" },
            { "$DIVEFLOAT", "DIVEFLOAT" },
            { "$DIVEWALKOUT", "DIVEWALKOUT" }
        };

        public override string Name { get; }

        protected readonly StateField field;

        protected StateFieldModifierVariable(string name, LogicManager lm, string fieldName)
        {
            Name = name;

            try
            {
                field = lm.StateManager.GetBoolStrict(fieldName);
                //if (field == null)
                    //field = lm.StateManager.GetIntStrict(fieldName);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Exception while constructing StateFieldModifierVariable ({fieldName})", e);
            }
        }

        public static bool TryMatch(LogicManager lm, string term, out LogicVariable variable)
        {
            if (fieldModifierNames.TryGetValue(term, out string fieldName))
            {
                variable = new StateFieldModifierVariable(term, lm, fieldName);
                return true;
            }
            else
            {
                variable = default;
                return false;
            }
        }

        public override IEnumerable<LazyStateBuilder> ModifyState(object? sender, ProgressionManager pm, LazyStateBuilder state)
        {
            if (pm.Has(field))
                yield return state;
        }

        public override IEnumerable<Term> GetTerms()
        {
            return Enumerable.Empty<Term>();
        }
    }
}

﻿using RandomizerCore;
using RandomizerCore.Randomization;

namespace RandomizerMod.RC
{
    public class SelfDualTransitionGroupBuilder : GroupBuilder
    {
        public bool coupled;
        public readonly Bucket<string> Transitions = new();

        public override void Apply(List<RandomizationGroup> groups, RandoFactory factory)
        {
            List<IRandoCouple> ts = new();
            foreach (string s in Transitions.EnumerateWithMultiplicity())
            {
                ts.Add(factory.MakeTransition(s));
            }

            if (coupled)
            {
                CoupledRandomizationGroup g = new()
                {
                    Items = ts.Select(irc => irc as IRandoItem).ToArray(),
                    Locations = ts.Select(irc => irc as IRandoLocation).ToArray(),
                    Label = label,
                    Strategy = strategy ?? factory.gs.ProgressionDepthSettings.GetTransitionPlacementStrategy(),
                    Validator = new WeakTransitionValidator(),
                };
                g.Dual = g;
                groups.Add(g);
                OnCreateGroup?.Invoke(g);
            }
            else
            {
                RandomizationGroup g = new()
                {
                    Items = ts.Select(irc => irc as IRandoItem).ToArray(),
                    Locations = ts.Select(irc => irc as IRandoLocation).ToArray(),
                    Label = label,
                    Strategy = strategy ?? factory.gs.ProgressionDepthSettings.GetTransitionPlacementStrategy(),
                };
                groups.Add(g);
                OnCreateGroup?.Invoke(g);
            }
        }
    }
}

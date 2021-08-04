using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentMdx.Lexer
{
    internal class KeLexer : ILexer
    {
        #region Properties

        private IEnumerable<IRegionRule> RegionRules { get; }

        #endregion Properties

        #region Methods

        public IEnumerable<RegionToken> Tokenize(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return Enumerable.Empty<RegionToken>();


            var state = new KeLexerState
            {
                Tokens = new List<RegionToken>(),
                CharEnumerator = new AdvancedCharEnumerator(source.Trim())
            };

            this.FindRegions(state);

            return state.Tokens;
        }

        private void FindRegions(KeLexerState state)
        {
            while (true)
            {
                var currentRegion = this.FindNextRegion(state);
                if (currentRegion is null)
                    break;

                var token = currentRegion.CreateToken();

                state.Tokens.Add(token);

                if (state.CharEnumerator.Index == -1)
                    return;

                while (true)
                {
                    if (!state.CharEnumerator.MoveNextWhileWhiteSpace())
                        return;

                    if (state.CharEnumerator.Current.Value == '.')
                    {
                        state.Tokens.Add(new RegionToken()
                        {
                            RegionMdxType = RegionMdxType.DotDelimiter,
                            Value = "."
                        });

                        if (!state.CharEnumerator.MoveNext())
                            return;
                        continue;
                    }
                    else if (state.CharEnumerator.Current.Value == '.')
                    {
                        state.Tokens.Add(new RegionToken()
                        {
                            RegionMdxType = RegionMdxType.CommaDelimiter,
                            Value = ","
                        });

                        if (!state.CharEnumerator.MoveNext())
                            return;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <remarks>Char enumerator should be setted on next symbol after region.</remarks>
        private RegionSearchState FindNextRegion(KeLexerState state)
        {
            var currentChar = state.CharEnumerator.Current.Value;
            state.CharEnumerator.MoveNext();
            var nextChar = state.CharEnumerator.Current;

            List<RegionSearchState> possibleRegions = new List<RegionSearchState>();
            foreach (var regionRule in this.RegionRules)
            {
                var checkResult = regionRule.Check(currentChar, nextChar, string.Empty);
                if (checkResult.HasFlag(RuleCheckResult.NotFound))
                    continue;

                var regionToken = new RegionSearchState(regionRule)
                { 
                    Index = state.CharEnumerator.Index
                };

                regionToken.AddSymbol(currentChar, checkResult);
                regionToken.SetLastCheckResult(checkResult);
                possibleRegions.Add(regionToken);
            }

            if (possibleRegions.Count == 0)
                return null;

            if (possibleRegions.Count == 1 && possibleRegions[0].LastCheckResult.HasFlag(RuleCheckResult.Found))
                return possibleRegions[0];

            state.CurrentParsedString = currentChar.ToString();

            var foundedRegion = this.ObserveToSeveralRegions(state, possibleRegions);
            if (foundedRegion is null)
                return null;

            return foundedRegion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="regionSearchStates"></param>
        /// <returns></returns>
        /// <remarks>Char enumerator should be setted on next symbol after region.</remarks>
        private RegionSearchState ObserveToSeveralRegions(KeLexerState state, List<RegionSearchState> regionSearchStates)
        {
            var currentChar = state.CharEnumerator.Current.Value;
            var currentRegionSerachStates = new List<RegionSearchState>(regionSearchStates);
            while (currentRegionSerachStates.Count != 0)
            {
                var waitForSubRegions = currentRegionSerachStates.Where(s => s.LastCheckResult.HasFlag(RuleCheckResult.SubRegionsStartPart)).ToList();
                if (waitForSubRegions.Count != 0)
                {
                    foreach (var regionSearchState in currentRegionSerachStates.ToArray())
                    {
                        var currentParsedStringBefore = state.CurrentParsedString;
                        if (!this.TryToCheckSubRegions(state, regionSearchState))
                        {
                            currentRegionSerachStates.Remove(regionSearchState);
                            continue;
                        }

                        return regionSearchState;
                    }
                }

                state.CharEnumerator.MoveNext();
                var nextChar = state.CharEnumerator.Current;

                foreach (var regionSearchState in currentRegionSerachStates.ToArray())
                {
                    if (regionSearchState.LastCheckResult.HasFlag(RuleCheckResult.Found))
                        continue;

                    regionSearchState.Index++;
                    var checkResult = regionSearchState.RegionRule.Check(currentChar, nextChar, state.CurrentParsedString);
                    if (checkResult.HasFlag(RuleCheckResult.NotFound))
                    {
                        currentRegionSerachStates.Remove(regionSearchState);
                        continue;
                    }

                    regionSearchState.AddSymbol(currentChar, checkResult);
                    regionSearchState.SetLastCheckResult(checkResult);

                    if (checkResult.HasFlag(RuleCheckResult.Found))
                    {
                        var maxPriority = currentRegionSerachStates.Max(_ => _.RegionRule.RegionPriority);
                        if (regionSearchState.RegionRule.RegionPriority == maxPriority)
                        {
                            state.CurrentParsedString += currentChar;
                            return regionSearchState;
                        }
                    }
                }

                state.CurrentParsedString += currentChar;

                if (currentRegionSerachStates.Count == 0)
                    return null;

                if (currentRegionSerachStates.All(r => r.LastCheckResult.HasFlag(RuleCheckResult.Found)))
                    return currentRegionSerachStates.OrderBy(r => r.RegionRule.RegionPriority).First();

                if (!nextChar.HasValue)
                    return currentRegionSerachStates.Where(r => r.LastCheckResult.HasFlag(RuleCheckResult.Found)).OrderBy(r => (int)r.RegionRule.RegionPriority).FirstOrDefault();

                currentChar = nextChar.Value;
            }

            return null;
        }

        private bool TryToCheckSubRegions(KeLexerState state, RegionSearchState mainRegionState)
        {
            var currentParsedString = state.CurrentParsedString;
            while (true)
            {
                if (!state.CharEnumerator.MoveNextWhileWhiteSpace())
                    return false;

                var currentChar = state.CharEnumerator.Current.Value;
                state.CharEnumerator.MoveNext();
                var nextChar = state.CharEnumerator.Current;

                var mainRegionCheckResult = mainRegionState.RegionRule.Check(currentChar, nextChar, currentParsedString);
                if (mainRegionCheckResult.HasFlag(RuleCheckResult.Found))
                {
                    mainRegionState.Index = mainRegionState.Index;
                    mainRegionState.AddSymbol(currentChar, mainRegionCheckResult);
                    mainRegionState.SetLastCheckResult(mainRegionCheckResult);
                    return true;
                }

                state.CharEnumerator.SetIndex(state.CharEnumerator.Index - 1);

                var subRegion = this.FindNextRegion(state);
                if (subRegion is null)
                    return mainRegionCheckResult.HasFlag(RuleCheckResult.Found);

                mainRegionState.AddSubRegion(subRegion);
            }
        }

        #endregion Methods

        #region Constructors

        public KeLexer() : this(RegionRulesBuilder.GetRegionsRules())
        {
        }

        public KeLexer(IEnumerable<IRegionRule> regionRules)
        {
            this.RegionRules = regionRules?.ToArray() ?? throw new ArgumentNullException(nameof(regionRules));
        }

        #endregion Constructors
    }
}

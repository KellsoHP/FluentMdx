using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentMdx.Lexer
{
    [DebuggerDisplay("'{LastCheckResult}':[{RegionRule}]")]
    internal class RegionSearchState
    {
        #region Properties

        public IRegionRule RegionRule { get; }

        public RuleCheckResult LastCheckResult => lastCheckResult;

        public IReadOnlyCollection<RegionSearchState> SubRegionsTokens => this.subRegionsStates;

        public IReadOnlyCollection<KeyValuePair<RuleCheckResult, List<char>>> RegionSymbols => this.regionSymbols;

        public int Index { get; set; }

        #endregion Properties

        #region Fields

        private readonly List<RegionSearchState> subRegionsStates;

        private readonly Dictionary<RuleCheckResult, List<char>> regionSymbols;

        private RuleCheckResult lastCheckResult;

        #endregion Fields

        #region Methods

        public void AddSubRegion(RegionSearchState regionToken)
        {
            this.subRegionsStates.Add(regionToken);
        }

        public void AddSymbol(char symbol, RuleCheckResult checkResult)
        {
            if (!this.regionSymbols.TryGetValue(checkResult, out var symbols))
            {
                symbols = new List<char>();
                this.regionSymbols.Add(checkResult, symbols);
            }

            symbols.Add(symbol);
        }

        public void SetLastCheckResult(RuleCheckResult checkResult)
        {
            this.lastCheckResult = checkResult;
        }

        public string GetTitle()
        {
            return string.Concat(this.regionSymbols.Where(kvp => kvp.Key.HasFlag(RuleCheckResult.TitlePart)).SelectMany(kvp => kvp.Value));
        }

        public RegionToken CreateToken()
        {
            return new RegionToken
            {
                RegionMdxType = this.RegionRule.MdxType,
                Value = this.GetTitle(),
                SubRegionsTokens = this.subRegionsStates.Select(_ => _.CreateToken()).ToArray()
            };
        }

        #endregion Methods

        #region Constructors

        public RegionSearchState(IRegionRule regionRule)
        {
            this.RegionRule = regionRule ?? throw new ArgumentNullException(nameof(regionRule));
            this.subRegionsStates = new List<RegionSearchState>();
            this.regionSymbols = new Dictionary<RuleCheckResult, List<char>>();
        }

        #endregion Constructors
    }
}

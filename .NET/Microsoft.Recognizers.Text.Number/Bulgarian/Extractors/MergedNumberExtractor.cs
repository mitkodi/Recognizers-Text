﻿using System.Collections.Concurrent;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Bulgarian;

namespace Microsoft.Recognizers.Text.Number.Bulgarian
{
    internal class MergedNumberExtractor : BaseMergedNumberExtractor
    {
        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), MergedNumberExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), MergedNumberExtractor>();

        public MergedNumberExtractor(NumberMode mode, NumberOptions options)
        {
            NumberExtractor = Bulgarian.NumberExtractor.GetInstance(mode, options);
            RoundNumberIntegerRegexWithLocks =
                new Regex(NumbersDefinitions.RoundNumberIntegerRegexWithLocks, RegexOptions.Singleline);
            ConnectorRegex =
                new Regex(NumbersDefinitions.ConnectorRegex, RegexOptions.Singleline);
        }

        public sealed override BaseNumberExtractor NumberExtractor { get; set; }

        public sealed override Regex RoundNumberIntegerRegexWithLocks { get; set; }

        public sealed override Regex ConnectorRegex { get; set; }

        public static MergedNumberExtractor GetInstance(
            NumberMode mode = NumberMode.Default,
            NumberOptions options = NumberOptions.None)
        {
            var cacheKey = (mode, options);
            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new MergedNumberExtractor(mode, options);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentMdx.Lexer
{
    internal sealed class AdvancedCharEnumerator : IEnumerator<char?>
    {
        #region Properties

        public char? Current => this.index == -1 ? null as char? : this.symbols[this.index];

        object IEnumerator.Current => Current;

        public int Index => this.index;

        public string Source { get; }

        #endregion Properties

        #region Fields

        private int index;

        private readonly char[] symbols;

        #endregion Fields

        #region Methods

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (this.index < this.Source.Length - 1 && this.index != -1)
            {
                this.index++;
                return true;
            }
            else
            {
                this.index = -1;
                return false;
            }
        }

        public bool MoveNextWhileWhiteSpace()
        {
            if (this.index == -1)
                return false;

            while (char.IsWhiteSpace(this.symbols[this.index]))
            {
                if (!this.MoveNext())
                    break;
            }

            return this.index != -1;
        }

        public void Reset()
        {
            this.index = 0;
        }

        public void SetIndex(int index)
        {
            this.index = index >= this.Source.Length ? -1 : index;
        }

        #endregion Methods

        #region Constructors

        public AdvancedCharEnumerator(string source)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.symbols = source.ToCharArray();
            this.index = 0;
        }

        #endregion Constructors
    }
}

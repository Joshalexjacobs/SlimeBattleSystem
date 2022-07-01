using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeBattleSystem.Tests
{
    public class RandomMock : Random
    {
        private List<int> _valueStack;
        
        public RandomMock(int [] values)
        {
            _valueStack = values.ToList();
        }

        public override int Next()
        {
            var item = _valueStack[_valueStack.Count - 1];

            _valueStack.RemoveAt(_valueStack.Count - 1);

            return item;
        }

        public override int Next(int maxValue)
        {
            return Next();
        }

        public override int Next(int minValue, int maxValue)
        {
            return Next();
        }
    }
}
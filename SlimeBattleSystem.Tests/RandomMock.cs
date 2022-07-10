using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeBattleSystem.Tests;

/// <summary>
///   Overrides the Random class. Allows the user to pre-determine which values will be returned from any of the Next()
///   functions.
/// </summary>
public class RandomMock : Random {
  private readonly List<int> _valueStack;

  public RandomMock(int[] values) {
    _valueStack = values.ToList();
  }

  /// <summary>
  ///   Pops the next value from the stack of integers provided in the constructor.
  /// </summary>
  /// <returns>int</returns>
  public override int Next() {
    var item = _valueStack[_valueStack.Count - 1];

    _valueStack.RemoveAt(_valueStack.Count - 1);

    return item;
  }

  public override int Next(int maxValue) {
    return Next();
  }

  public override int Next(int minValue, int maxValue) {
    return Next();
  }
}
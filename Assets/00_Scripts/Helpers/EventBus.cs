using System;
using System.Collections.Generic;
using UniRx;

namespace _00_Scripts.Helpers
{
  public static class EventBus
  {
    private static readonly Subject<object> Bus = new();

    public static void Publish<T>(T message)
    {
      Bus.OnNext(message);
    }

    public static IObservable<T> On<T>()
    {
      return Bus.OfType<object, T>();
    }
  }
}

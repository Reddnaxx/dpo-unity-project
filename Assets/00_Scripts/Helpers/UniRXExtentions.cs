using System;

using UniRx;

using UnityEngine.InputSystem;

namespace _00_Scripts.Helpers
{
  public static class UniRxExtensions {
    public static IObservable<InputAction.CallbackContext> OnPerformedAsObservable(this InputAction action) =>
      Observable.FromEvent<InputAction.CallbackContext>(
        h => action.performed += h,
        h => action.performed -= h
      );
    
    public static IObservable<InputAction.CallbackContext> OnCanceledAsObservable(this InputAction action) =>
      Observable.FromEvent<InputAction.CallbackContext>(
        h => action.canceled += h,
        h => action.canceled -= h
      );
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Infrastructure
{
    public class EAPWrapper
    {
		public static Task WrapEapToTap<TObject, TEventHandler, TEventArgs, TResult>(TObject obj,
	Func<TEventArgs, TResult> resultSelector,
	Action<TObject, object> eapAction,
	Action<TObject, TEventHandler> addEventHandler,
	Action<TObject, TEventHandler> removeEventHandler,
	Func<Action<object, TEventArgs>, TEventHandler> eventHandlerFactory)
	where TEventArgs : AsyncCompletedEventArgs
		{
			var tcs = new TaskCompletionSource<object>();
			var handler = default(TEventHandler);
			handler = eventHandlerFactory((sender, eventArgs) =>
			{
				var tcsLocal = (TaskCompletionSource<object>)eventArgs.UserState;
				try
				{
					if (eventArgs.Error != null)
					{
						tcsLocal.SetException(eventArgs.Error);
						return;
					}
					if (eventArgs.Cancelled)
					{
						tcsLocal.SetCanceled();
						return;
					}
					tcsLocal.SetResult(resultSelector(eventArgs));
					return;
				}
				finally
				{
					removeEventHandler(obj, handler);
				}
			});
			addEventHandler(obj, handler);
			eapAction(obj, tcs);
			return tcs.Task;
		}
	}
}

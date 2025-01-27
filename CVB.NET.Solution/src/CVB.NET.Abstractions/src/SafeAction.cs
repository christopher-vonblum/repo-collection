namespace CVB.NET.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SafeAction<TArg1> : IEquatable<SafeAction<TArg1>>
	{
		private readonly List<Action<TArg1>> handlers = new List<Action<TArg1>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1> operator +(SafeAction<TArg1> a, Action<TArg1> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1> operator -(SafeAction<TArg1> a, Action<TArg1> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1> operator +(SafeAction<TArg1> a, SafeAction<TArg1> b)
		{
			SafeAction<TArg1> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1> operator -(SafeAction<TArg1> a, SafeAction<TArg1> b)
		{
			SafeAction<TArg1> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1> && Equals((SafeAction<TArg1>)obj);
		}

		public bool Equals(SafeAction<TArg1> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1> x, SafeAction<TArg1> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1> x, SafeAction<TArg1> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2> : IEquatable<SafeAction<TArg1, TArg2>>
	{
		private readonly List<Action<TArg1, TArg2>> handlers = new List<Action<TArg1, TArg2>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2> operator +(SafeAction<TArg1, TArg2> a, Action<TArg1, TArg2> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2> operator -(SafeAction<TArg1, TArg2> a, Action<TArg1, TArg2> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2> operator +(SafeAction<TArg1, TArg2> a, SafeAction<TArg1, TArg2> b)
		{
			SafeAction<TArg1, TArg2> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2> operator -(SafeAction<TArg1, TArg2> a, SafeAction<TArg1, TArg2> b)
		{
			SafeAction<TArg1, TArg2> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2> && Equals((SafeAction<TArg1, TArg2>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2> x, SafeAction<TArg1, TArg2> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2> x, SafeAction<TArg1, TArg2> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3> : IEquatable<SafeAction<TArg1, TArg2, TArg3>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3>> handlers = new List<Action<TArg1, TArg2, TArg3>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3> operator +(SafeAction<TArg1, TArg2, TArg3> a, Action<TArg1, TArg2, TArg3> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3> operator -(SafeAction<TArg1, TArg2, TArg3> a, Action<TArg1, TArg2, TArg3> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3> operator +(SafeAction<TArg1, TArg2, TArg3> a, SafeAction<TArg1, TArg2, TArg3> b)
		{
			SafeAction<TArg1, TArg2, TArg3> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3> operator -(SafeAction<TArg1, TArg2, TArg3> a, SafeAction<TArg1, TArg2, TArg3> b)
		{
			SafeAction<TArg1, TArg2, TArg3> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3> && Equals((SafeAction<TArg1, TArg2, TArg3>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3> x, SafeAction<TArg1, TArg2, TArg3> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3> x, SafeAction<TArg1, TArg2, TArg3> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4> a, Action<TArg1, TArg2, TArg3, TArg4> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4> a, Action<TArg1, TArg2, TArg3, TArg4> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4> a, SafeAction<TArg1, TArg2, TArg3, TArg4> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4> a, SafeAction<TArg1, TArg2, TArg3, TArg4> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4> x, SafeAction<TArg1, TArg2, TArg3, TArg4> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4> x, SafeAction<TArg1, TArg2, TArg3, TArg4> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> y)
		{
			return !(x == y);
		}
	}
	
	public class SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> : IEquatable<SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>>
	{
		private readonly List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>> handlers = new List<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>>();

		public SafeAction()
		{
		}

		public SafeAction(IEnumerable<Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>> handlers)
		{
			if(handlers == null)
			{
				throw new ArgumentNullException(nameof(handlers));
			}

			this.handlers = handlers.ToList();
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> b)
		{
			lock (a.handlers)
			{
				a.handlers.Add(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> a, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> b)
		{
			lock (a.handlers)
			{
				a.handlers.Remove(b);
			}

			return a;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> operator +(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(a.handlers.Concat(b.handlers));
			}

			return newAction;
		}

		public static SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> operator -(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> a, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> b)
		{
			SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> newAction;
			
			lock (a.handlers)
			{
				newAction = new SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(a.handlers.Concat(b.handlers));
			}
			
			newAction.handlers.RemoveAll(h => b.handlers.Contains(h));
			
			return a;
		}

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, TArg16 arg16)
		{
			List<Exception> handlerExceptions = new List<Exception>();

			foreach (var handler in handlers)
			{
				try
				{
					handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
				}
				catch (Exception ex)
				{
					handlerExceptions.Add(ex);
				}
			}

			if (handlerExceptions.Any())
			{
				throw new AggregateException(handlerExceptions);
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> && Equals((SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>)obj);
		}

		public bool Equals(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> action)
		{
			return action != null && handlers.All(h => action.handlers.Contains(h));
		}

		public override int GetHashCode()
		{
			return handlers.Aggregate(1, (current, h) => current ^ h.GetHashCode());
		}

		public static bool operator ==(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> y)
		{
			return x != null && x.Equals(y);
		}

		public static bool operator !=(SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> x, SafeAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> y)
		{
			return !(x == y);
		}
	}
}

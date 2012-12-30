using System.ComponentModel;

namespace Magic.UI.Helpers
{

	public class Observable<T> : INotifyPropertyChanged
	{
		private T value;
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public Observable()
		{
		}

		public Observable(T value)
		{
			this.value = value;
		}

		public T Value
		{
			get { return value; }
			set
			{
				if (this.value != null && this.value.Equals(value))
				{
					return;
				}

				this.value = value;
				PropertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		public static implicit operator T(Observable<T> val)
		{
			return val.value;
		}

	}

}

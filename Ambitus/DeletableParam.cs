using System;

namespace Ambitus
{
	public class DeletableParam<T>
	{
		private T value;
		private T valueOnDelete;
		private bool delete;

		public T Value
		{
			get
			{
				if (delete)
				{
					return valueOnDelete;
				}
				else
				{
					return value;
				}
			}
			set
			{
				delete = false;
				this.value = value;
			}
		}

		public bool Delete
		{
			get
			{
				return delete;
			}
			set
			{
				if (value)
				{
					this.value = default(T);
				}
				delete = value;
			}
		}

		public DeletableParam(T valueOnDelete)
		{
			this.valueOnDelete = valueOnDelete;
		}

		public override string ToString()
		{
			return Value == null ? String.Empty : Value.ToString();
		}
	}
}

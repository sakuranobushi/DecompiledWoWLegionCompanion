using System;
using System.Runtime.CompilerServices;

namespace WowStaticData
{
	public class RewardPackXCurrencyTypeRec
	{
		public int CurrencyTypeID
		{
			get;
			private set;
		}

		public int ID
		{
			get;
			private set;
		}

		public int Quantity
		{
			get;
			private set;
		}

		public int RewardPackID
		{
			get;
			private set;
		}

		public RewardPackXCurrencyTypeRec()
		{
		}

		public void Deserialize(string valueLine)
		{
			int num = 0;
			int num1 = 0;
			int num2 = 0;
			do
			{
				num = valueLine.IndexOf('\t', num1);
				if (num >= 0)
				{
					string str = valueLine.Substring(num1, num - num1).Trim();
					this.DeserializeIndex(num2, str);
					num2++;
				}
				num1 = num + 1;
			}
			while (num > 0);
		}

		private void DeserializeIndex(int index, string valueText)
		{
			switch (index)
			{
				case 0:
				{
					this.ID = Convert.ToInt32(valueText);
					break;
				}
				case 1:
				{
					this.RewardPackID = Convert.ToInt32(valueText);
					break;
				}
				case 2:
				{
					this.CurrencyTypeID = Convert.ToInt32(valueText);
					break;
				}
				case 3:
				{
					this.Quantity = Convert.ToInt32(valueText);
					break;
				}
			}
		}
	}
}
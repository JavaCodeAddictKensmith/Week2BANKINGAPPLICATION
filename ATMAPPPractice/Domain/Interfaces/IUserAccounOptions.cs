using System;
namespace ATMAPPPractice.Domain.Interfaces
{
	public interface IUserAccountActions
	{
		void CheckBalance();
		void PlaceDeposit();
		void MakeWithdrawal();
	}
}


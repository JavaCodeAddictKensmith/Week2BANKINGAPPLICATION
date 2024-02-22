using System;
using ATMAPPPractice.Domain.Enums;

namespace ATMAPPPractice.Domain.Interfaces
{
	public interface ITransaction
	{
		void InserTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc);
		void ViewTransaction();
	}
}


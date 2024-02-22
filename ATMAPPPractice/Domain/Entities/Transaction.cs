using System;
using ATMAPPPractice.Domain.Enums;

namespace ATMAPPPractice.Domain.Entities
{
	public class Transaction
	{
		public long TransactionId { get; set; }
		public long UserBankAccountId { get; set; }
		public DateTime TransactionDate { get; set; }
		public TransactionType TransactionType { get; set; }
		public string Description { get; set;
		}

		public Decimal TransactionAmount { get; set; }



	}

}


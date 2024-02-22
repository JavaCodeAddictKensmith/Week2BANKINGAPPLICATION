using System;
namespace ATMAPPPractice.Domain.Entities
{
	public class InternalTransfer
	{
		public decimal TransferAmount { get; set; }
        public long ReciepeintBankAccountNumber { get; set; }
        public string ReceipeintBankAccountName { get; set; }
    }
}


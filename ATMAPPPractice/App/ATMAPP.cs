
using ATMAPPPractice.Domain.Interfaces;
using ATMAPPPractice.UI;
using ATMAPPPractice.Domain.Entities;
using ATMAPPPractice.Domain.Interfaces;
using ATMAPPPractice.UI;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using ATMAPPPractice.Domain.Enums;
using ConsoleTables;

namespace ATMApp
{
    public class ATMApp : IUserLogin, IUserAccountActions,ITransaction
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen screen;

        public ATMApp()
        {
            screen = new AppScreen();
        }
        public void Run()
        {
            AppScreen.Welcome();
          CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuOption();
            }

        }

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id=1, FullName = "Obinna Ezeh", AccountNumber=123456,CardNumber =321321, CardPin=123123,AccountBalance=50000.00m,IsLocked=false},
                new UserAccount{Id=2, FullName = "Amaka Hope", AccountNumber=456789,CardNumber =654654, CardPin=456456,AccountBalance=4000.00m,IsLocked=false},
                new UserAccount{Id=3, FullName = "Femi Sunday", AccountNumber=123555,CardNumber =987987, CardPin=789789,AccountBalance=2000.00m,IsLocked=true},
            };
           _listOfTransactions = new List<Transaction>();
        }

        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                    if (isCorrectLogin == false)
                    {
                        Utility.PrintMessage("\nInvalid card number or PIN.", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if (selectedAccount.IsLocked)
                        {
                            AppScreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();
                }
            }

        }

        private void ProcessMenuOption()
        {
            switch(Validator.Convert<int>("an option"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = screen
                        .InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect your ATM Card");
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid option.", false);
                    Console.WriteLine(" Default action happens her");
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your Account Balance is : {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nOnly multiples of 500 and 1000 naira allowed.\n");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

            //simulate counting
            Console.WriteLine("\nChecking and Counting bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            //some gaurd clause
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false); ;
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.PrintMessage($"Enter deposit amount in multiples of 500 or 1000. Try again.", false);
                return;
            }

            if (PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage($"You have cancelled your action.", false);
                return;
            }

            //bind transaction details tp transaction object
            InserTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");


            //Update account Balance
            selectedAccount.AccountBalance += transaction_amt;
            //Print sucess message to the screen

            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was successful", true);

        }

        public void MakeWithdrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if(selectedAmount == -1)
            {
                MakeWithdrawal();
                return;

            }else if (selectedAmount!=0)
            {
                transaction_amt = selectedAmount;

            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
                return;
            }

           

            if(transaction_amt> selectedAmount)
            //Input validation
            if(transaction_amt<= 0)
            {
                Utility.PrintMessage("Amount needs to be  greater than zero, try again");
            }

            if (transaction_amt % 500 != 0)
            {

                Utility.PrintMessage("You can only withdraw amount in multiple of 500");
                return;

            }

            //Business Logic Validations
            if(transaction_amt> selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdraw {Utility.FormatAmount(transaction_amt)}", false);
                return;
               
            }

            if ((selectedAccount.AccountBalance - transaction_amt)<minimumKeptAmount) {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have a minimum" + $"{Utility.FormatAmount(minimumKeptAmount)}");
                return;
            }
            //Bind the withdrawal details to transaction object
            InserTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt,"");

            //update account balance
            selectedAccount.AccountBalance -= transaction_amt;

            //suceess message
            Utility.PrintMessage($"You have successfully withdrawn" + $"{Utility.FormatAmount(transaction_amt)}.", true);







        }


        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;

            Console.WriteLine("\nSummary");
            Console.WriteLine("------");
            Console.WriteLine($"{AppScreen.cur}1000 X {thousandNotesCount} = {1000 * thousandNotesCount}");
            Console.WriteLine($"{AppScreen.cur}500 X {fiveHundredNotesCount} = {500 * fiveHundredNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);

        }

        public void InserTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTranscationId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc


            };
            _listOfTransactions.Add(transaction);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _listOfTransactions.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();

            //check if there is a transaction yet
            if(filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no trasnaction yet", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type","Description","Amount "+ AppScreen.cur);
                foreach(var tran in filteredTransactionList)
                {
                    table.AddRow(tran.TransactionId,tran.TransactionDate, tran.TransactionType,tran.Description,tran.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count}", true);

            }
        }

        private void ProcessInternalTransfer(InternalTransfer
            internalTransfer)
        {
            if (internalTransfer.TransferAmount < +0)
            {
                Utility.PrintMessage("Amount needs to be morethan zero. Try again");
                return;
            }

            //check senders account balance
            if(internalTransfer.TransferAmount> selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough balance to transfer  {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }

            //check the minimum kept amount

            if(selectedAccount.AccountBalance - internalTransfer.TransferAmount< minimumKeptAmount
                )
            {
                Utility.PrintMessage($"Transfer failed. Your account needs to have minimum {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }


            //cmheck receiver's account number is valid
            var selectedBankAccounttReceiver = (from userAcc in userAccountList
                                                where userAcc.AccountNumber == internalTransfer.ReciepeintBankAccountNumber
                                                select userAcc
                                                ).FirstOrDefault();

            if (selectedBankAccounttReceiver == null)
            {
                Utility.PrintMessage("Tranfer failed. Receiver bank accoun number is invalid", false);
                return;
            };
            //check receiver's name

            if(selectedBankAccounttReceiver.FullName != internalTransfer.ReceipeintBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Recipient bank account name does not match", false);
                return;
            }


            ///add transaction to transaction record - sender

            InserTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Tasnferred " + $" to {selectedBankAccounttReceiver.AccountNumber} ({selectedBankAccounttReceiver.FullName
                })");

            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;
            //add transaction record-receiver

            InserTransaction(selectedBankAccounttReceiver.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "You have transferred from" + $"{selectedAccount.AccountNumber} ({selectedAccount.FullName})");

            // update receiver's account balance

            selectedBankAccounttReceiver.AccountBalance += internalTransfer.TransferAmount;
            //print sucess message
            Utility.PrintMessage($"You have successfully transfered" + $"{Utility.FormatAmount(internalTransfer.TransferAmount)} to " + $"{internalTransfer.ReceipeintBankAccountName}", true);

        }
    }
}

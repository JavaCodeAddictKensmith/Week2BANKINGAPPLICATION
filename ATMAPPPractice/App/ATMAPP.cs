
using ATMAPPPractice.Domain.Interfaces;
using ATMAPPPractice.UI;
using ATMAPPPractice.Domain.Entities;
using ATMAPPPractice.Domain.Interfaces;
using ATMAPPPractice.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using ATMAPPPractice.Domain.Enums;

namespace ATMApp
{
    public class ATMApp : IUserLogin, IUserAccountActions
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        public void Run()
        {
            AppScreen.Welcome();
          CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            AppScreen.DisplayAppMenu();
            ProcessMenuOption();

        }

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id=1, FullName = "Obinna Ezeh", AccountNumber=123456,CardNumber =321321, CardPin=123123,AccountBalance=50000.00m,IsLocked=false},
                new UserAccount{Id=2, FullName = "Amaka Hope", AccountNumber=456789,CardNumber =654654, CardPin=456456,AccountBalance=4000.00m,IsLocked=false},
                new UserAccount{Id=3, FullName = "Femi Sunday", AccountNumber=123555,CardNumber =987987, CardPin=789789,AccountBalance=2000.00m,IsLocked=true},
            };
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
                    Console.WriteLine("Placing Deposit");
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    Console.WriteLine("Making Withdrawal");
                    break;
                case (int)AppMenu.InternalTransfer:
                    Console.WriteLine("Making internal Trasnfer");
                    break;
                case (int)AppMenu.ViewTransaction:
                    Console.WriteLine("Viewing Transactions...");
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
            throw new NotImplementedException();
        }

        public void MakeWithdrawal()
        {
            throw new NotImplementedException();
        }
    }
}

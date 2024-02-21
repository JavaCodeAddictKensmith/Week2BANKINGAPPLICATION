using System;
using ATMAPPPractice.Domain.Entities;

namespace ATMAPPPractice.UI
{
    public static class AppScreen
    {
        internal const string cur = "N ";
        internal static void Welcome()
        {
            //clears the console screen
            Console.Clear();
            //sets the title of the console window
            Console.Title = "My ATM App";
            //sets the text color or foreground color to white
            Console.ForegroundColor = ConsoleColor.White;

            //set the welcome message 
            Console.WriteLine("\n\n-----------------Welcome to My ATM App-----------------\n\n");
            //prompt the user to insert atm card
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("Note: Actual ATM machine will accept and validate" +
                " a physical ATM card, read the card number and validate it.");
            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validator.Convert<long>("your card number.");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card PIN"));
            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking card number and PIN...");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest branch" +
                " to unlock your account. Thank you.", true);
            Environment.Exit(1);
        }

        internal static void WelcomeCustomer(string FullName)
        {
            Console.WriteLine($"Welcome back {FullName}");
            Utility.PressEnterToContinue();
        }
        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("---MY ATM APP MENU----");
            Console.WriteLine(":            :");
            Console.WriteLine("1. Account Balance         :");
            Console.WriteLine("2. Cash Deposit         :");
            Console.WriteLine("3.Withdrawal        :");
            Console.WriteLine("5. Transactions         :");
            Console.WriteLine("6. Logout         :");
        }

        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank you for using my ATM APP");
            Utility.PrintDotAnimation();
            Console.Clear();
        }
    }
}


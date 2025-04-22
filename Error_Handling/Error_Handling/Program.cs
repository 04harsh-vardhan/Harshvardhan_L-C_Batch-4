public class ATM
{
    private long cash;
    private UserAccount user;
    private bool isServerDown = false;
    private int wrongPinCount = 0;
    private bool isLimitExceeded = false;
    public long Withdraw(long amount, UserAccount user)
    {
        if (user.GetBalance() <= 0)
        {
            throw new InsufficientCashException("User Does Not have sufficient Balance");
        }
        if (amount > cash)
        {
            cash -= amount;
        }
        else
        {
            throw new InsufficientCashException("Insufficient cash in ATM machine");
        }
        return amount;
    }
    public void GetUserDetail()
    {
        // some API call to get user
        if (isServerDown)
        {
            throw new ServerDownException("Server is Currently Down:  Unable to connect with server");
        }
    }
    public bool CheckDailyLimit(int totalCashWithdrawed)
    {
        // some logic to check daily limit
        if (isLimitExceeded)
        {
            throw new DailyLimitExceedException("Daily Limit To Withdraw has been exceeded");
        }
        return false;
    }
    public bool CheckPin(int pin)
    {
        // some Logic to Check Pin
        if (wrongPinCount > 3)
        {
            throw new PinLimitExceedException("Pin limit has been exceeded");
        }
        return true;
    }

}

public class UserAccount
{
    private long balance;
    private long accountNumber;
    public long GetBalance()
    {
        return balance;
    }
}

public class InsufficientCashException : Exception
{
    public InsufficientCashException(string message) : base(message) { }
}

public class ServerDownException : Exception
{
    public ServerDownException(string message) : base(message) { }
}

public class PinLimitExceedException : Exception
{
    public PinLimitExceedException(string message) : base(message) { }
}

public class DailyLimitExceedException : Exception
{
    public DailyLimitExceedException(string message) : base(message) { }
}
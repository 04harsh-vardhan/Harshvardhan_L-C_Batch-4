public
class Customer
{

private
    String firstName;

private
    String lastName;

private
    Wallet myWallet;

public
    String getFirstName()
    {

        return firstName;
    }

public
    String getLastName()
    {

        return lastName;
    }

public
    Wallet getWallet()
    {

        return myWallet;
    }

}

public class Wallet
{

private
    float value;

public
    float getTotalMoney()
    {

        return value;
    }

public
    void setTotalMoney(float newValue)
    {

        value = newValue;
    }

public
    void addMoney(float deposit)
    {

        value += deposit;
    }

public
    void subtractMoney(float debit)
    {

        value -= debit;
    }

}
/*
                               Problem Statement
Client code…. assuming some delivery boy wants to get his payment

// code from some method inside the delivery boy class... payment = 2.00; //

“I want my two dollars!”

Wallet theWallet = myCustomer.getWallet();

if (theWallet.getTotalMoney() > payment) {

theWallet.subtractMoney(payment);

} else {

// come back later and get my money

}
*/

/*
                                   Solution
   Here as we can see that the delivery boy class has access to the wallet which
   it should not have access to because Wallet should be a personal property not public
   and delivery boy class can manipulate the wallet to get more money
*/
// Correct Customer class

public class Customer
{

private
    String firstName;

private
    String lastName;

private
    Wallet myWallet;

public
    String getFirstName()
    {

        return firstName;
    }

public
    String getLastName()
    {

        return lastName;
    }

public
    void payMoney(int payment)
    {
        Wallet myWallet = new Wallet();
        if (myWallet.getTotalMoney() > payment)
        {

            theWallet.subtractMoney(payment);
        }
        else
        {

            // come back later and get my money
        }
    }
}
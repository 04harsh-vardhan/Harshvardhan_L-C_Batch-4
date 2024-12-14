def getArmstrongNumber(userInput):
    # Initializing Sum and Number of Digits
    armstrongNumber = 0
    totalDigitCountInNumber = 0

    # Calculating Number of individual digits
    userInputCopy = userInput
    while userInputCopy > 0:
        totalDigitCountInNumber = totalDigitCountInNumber + 1
        userInputCopy = userInputCopy // 10

    # Finding Armstrong Number
    userInputCopy = userInput
    for _ in range(1, userInputCopy + 1):
        unitPlaceDigit = userInputCopy % 10
        armstrongNumber = armstrongNumber + (unitPlaceDigit ** totalDigitCountInNumber)
        userInputCopy //= 10
    return armstrongNumber


# End of Function

# User Input
userInput = int(input("\nPlease Enter the Number to Check for Armstrong: "))

if (userInput == getArmstrongNumber(userInput)):
    print("\n %d is Armstrong Number.\n" % userInput)
else:
    print("\n %d is Not a Armstrong Number.\n" % userInput)
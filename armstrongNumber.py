def getArmstrongNumber(userInput):
    # Initializing Sum and Number of Digits
    armstrongNumber = 0
    totalDigits = 0

    # Calculating Number of individual digits
    currentNumber = userInput
    while currentNumber > 0:
        totalDigits = totalDigits + 1
        currentNumber = currentNumber // 10

    # Finding Armstrong Number
    currentNumber = userInput
    for _ in range(1, currentNumber + 1):
        unitPlaceDigit = currentNumber % 10
        armstrongNumber = armstrongNumber + (unitPlaceDigit ** totalDigits)
        currentNumber //= 10
    return armstrongNumber


# End of Function

# User Input
userInput = int(input("\nPlease Enter the Number to Check for Armstrong: "))

if (userInput == getArmstrongNumber(userInput)):
    print("\n %d is Armstrong Number.\n" % userInput)
else:
    print("\n %d is Not a Armstrong Number.\n" % userInput)

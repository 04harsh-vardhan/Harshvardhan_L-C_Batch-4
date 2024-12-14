def validateUserInput(userInput):
    if userInput.isdigit() and 1<= int(userInput) <=100:
        return True
    else:
        return False

def main():
    randomNumber=random.randint(1,100)
    hasCorrectlyGuessedNumber=False
    userGuessedNumber=input("Guess a number between 1 and 100:")
    totalGuesses=0
    while not hasCorrectlyGuessedNumber:
        if not validateUserInput(userGuessedNumber):
            userGuessedNumber=input("I wont count this one Please enter a number between 1 to 100")
            continue
        else:
            totalGuesses+=1
            userGuessedNumber=int(userGuessedNumber)

        if userGuessedNumber<randomNumber:
            userGuessedNumber=input("Too low. Guess again")
        elif userGuessedNumber>randomNumber:
            userGuessedNumber=input("Too High. Guess again")
        else:
            print("You guessed it in",totalGuesses,"guesses!")
            hasCorrectlyGuessedNumber=True


main()
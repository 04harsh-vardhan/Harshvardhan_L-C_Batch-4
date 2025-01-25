main()

def main():
    startGame()
        
def startGame():
    targetNumber=random.randint(1,100)
    isUserGuessedSuccess=False
    userInput=input("Guess a number between 1 and 100:")
    totalGuesses=0
    while not isUserGuessedSuccess:
        isInputValid,message = validateInput(userInput)
        if not isInputValid:
            userInput = input(message)
            continue
        totalGuesses+=1
        isUserGuessedSuccess,message = checkUserGuess(userInput,targetNumber)
        if not isUserGuessedSuccess:
            userInput = input(message)
        print("You guessed it in",totalGuesses,"guesses!")

def isValidUserInput(input):
    if input.isdigit() and 1<= int(input) <=100:
        return True
    else:
        return False

def checkUserGuess(userInput,targetNumber):
    isSuccess = False
    message = None
    if userInput<targetNumber:
        message="Too low. Guess again"
    elif userInput>targetNumber:
        message="Too High. Guess again"
    else:
        isSuccess=True
    return isSuccess,message;

def validateInput(userInput):
    isInputValid = False
    message = None
    if not isValidUserInput(userInput):
        message="I wont count this one Please enter a number between 1 to 100"
    else:
        userInput=int(userInput)
        isInputValid = True
    return isInputValid,message

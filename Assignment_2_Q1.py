main()

def main():
    startGame()
        
def startGame():
    number=random.randint(1,100)
    isUserGuessedSuccess=False
    userInput=input("Guess a number between 1 and 100:")
    totalGuesses=0
    while not isUserGuessedSuccess:
        isInputValid,message = validateInput(userInput)
        if not isInputValid:
            userInput = input(message)
            continue
        totalGuesses+=1
        isUserGuessedSuccess,message = checkUserGuess(userInput,number)
        if not isUserGuessedSuccess:
            userInput = input(message)
        print("You guessed it in",totalGuesses,"guesses!")

def fun(input):
    if input.isdigit() and 1<= int(input) <=100:
        return True
    else:
        return False

def checkUserGuess(userInput,number):
    isSuccess = False
    message = None
    if userInput<number:
        message="Too low. Guess again"
    elif userInput>number:
        message="Too High. Guess again"
    else:
        isSuccess=True
    return isSuccess,message;

def validateInput(userInput):
    isInputValid = False
    message = None
    if not fun(userInput):
        message="I wont count this one Please enter a number between 1 to 100"
    else:
        userInput=int(userInput)
        isInputValid = True
    return isInputValid,message
import random
def generateRandomInteger(diceSides):
    randomNumber=random.randint(1, diceSides)
    return randomNumber


def main():
    diceSides=6
    isUserRolling=True
    while isUserRolling:
        userInput=input("Ready to roll? Enter Q to Quit")
        if userInput.lower() !="q":
            randomNumber=generateRandomInteger(diceSides)
            print("You have rolled a",randomNumber)
        else:
            isUserRolling=False
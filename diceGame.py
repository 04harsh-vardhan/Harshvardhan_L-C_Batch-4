import random
def generateRandomNumber(diceSides):
    result=random.randint(1, diceSides)
    return result


def main():
    diceSides=6
    isUserTurn=True
    while isUserTurn:
        userInput=input("Ready to roll? Enter Q to Quit")
        if userInput.lower() !="q":
            result=generateRandomNumber(diceSides)
            print("You have rolled a",result)
        else:
            isUserTurn=False

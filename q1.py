import random
def rollDice(diceSides):
    rollResult=random.randint(1, diceSides)
    return rollResult


def main():
    diceSides=6
    continueDiceRolling=True
    while continueDiceRolling:
        userInput=input("Ready to roll? Enter Q to Quit")
        if userInput.lower() !="q":
            rollResult=rollDice(diceSides)
            print("You have rolled a",rollResult)
        else:
            continueDiceRolling=False
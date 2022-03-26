# Wordle Clone
## Description

This is a basic wordle clone. All functions and controls are the same as the real game. NOTE: There is no keyboard at the bottom of the application unlike the real wordle, this
is a to-be-added functin that I just haven't gotten around to due to Exams and School.

## Complexity
The code is currently not amazingly optimized and many algorithms are brute forced in order to solve many problems. One main optimization is the usage of
a hash in oder to access whether or not it is a valid guess in an average of O(1) time. However the complexity of the algorithm to mark letters as 'yellow,'
a letter that is in the wrong spot but is in the word, is O(N^2). I used a simple brute force algorithm as N is always a very small number and never adds much complexity.

## To Be Added
- Random Guess Button
- AI that solves puzzle
- Keyboard UI
- Uploading to Web so that it can be played
- A Nicer looking Window when you win the game and make a wrong guess


## Controls
 As this is a simple wordle game, the controls are all of the alphabet keys, A - Z, and backspace to delete letters that you have guessed. However I added two more buttons, 1 and 2. By pressing 1, you reset the game and 
 by pressing 2 you can see what the word is automatically. 

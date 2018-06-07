# War Requirements Doc
This document provides goals and requirements for creating the classic card game "War" for demonstration and review.
## Goals
+ Physically create an electronic version of the classic card game War.
+ Showcase knowledge of design processes and programming patterns
## Background
+ Is this section necessary?
## Assumptions
+ The game will be played by a single player against a single computer opponent.
+ Ace cards are high (they have the highest value, 14)
+ The player decks will be reshuffled when they are empty (assuming they have collected cards)
## User Stories
1. A User wants to shuffle the deck and deal the cards.
2. A User wants to play a card from their deck and match against the opponent.
3. A User wants to collect the opponent's card if they win the match.
4. A User wants to start War if their card is matched by their Opponent.
5. A User wants to collect all cards in a War if they win the War.
6. A User should win if they collect all the cards in play.
7. A User should lose if they no longer have any cards in play.

# Software Implementation

## Game Flow
The general flow of a game using the data and modules described above.

1. The Manager Module checks for a win case (either player or opponent control 52 cards across both their Deck and Claim piles)
2. If no win condition is satisfied, the Manger Module will request a single Card instance to be drawn from both Player and Computer decks
	+ If no card is availble from the Deck, the Claims pile is moved into the Deck and shuffled, and then a card is drawn from the Deck
3. The Manager Module then calls the Card Compare routine to determine the outcome of the round.
4. If the outcome is a win for the player, the Manager Module moves both cards from play into the player's Claims pile.  If the outcome is a lose, the Manager Module moves both cards into the opponent's Claims pile.  Return to Step 1.
	+ If the outcome is a tie, goto step 5 to resolve the tie.
5. 'War' has been started.  Goto step 3 and repeat until the outcome is either Win or Lose.  If the outcome is Win, the Manager Module moves all cards from play into the player's Claims pile, and if it is a Lose, the Manager Module moves all cards from play into the opponent's Claims pile.

## Data
What data needs to be stored and considered for scoring, gameplay, UI, etc.

1. Card Data - Each card in a deck (52 cards) will need representation
	+ Card Rank (2 - 13) - The "strength" of a card.  A higher rank will beat a lower rank card.  Cards of equal rank will begin a War.
	+ Card Image - The associated image to be displayed with the card when it is played.
2. Player Data - A dataclass associated with each player (human and computer) for recording statistics for use during debugging.
	+ 
## Modules
The various sub components of the game that will be combined to form the game.

1. Deck Module - A deck is a set of cards that can be  drawn from or added to.
	+ A Deck module may either be the deck a player/computer is drawing from, or a deck they are depositing claimed/won cards into.
	+ A Deck may be shuffled at any time
2. Manager Module - The manager module controls the general flow of the game by keeping track of turns, managing decks (and checking for win cases), and managing use of the Card Comparation Module.
	+ The Manager Module is a singleton - there should only be one instance of it
	+ The Manager Module will request cards from player and opponent Decks, and deposit cards to player and opponent Claims.
	+ The Manager Module will process request cards with the Card Comparator.
	+ The Manager Module will detect a win condition and end the game if it is met.

3. Card Comparator Module - The Card Comparator module is the main decision engine for the game, and will compare the cards given to it by the Manager Module.
	+ The Card Comparator Module is a singleton - there should only be one instance of it.
	+ The Card Comparator Module will create a new "Battle" to compare the cards given to it, and will maintain a history of these "Battles" until it is cleared by the Manager Module.
	+ The Card Comparator will return an enum of the outcome: either WIN, LOSE, or WAR.  The Manager Module will handle these outcomes, and will continue calling the Card Comparator with new cards until the Card Comparator returns a WIN or LOSE outcome.
	+ If no Win or LOSE outcome is encountered during a WAR and a single player or opponent has no more cards to draw (either in their Deck or the Claims pile), the player or opponent with cards remaining will win the game.
	+ If no WIN or LOSE outcome is encountered during a WAR and both players have no more cards to draw (either in their Deck or Claims pile), all cards played in the War will be returned to their respective owner and their Deck will be reshuffled, effectively aborting the WAR.




# BoggleBoard

It's America's favorite plastic block inside of a plastic tray with sand-timer game, *Now on PC*!

## Actually though...

I originally built this as sort of a fun project with Tries. While I've always read about them, I've never actually implemented one until 
now. Boggle seemed like a good way to run the Trie through it's paces, so here we are.

### To Use:
Simply call the static function *BoggleBoard.GetAllWords(**boardSize**, **board**, **filePath**)*, 

#### boardSize: 
representing N, of an **NxN** matrix.

#### board: 
an array of char, list of char, list of single character strings, array of single character strings, 
or an single NxN length string. 

#### filePath 
An optional parameter pointing to the Trie's input source. Currently this will default to 
'dictionary.txt' (included in the run-time directory), unless otherwise specified.

Please credit the author, me, Steve.

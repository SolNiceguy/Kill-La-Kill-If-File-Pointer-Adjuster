
# Kill-La-Kill-If-File-Pointer-Adjuster
  
[PointerAdjuster.zip](https://github.com/SolNiceguy/Kill-La-Kill-If-File-Pointer-Adjuster/files/7003080/PointerAdjuster.zip)  
Form1.cs has the all the code.  


Used for adding code to files that use pointers for the game Kill La Kill If. Automtically adjusts pointers depending on how much pointer code is added. 

script_character file functionality has been disabled since it's still a bit buggy.   


## Instructions
1. Drag file to "File Path" window 
3. "Middle Offset" is the offset where the pointer code ends and the bottom code begins. Will automatically fill in the default value for the file if left empty.  
2. Enter hex code into "Pointer Code" and "Bottom Code" windows.  
3. Apply
4. At offset 8 of your file, add how many entries you added (this is different for each file; eg for unit_BGM_Data.bin, the total number of entries is 9, where the first entry starts at offset 10, and each one has length 24)

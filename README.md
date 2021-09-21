
# Kill-La-Kill-If-File-Pointer-Adjuster
  
[PointerAdjuster.zip](https://github.com/SolNiceguy/Kill-La-Kill-If-File-Pointer-Adjuster/files/7003080/PointerAdjuster.zip)  
Form1.cs has the all the code.  


Used for adding code to files that use pointers for the game Kill La Kill If. Automtically adjusts pointers depending on how much pointer code is added. 

script_character file functionality has been disabled since it's still a bit buggy.   


## Instructions
1. Drag file to "File Path" window  
2. "Middle Offset" is the offset where the pointer code ends and the bottom code begins. Will automatically fill in the default value for the file if left empty.  
3. Enter hex code into "Pointer Code" and "Bottom Code" windows.  
4. Apply  
5. At offset 8 of your file, add how many entries you added. This is different for each file; eg for unit_Action_Data.bin, the entry total at offset 8 is 39, with the first entry starting at offset 10, and each one having length 84.  This can be found automatically be dragging a file in, then hitting apply.   
  MANUAL WAY:The fastest manual way to find entry length is to just eyeball it and look for stuff that repeats (eg for unit_Action_Data.bin, the pointer code starts with B0_01 at offset 10, so you can assume an entry has hex length 84, since that's the length to the next B0_01)  
  The surest manual way to find entry length is to find the length between the start of the entry/pointer code (generally at offset 10), and the start of the bottom code which is what the first pointer points to (for unit_Action_Data.bin, this is 2C140000, which points to 142C, which means the hex length would be 141C). Also generally the pointer code will look very different from the bottom code. Next, convert the hex length to decimal (I like to use https://www.rapidtables.com/convert/number/hex-to-decimal.html) (142C would be 5148), then divide that number by the total entries at offset 8 (5148/39 = 132), then convert back to hex (https://www.rapidtables.com/convert/number/decimal-to-hex.html) (132 becomes 84, which confirms what we found using the fast way).

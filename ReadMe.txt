Bellevue College C# Programming Certificate Capstone
====Requirements====
Create a connection to the SQL Server database using the Entity Framework database first approach.  
Load all the data into collections to be kept in memory while the application is running.  
Refer to the Additional Rules section for more details on the collections to be kept in memory while the application is running.  
The data in memory and in the database should always be the same.
====Features====
-Search Database by partial matches to Title, Author, Subject, or ISBN
-Login as a librarian
-Check out/in books
-Add books to the system (both existing and non-existing)
-Remove books
-Display database information:
	-Librarians,
	-Cardholders/Patrons
	-Authors
	-Overdue books
-Display search results and select individual books for detailed information.
-Separate menus for Cardholders and Librarians
-Display error messages for inappropriate interactions

====Comments====
-Uses polymorphism
-Employs Model View Controller Design Pattern
-WPF is used to provide user interface into the database.
-Abstract Data Structures are Enumerable (able to use for-each loops)
-ADTs use Properties to provide access into collections.
-ADTs use SortedSets to keep collections sorted.
-LINQ is used to enable queries into databases and obtain partial matches.
-Able to save database locally into an XML file.
-Uses a separate assembly to Save and Load into XML file.
-Contains some small animation in the menu design.
-Code is easily readable and commented.

====Future Changes====
-Separate UI (WPF) into a separate assembly from the Model components of the program
-PeopleList and CheckOutLogs should use SortedSet instead of List to provide O(logn) to maintain a sorted collection as well as O(logn) insertion and removal. 
 A HashSet could also be used concurrently with the same object references to provide O(1) retrieval.
-Pages should be used instead of separate Windows to provide a smoother user experience.
-More animations and art assets should be used to provide a better user experience.
-EMBD generated classes should have been used instead of using separate memory classes for interface.  
 This was done to fulfill the project’s polymorphism requirement.
-Factory Pattern should have been employed when loading from XML file.
-Command Pattern should have been used so Undo operations can be easily deployed in the future.
-Instead of a complicated Controller class, the Decorator pattern should have been used in order to provide functionality without changing existing classes.  
 (Requires providing better access to Model classes with Properties)

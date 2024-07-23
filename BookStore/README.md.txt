Problem Statement: Building a Basic Bookstore Web Application

Objective:
Develop a basic web application for managing a bookstore using ASP.NET Core. 
The application should allow users to perform CRUD (Create, Read, Update, Delete) operations on a list of books.
Requirements:

1.	Project Setup:
(done) o	Use ASP.NET Core to create the web application.
(done) o	Use Entity Framework Core for database operations.
(done) o	Use SQLite as the database provider for simplicity.


(done) 2.	Book Model:
o	Create a Book class with the following properties:
	Id (int, primary key)
	Title (string, required)
	Author (string, required)
	Genre (string, optional)
	Price (decimal, required)


(done) 3.	Database Context:
o	Create a BookstoreContext class that inherits from DbContext.
o	Configure the context to use SQLite.


(done) 4.	Controllers:
o	Create a BooksController to handle HTTP requests related to books.
o	Implement the following actions:
	Index (GET): Display a list of all books.
	Details (GET): Display details of a specific book by ID.
	Create (GET, POST): Add a new book.
	Edit (GET, POST): Edit an existing book.
	Delete (GET, POST): Delete a book.

(done) 5.	Views:
o	Use Razor Pages for the views.
o	Create views for each action in the BooksController (Index, Details, Create, Edit, Delete).


6.	Validation:
o	Implement basic validation for the Book model using data annotations.


7.	Bonus:
o	Add search functionality to filter books by title or author.
o	Implement pagination for the list of books.
o	Style the application using Bootstrap or any other CSS framework.


Deliverables:
•	A fully functional ASP.NET Core web application with source code.
•	A README file with instructions on how to run the application.


Evaluation Criteria:
•	Correctness and completeness of the implementation.
•	Code quality and organization.
•	Proper use of ASP.NET Core and Entity Framework Core features.
•	Usability and design of the web application

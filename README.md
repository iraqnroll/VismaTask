# Visma summer internship 2021 task
#### Usage example :
`VismaTask.exe [command to excecute] [additional argument 1] [additional argument 2] ...`
#### Commands :
- Display commands
  - `-h`
- Add a new book to the library
  - `-a [Book Name] [Author] [Category] [Language] [Date published (yyyy-MM-dd)] [ISBN]`
- Take a book from the library
  - `-t [Book ID] [Your name] [Your surname] [Return Date (yyyy-MM-dd)]`
- Return a book to the library
  - `-r [Book ID] [Your name] [Your surname]`
- List all books in the library with the specified filter
  - `-l [Author | Category | Language | ISBN | Name | Taken | Available] [Filter value (unrequired with some filters)]`
- Delete a book from the library
  - `-d [Book ID]`
 #### Examples :
 ```
 # Adding a book:
 -a "Harry Potter and the Philosopher's stone" "J.K. Rowling" "Fantasy" "English" "1998-06-26" "9781524721251"
 
 # Listing all books by author :
 -l "Author" "J.K. Rowling"
 
 # Listing all available books :
 -l "Available"
 
 # Deleting a book :
 -d 0
 ```

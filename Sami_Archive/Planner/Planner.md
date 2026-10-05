
# Sami archive 2
- Use Blazor
- Check ERD to setup database.
- Leave testing to the end - or when the project is more fleshed out.
- App service deployment on Azure.


Steps for project
- New solution.
- Setup authentication.
- Setup database.
- Setup Blazor.
- 


# Task:
## Tasks
- Add ratings
- Add comments
- Add favourite
- Share button

### Change UI
- Personal [stuff placeholder] - Books, genres, authors
- Public - Seeing other peoples work, published reviews


## Currently going to do later
- Implementing email confirmation flow for user


## What to implement
- Roles (admin roles, user roles)
- Users can manage their account, change settings


## In order of task:
- Email provider integration: setting up a service to handle sending the registration conformation and password reset emails.
- Account recovery: Building a secure "Forgot Password" flow.
- Registration and Login Flow: Users are able to make accounts, but it needs to be authenticated and verified. [Partial done]


1. Expanding "Roles" (Authorization)
- Use policy based authentication - by creating policies what each role can do, it becomes easier to moderate or manage roles later.
- Implement a admin dashboard - Place that admin can view all users, assign roles, banning/suspending accounts and toggle user roles.

2. Expanding "Account Management"
Changing account settings needs to be done in specific flows;
- Profile data: Updating basic information (first name, last name, email, etc)
- Security settings: Changing password, enabling 2FA, managing sessions
- Email/Data workflows: If the user changes their email, the project needs to send a verification link to the new address before applying the change
- Data Privacy: Include features for users to download their data and a hard "delete my account" button.




## Redesign homepage. Start small
- Have users name, saying "welcome back, [username]".
- They have some sort of database to create or archieve their own books from the shared collection.
-	- They see the list of books, press add, and the books information will also show on their content page/collection page



## Other ideas
- Idea - Content: I like the idea that they have their own library, and can see their books, and authors they follow.

## Author List 
- When a author box is clicked, I want the user to be taken to a details page and is shown a list of books the author is apart off [Done]
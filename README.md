# PasswordManager - a console Password Manager in .NET

## Instructions

### 1.1. Run
- For Visual Studio Code: In the terminal, navigate to PasswordManager -> `dotnet run`
![Image](https://github.com/Minaac-ops/PasswordManager/assets/72027505/f463fcb1-77e0-499b-8b82-b3af10d81fe2)

- For Rider: Right-click on the console application (Password Manager) and choose 'Run PasswordManager'

### 1.2. Authenticate/Authorize
- Create a new login or log in to your vault as an existing user

## Documentation of the Product

### "Start Page"
![Image](https://github.com/Minaac-ops/PasswordManager/assets/72027505/bd44a4ff-0d5f-47af-bd95-0676722199a3)

### Menu
![Image](https://github.com/Minaac-ops/PasswordManager/assets/72027505/54ae20ba-37ef-4094-9076-a9107823b351)

### Creating a New Item in the Vault
- Before entering the password, you will be asked if you want your password generated for you
![Image](https://github.com/Minaac-ops/PasswordManager/assets/72027505/9d6b03a7-28d2-464d-9f86-9ad43c8db1fe)

### Viewing Items with Successful Decryption of Passwords
- Example of auto-generated passwords
![Image](https://github.com/Minaac-ops/PasswordManager/assets/72027505/bdb26455-97f9-4cd7-ab9e-a24aea79f6ba)

## Discussion

Password Managers typically use a master password and a secret key to authenticate users when attempting to log in to their vault. The secret key mentioned is generated when an account is created and associated with that account, providing an additional layer of security beyond a master password.

In this case, users need only use their master password. Another option is to use alternative forms of two-factor authentication, such as a code sent to an email or phone number associated with the user attempting to access the vault.

In any case, a strong master password is essential, as it is the easiest way to gain access. During the creation of new users, checks are made on the master password to ensure a combination of uppercase and lowercase letters, digits, special characters, and a minimum length of 9 characters.

The password undergoes a cryptographic process before the user credentials are stored in the database, ensuring that no plain text passwords are stored.

- Generating a random salt unique to each user and part of the user's credentials.
- Using an instance of Rfc2898DeriveBytes for key derivation and deriving a cryptographic key from the user's password combined with the random salt. A high iteration increases the time required to perform a brute-force attack.
- Finally, generating the password hash.

The unique salt is also used for encrypting and decrypting passwords in a vault, providing additional security so that you cannot read plain text passwords without the owner's unique salt once you have accessed a vault.

Strong hashing algorithms and key derivation techniques have been used to ensure the secure storage of data, preventing unauthorized access to plain text passwords.

A MongoDB database is used to store data, and while MongoDB has built-in security measures, such as specifying network access, they have not been used in this case. Error handling is minimal, but where implemented, it has been designed to provide consistent messages to the user. For example, during login, the user will not know whether it's the username or password that is incorrect
